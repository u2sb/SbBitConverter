using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Sb.Extensions.System.Threading;

/// <summary>
///   高性能异步锁，支持可重入。
///   无竞争时 CAS 原子获取（~15ns），零分配；竞争时 SemaphoreSlim 信号唤醒。
/// </summary>
public sealed class AsyncLock : IDisposable
{
  private readonly SemaphoreSlim _gate = new(1, 1);
  private readonly SemaphoreSlim _signal = new(0, int.MaxValue);
  private long _ownerKey;
  private int _depth;
  private long _nextKey = 1;
  private int _disposed;

  private static readonly AsyncLocal<long> AsyncId = new();

  /// <inheritdoc />
  public void Dispose()
  {
    if (Interlocked.Exchange(ref _disposed, 1) == 1) return;

    _gate.Wait();
    try
    {
      _signal.Dispose();
    }
    finally
    {
      _gate.Dispose();
    }
  }

  private void ThrowIfDisposed()
  {
    if (Volatile.Read(ref _disposed) == 1)
      throw new ObjectDisposedException(nameof(AsyncLock));
  }

  /// <summary>
  ///   异步获取锁。无竞争时同步返回，零分配。
  /// </summary>
  public ValueTask<InnerLock> LockAsync(CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    var oldKey = AsyncId.Value;
    var newKey = Interlocked.Increment(ref _nextKey);
    AsyncId.Value = newKey;

    var inner = new InnerLock(this, oldKey, newKey);

    // 快速路径 1：可重入（当前上下文已持有锁）
    if (oldKey != 0 && Volatile.Read(ref _ownerKey) == oldKey)
    {
      Interlocked.Increment(ref _depth);
      _ownerKey = newKey;
      return new ValueTask<InnerLock>(inner);
    }

    // 快速路径 2：CAS 无锁获取（零分配，仅一次原子操作）
    if (Interlocked.CompareExchange(ref _ownerKey, newKey, 0) == 0)
    {
      _depth = 1;
      return new ValueTask<InnerLock>(inner);
    }

    // 慢路径：SemaphoreSlim 信号等待
    return SlowLockAsync(inner, newKey, cancellationToken);
  }

  /// <summary>
  ///   同步获取锁。
  /// </summary>
  public InnerLock Lock(CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    var oldKey = AsyncId.Value;
    var newKey = Interlocked.Increment(ref _nextKey);
    AsyncId.Value = newKey;

    var inner = new InnerLock(this, oldKey, newKey);

    // 快速路径 1：已持有锁（可重入）
    if (oldKey != 0 && Volatile.Read(ref _ownerKey) == oldKey)
    {
      Interlocked.Increment(ref _depth);
      _ownerKey = newKey;
      return inner;
    }

    // 快速路径 2：CAS 无锁获取
    if (Interlocked.CompareExchange(ref _ownerKey, newKey, 0) == 0)
    {
      _depth = 1;
      return inner;
    }

    while (true)
    {
      _gate.Wait(cancellationToken);
      if (TryEnter(newKey, oldKey))
      {
        _gate.Release();
        return inner;
      }

      _gate.Release();
      _signal.Wait(cancellationToken);
    }
  }

  private static async ValueTask<InnerLock> SlowLockAsync(InnerLock inner, long newKey, CancellationToken ct)
  {
    while (true)
    {
      await inner.Parent._gate.WaitAsync(ct).ConfigureAwait(false);
      if (inner.Parent.TryEnter(newKey, inner.OldKey))
      {
        inner.Parent._gate.Release();
        return inner;
      }

      inner.Parent._gate.Release();
      await inner.Parent._signal.WaitAsync(ct).ConfigureAwait(false);
    }
  }

  private bool TryEnter(long newKey, long oldKey)
  {
    if (_ownerKey == 0)
    {
      _ownerKey = newKey;
      _depth = 1;
      return true;
    }

    if (_ownerKey == oldKey)
    {
      _ownerKey = newKey;
      _depth++;
      return true;
    }

    return false;
  }

  /// <summary>
  ///   释放锁。必须在持有 _gate 时调用。
  /// </summary>
  internal void ReleaseLock(long oldKey, long newKey)
  {
    // 防护双重释放（包括 struct 复制场景）：_depth == 0 表示锁未被持有
    if (_depth == 0) return;

    // 所有权校验：防止旧 InnerLock 副本窃取他人持有的锁
    if (_ownerKey != newKey) return;

    _depth--;
    if (_depth == 0)
    {
      _ownerKey = 0;
      _signal.Release();
    }
    else
    {
      _ownerKey = oldKey;
    }
  }

  #region InnerLock

  /// <summary>
  ///   锁的句柄，通过 <see cref="IDisposable.Dispose" /> 或 <see cref="IAsyncDisposable.DisposeAsync" /> 释放。
  /// </summary>
  public struct InnerLock : IDisposable, IAsyncDisposable
  {
    internal readonly AsyncLock Parent;
    internal readonly long OldKey;
    internal readonly long NewKey;

    internal InnerLock(AsyncLock parent, long oldKey, long newKey)
    {
      Parent = parent;
      OldKey = oldKey;
      NewKey = newKey;
    }

    /// <inheritdoc />
    public void Dispose()
    {
      var parent = Parent;
      try
      {
        parent._gate.Wait();
      }
      catch (ObjectDisposedException)
      {
        return;
      }

      try
      {
        parent.ReleaseLock(OldKey, NewKey);
      }
      finally
      {
        parent._gate.Release();
      }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
      var parent = Parent;
      try
      {
        await parent._gate.WaitAsync().ConfigureAwait(false);
      }
      catch (ObjectDisposedException)
      {
        return;
      }

      try
      {
        parent.ReleaseLock(OldKey, NewKey);
      }
      finally
      {
        parent._gate.Release();
      }
    }
  }

  #endregion
}

