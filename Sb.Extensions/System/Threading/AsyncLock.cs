// https://raw.githubusercontent.com/neosmart/AsyncLock/refs/heads/master/AsyncLock/AsyncLock.cs

using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Sb.Extensions.System.Threading;

/// <summary>
/// </summary>
public sealed class AsyncLock : IDisposable
{
  internal const long UnlockedId = 0x00; // "owning" task id when unlocked

  private static long _asyncStackCounter;

  // An AsyncLocal<T> is not really the task-based equivalent to a ThreadLocal<T>, in that
  // it does not track the async flow (as the documentation describes) but rather it is
  // associated with a stack snapshot. Mutation of the AsyncLocal in an await call does
  // not change the value observed by the parent when the call returns, so if you want to
  // use it as a persistent async flow identifier, the value needs to be set at the outer-
  // most level and never touched internally.

  // ReSharper disable once InconsistentNaming
  private static readonly AsyncLocal<long> _asyncId = new();
  internal readonly SemaphoreSlim Reentrancy = new(1, 1);

  // We are using this SemaphoreSlim like a posix condition variable.
  // We only want to wake waiters, one or more of whom will try to obtain
  // a different lock to do their thing. So long as we can guarantee no
  // wakes are missed, the number of awakees is not important.
  // Ideally, this would be "friend" for access only from InnerLock, but
  // whatever.
  internal readonly SemaphoreSlim Retry = new(0, 1);

  private int _disposed;
  internal long OwningId = UnlockedId;
  internal long OwningThreadId = UnlockedId;

  internal int Reentrances;

  internal static long AsyncId => _asyncId.Value;
  internal static int ThreadId => Environment.CurrentManagedThreadId;

  /// <inheritdoc />
  public void Dispose()
  {
    if (Interlocked.Exchange(ref _disposed, 1) == 1) return;

    Reentrancy.Dispose();
    Retry.Dispose();
  }

  private void ThrowIfDisposed()
  {
    if (Volatile.Read(ref _disposed) == 1)
      throw new ObjectDisposedException(nameof(AsyncLock), "AsyncLock has been disposed and can no longer be used.");
  }

  /// <summary>
  ///   Make sure InnerLock.LockAsync() does not use await, because an async function triggers a snapshot of
  ///   the AsyncLocal value.
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public ValueTask<InnerLock> LockAsync(CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    var @lock = new InnerLock(this, AsyncId, ThreadId);
    _asyncId.Value = Interlocked.Increment(ref _asyncStackCounter);
    return @lock.ObtainLockAsync(cancellationToken);
  }

  /// <summary>
  ///   Lock
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public InnerLock Lock(CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    var @lock = new InnerLock(this, AsyncId, ThreadId);
    // Increment the async stack counter to prevent a child task from getting
    // the lock at the same time as a child thread.
    _asyncId.Value = Interlocked.Increment(ref _asyncStackCounter);
    return @lock.ObtainLock(cancellationToken);
  }
}

#region InnerLock

/// <summary>
/// </summary>
public readonly struct InnerLock : IDisposable, IAsyncDisposable
{
  private readonly AsyncLock _parent;
  private readonly long _oldId;
  private readonly int _oldThreadId;

  internal InnerLock(AsyncLock parent, long oldId, int oldThreadId)
  {
    _parent = parent;
    _oldId = oldId;
    _oldThreadId = oldThreadId;
  }

  internal ValueTask<InnerLock> ObtainLockAsync(CancellationToken cancellationToken = default)
  {
    if (_parent.Reentrancy.Wait(0))
    {
      if (InnerTryEnter())
      {
        Interlocked.Exchange(ref _parent.OwningThreadId, AsyncLock.ThreadId);
        _parent.Reentrancy.Release();
        return new ValueTask<InnerLock>(this);
      }

      _parent.Reentrancy.Release();
    }

    return SlowPath(this, cancellationToken);
  }

  private static async ValueTask<InnerLock> SlowPath(InnerLock @lock, CancellationToken ct)
  {
    while (true)
    {
      await @lock._parent.Reentrancy.WaitAsync(ct).ConfigureAwait(false);
      if (@lock.InnerTryEnter()) break;

      var waitTask = @lock._parent.Retry.WaitAsync(ct).ConfigureAwait(false);
      @lock._parent.Reentrancy.Release();
      await waitTask;
    }

    Interlocked.Exchange(ref @lock._parent.OwningThreadId, AsyncLock.ThreadId);
    @lock._parent.Reentrancy.Release();
    return @lock;
  }

  internal InnerLock ObtainLock(CancellationToken cancellationToken = default)
  {
    while (true)
    {
      _parent.Reentrancy.Wait(cancellationToken);
      if (InnerTryEnter(true))
      {
        _parent.Reentrancy.Release();
        break;
      }

      // We need to wait for someone to leave the lock before trying again.
      var waitTask = _parent.Retry.WaitAsync(cancellationToken);
      _parent.Reentrancy.Release();
      // This should be safe since the task we are awaiting doesn't need to make progress
      // itself to complete - it will be completed by another thread altogether. cf SemaphoreSlim internals.
      waitTask.GetAwaiter().GetResult();
    }

    return this;
  }

  private bool InnerTryEnter(bool synchronous = false)
  {
    if (synchronous)
    {
      var owningThreadId = Interlocked.Read(ref _parent.OwningThreadId);
      if (owningThreadId == AsyncLock.UnlockedId)
        owningThreadId =
          Interlocked.CompareExchange(ref _parent.OwningThreadId, AsyncLock.ThreadId, AsyncLock.UnlockedId);

      if (owningThreadId != AsyncLock.UnlockedId && owningThreadId != AsyncLock.ThreadId) return false;
      Interlocked.Exchange(ref _parent.OwningId, AsyncLock.AsyncId);
    }
    else
    {
      var owningId = Interlocked.Read(ref _parent.OwningId);
      if (owningId == AsyncLock.UnlockedId)
      {
        if (Interlocked.CompareExchange(ref _parent.OwningId, AsyncLock.AsyncId, AsyncLock.UnlockedId) !=
            AsyncLock.UnlockedId)
          return false;
      }
      else if (owningId != _oldId)
        // Another thread currently owns the lock
      {
        return false;
      }
      else
        // Nested re-entrance
      {
        Interlocked.Exchange(ref _parent.OwningId, AsyncLock.AsyncId);
      }
    }

    // We can go in
    _parent.Reentrances += 1;
    return true;
  }

  /// <inheritdoc />
  public void Dispose()
  {
    var @this = this;
    @this._parent.Reentrancy.Wait();
    Release(ref @this);
  }

  /// <inheritdoc />
  public async ValueTask DisposeAsync()
  {
    var @this = this;
    await @this._parent.Reentrancy.WaitAsync();
    Release(ref @this);
  }

  private void Release(ref InnerLock @this)
  {
    var oldId = _oldId;
    var oldThreadId = _oldThreadId;

    try
    {
      @this._parent.Reentrances -= 1;

      if (@this._parent.Reentrances == 0)
      {
        Interlocked.Exchange(ref @this._parent.OwningId, AsyncLock.UnlockedId);
        Interlocked.Exchange(ref @this._parent.OwningThreadId, AsyncLock.UnlockedId);
      }
      else
      {
        Interlocked.Exchange(ref @this._parent.OwningId, oldId);
        Interlocked.Exchange(ref @this._parent.OwningThreadId, oldThreadId);
      }

      if (@this._parent.Retry.CurrentCount == 0) @this._parent.Retry.Release();
    }
    finally
    {
      @this._parent.Reentrancy.Release();
    }
  }
}

#endregion
