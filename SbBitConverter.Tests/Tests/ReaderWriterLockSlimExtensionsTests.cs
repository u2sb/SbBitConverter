// 补充覆盖：对应 Sb.Extensions\System\Threading\ReaderWriterLockSlimExtensions.cs
using Sb.Extensions.System.Threading;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   ReaderWriterLockSlim 拓展测试
/// </summary>
public class ReaderWriterLockSlimExtensionsTests
{
  // ── Enter 作用域 ──

  [Fact]
  public void EnterReadLockScope_EnterAndExit()
  {
    using var locker = new ReaderWriterLockSlim();
    using (locker.EnterReadLockScope())
    {
      Assert.True(locker.IsReadLockHeld);
    }

    Assert.False(locker.IsReadLockHeld);
  }

  [Fact]
  public void EnterWriteLockScope_EnterAndExit()
  {
    using var locker = new ReaderWriterLockSlim();
    using (locker.EnterWriteLockScope())
    {
      Assert.True(locker.IsWriteLockHeld);
    }

    Assert.False(locker.IsWriteLockHeld);
  }

  [Fact]
  public void EnterUpgradeableReadLockScope_EnterAndExit()
  {
    using var locker = new ReaderWriterLockSlim();
    using (locker.EnterUpgradeableReadLockScope())
    {
      Assert.True(locker.IsUpgradeableReadLockHeld);
      // 升级读锁中可以再获取写锁
      using (locker.EnterWriteLockScope())
      {
        Assert.True(locker.IsWriteLockHeld);
      }
    }

    Assert.False(locker.IsUpgradeableReadLockHeld);
  }

  [Fact]
  public void EnterReadLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.EnterReadLockScope());
  }

  [Fact]
  public void EnterWriteLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.EnterWriteLockScope());
  }

  [Fact]
  public void EnterUpgradeableReadLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.EnterUpgradeableReadLockScope());
  }

  // ── TryEnter（TimeSpan 重载） ──

  [Fact]
  public void TryEnterReadLockScope_TimeSpan_Success()
  {
    using var locker = new ReaderWriterLockSlim();
    var taken = locker.TryEnterReadLockScope(TimeSpan.FromSeconds(1), out var scope);
    Assert.True(taken);
    Assert.True(locker.IsReadLockHeld);
    scope.Dispose();
    Assert.False(locker.IsReadLockHeld);
  }

  [Fact]
  public async Task TryEnterReadLockScope_TimeSpan_Failure_ReturnsFalseAndDefaultScope()
  {
    using var locker = new ReaderWriterLockSlim();
    using var holderScope = await HoldWriteLockOnOtherThreadAsync(locker);

    var taken = locker.TryEnterReadLockScope(TimeSpan.FromMilliseconds(10), out var scope);
    Assert.False(taken);
    Assert.Equal(default, scope); // 默认作用域 Dispose 安全无副作用
  }

  [Fact]
  public void TryEnterWriteLockScope_TimeSpan_Success()
  {
    using var locker = new ReaderWriterLockSlim();
    var taken = locker.TryEnterWriteLockScope(TimeSpan.FromSeconds(1), out var scope);
    Assert.True(taken);
    Assert.True(locker.IsWriteLockHeld);
    scope.Dispose();
    Assert.False(locker.IsWriteLockHeld);
  }

  [Fact]
  public async Task TryEnterWriteLockScope_TimeSpan_Failure_ScopeSafeToDispose()
  {
    using var locker = new ReaderWriterLockSlim();
    using var holderScope = await HoldWriteLockOnOtherThreadAsync(locker);

    var taken = locker.TryEnterWriteLockScope(TimeSpan.FromMilliseconds(10), out var scope);
    Assert.False(taken);
    scope.Dispose(); // 不应抛出 SynchronizationLockException
  }

  [Fact]
  public void TryEnterUpgradeableReadLockScope_TimeSpan_Success()
  {
    using var locker = new ReaderWriterLockSlim();
    var taken = locker.TryEnterUpgradeableReadLockScope(TimeSpan.FromSeconds(1), out var scope);
    Assert.True(taken);
    Assert.True(locker.IsUpgradeableReadLockHeld);
    scope.Dispose();
    Assert.False(locker.IsUpgradeableReadLockHeld);
  }

  // ── TryEnter（毫秒重载） ──

  [Fact]
  public void TryEnterReadLockScope_Milliseconds_Success()
  {
    using var locker = new ReaderWriterLockSlim();
    var taken = locker.TryEnterReadLockScope(1000, out var scope);
    Assert.True(taken);
    Assert.True(locker.IsReadLockHeld);
    scope.Dispose();
  }

  [Fact]
  public async Task TryEnterWriteLockScope_Milliseconds_Failure()
  {
    using var locker = new ReaderWriterLockSlim();
    using var holderScope = await HoldWriteLockOnOtherThreadAsync(locker);

    var taken = locker.TryEnterWriteLockScope(10, out var scope);
    Assert.False(taken);
    scope.Dispose();
  }

  [Fact]
  public void TryEnterUpgradeableReadLockScope_Milliseconds_Success()
  {
    using var locker = new ReaderWriterLockSlim();
    var taken = locker.TryEnterUpgradeableReadLockScope(1000, out var scope);
    Assert.True(taken);
    Assert.True(locker.IsUpgradeableReadLockHeld);
    scope.Dispose();
  }

  [Fact]
  public void TryEnterReadLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.TryEnterReadLockScope(10, out _));
  }

  [Fact]
  public void TryEnterWriteLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.TryEnterWriteLockScope(TimeSpan.Zero, out _));
  }

  [Fact]
  public void TryEnterUpgradeableReadLockScope_NullLocker_ThrowsArgumentNullException()
  {
    ReaderWriterLockSlim locker = null!;
    Assert.Throws<ArgumentNullException>(() => locker.TryEnterUpgradeableReadLockScope(10, out _));
  }

  // ── 辅助 ──

  /// <summary>
  ///   在其他线程上持有写锁，直到返回的 IDisposable 被释放
  /// </summary>
  private static async Task<IDisposable> HoldWriteLockOnOtherThreadAsync(ReaderWriterLockSlim locker)
  {
    var held = new TaskCompletionSource();
    var release = new TaskCompletionSource();

    var holder = Task.Run(() =>
    {
      using (locker.EnterWriteLockScope())
      {
        held.SetResult();
        release.Task.Wait();
      }
    });

    await held.Task.WaitAsync(TimeSpan.FromSeconds(3));
    return new ReleaseAction(release, holder);
  }

  private sealed class ReleaseAction(TaskCompletionSource release, Task holder) : IDisposable
  {
    public void Dispose()
    {
      release.TrySetResult();
      holder.Wait(3000);
    }
  }

  // ── 并发读 / 写 ──

  [Fact]
  public async Task ReadScopes_FromMultipleThreads_AllHeldSimultaneously()
  {
    using var locker = new ReaderWriterLockSlim();
    using var barrier = new Barrier(2);

    var tasks = Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
    {
      using (locker.EnterReadLockScope()) { }
      barrier.SignalAndWait(TimeSpan.FromSeconds(3)); // 两个读锁必须同时持有
    })).ToArray();

    await Task.WhenAll(tasks);
  }
}
