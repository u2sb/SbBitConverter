// 补充覆盖：对应 Sb.Extensions\System\Threading\AsyncLock.cs
using Sb.Extensions.System.Threading;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   AsyncLock 测试（所有用例总耗时远小于 5 秒）
/// </summary>
public class AsyncLockTests
{
  // ── 基本获取 / 释放 ──

  [Fact]
  public void Lock_AndDispose_AllowsReacquire()
  {
    var locker = new AsyncLock();
    var first = locker.Lock();
    first.Dispose();
    var second = locker.Lock(); // 已释放，可再次获取
    second.Dispose();
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_FastPath_ReturnsSynchronously()
  {
    var locker = new AsyncLock();
    using var handle = await locker.LockAsync();
    Assert.True(handle.DisposeAsync().IsCompletedSuccessfully); // 已持有，同步完成
    await handle.DisposeAsync();
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_DisposeAsync_Releases()
  {
    var locker = new AsyncLock();
    var handle = await locker.LockAsync();
    await handle.DisposeAsync();
    using var next = await locker.LockAsync();
    locker.Dispose();
  }

  // ── 可重入 ──

  [Fact]
  public void Lock_Reentrant_SameContext()
  {
    var locker = new AsyncLock();
    var outer = locker.Lock();
    var inner = locker.Lock(); // 同一上下文重入
    inner.Dispose();
    outer.Dispose();
    var again = locker.Lock(); // 全部释放后可重新获取
    again.Dispose();
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_Reentrant_Nested()
  {
    var locker = new AsyncLock();
    await using var outer = await locker.LockAsync();
    await using var inner = await locker.LockAsync(); // AsyncLocal 记录持有者
    locker.Dispose();
  }

  // ── 互斥 ──

  [Fact]
  public async Task Lock_BlocksSecondAcquirer_UntilReleased()
  {
    var locker = new AsyncLock();
    var acquired = new TaskCompletionSource();
    var second = Task.Run(() =>
    {
      using (locker.Lock()) { }
      acquired.SetResult();
    });

    using (locker.Lock())
    {
      await Task.Delay(50); // 给第二个任务时间去竞争
      Assert.False(acquired.Task.IsCompleted); // 仍被阻塞
    }

    // 释放后第二个任务才能获得锁
    await acquired.Task.WaitAsync(TimeSpan.FromSeconds(3));
    await second;
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_Contention_SlowPath_AcquiresAfterRelease()
  {
    var locker = new AsyncLock();
    var innerAcquired = new TaskCompletionSource();
    var second = Task.Run(async () =>
    {
      await using (await locker.LockAsync()) { } // 慢路径
      innerAcquired.SetResult();
    });

    await using (await locker.LockAsync())
    {
      await Task.Delay(50);
      Assert.False(innerAcquired.Task.IsCompleted);
    }

    await innerAcquired.Task.WaitAsync(TimeSpan.FromSeconds(3)); // outer 释放后被唤醒
    await second;
    locker.Dispose();
  }

  [Fact]
  public async Task Lock_MutualExclusion_ProtectsCounter()
  {
    var locker = new AsyncLock();
    var counter = 0;

    var tasks = Enumerable.Range(0, 8).Select(_ => Task.Run(() =>
    {
      for (var i = 0; i < 100; i++)
      {
        using (locker.Lock()) { }
        counter++;
      }
    })).ToArray();

    await Task.WhenAll(tasks);
    Assert.Equal(800, counter);
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_MutualExclusion_ProtectsCounter()
  {
    var locker = new AsyncLock();
    var counter = 0;

    var tasks = Enumerable.Range(0, 8).Select(async _ =>
    {
      for (var i = 0; i < 100; i++)
      {
        await using var handle = await locker.LockAsync();
        counter++;
      }
    }).ToArray();

    await Task.WhenAll(tasks);
    Assert.Equal(800, counter);
    locker.Dispose();
  }

  // ── 取消 ──

  [Fact]
  public async Task LockAsync_CancelledWhileWaiting_ThrowsOperationCanceled()
  {
    var locker = new AsyncLock();
    var held = new TaskCompletionSource();
    var release = new TaskCompletionSource();

    // 由其他异步上下文持有锁，避免主线程可重入快速路径
    var holder = Task.Run(async () =>
    {
      await using (await locker.LockAsync())
      {
        held.SetResult();
        await release.Task;
      }
    });

    await held.Task;
    using var cts = new CancellationTokenSource();
    cts.Cancel();
    await Assert.ThrowsAnyAsync<OperationCanceledException>(
      async () => await locker.LockAsync(cts.Token));

    release.SetResult();
    await holder;
    locker.Dispose();
  }

  [Fact]
  public void Lock_CancelledWhileWaiting_ThrowsOperationCanceled()
  {
    var locker = new AsyncLock();
    var held = new TaskCompletionSource();
    var release = new TaskCompletionSource();

    // 由其他线程持有锁，避免同线程可重入快速路径
    var holder = Task.Run(() =>
    {
      using (locker.Lock())
      {
        held.SetResult();
        release.Task.Wait();
      }
    });

    held.Task.Wait(3000);
    using var cts = new CancellationTokenSource();
    cts.Cancel();
    Assert.ThrowsAny<OperationCanceledException>(() => locker.Lock(cts.Token));

    release.SetResult();
    holder.Wait(3000);
    locker.Dispose();
  }

  // ── Dispose ──

  [Fact]
  public void Dispose_Twice_IsNoOp()
  {
    var locker = new AsyncLock();
    locker.Dispose();
    locker.Dispose();
  }

  [Fact]
  public async Task LockAsync_AfterDispose_ThrowsObjectDisposed()
  {
    var locker = new AsyncLock();
    locker.Dispose();
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await locker.LockAsync());
  }

  [Fact]
  public void Lock_AfterDispose_ThrowsObjectDisposed()
  {
    var locker = new AsyncLock();
    locker.Dispose();
    Assert.Throws<ObjectDisposedException>(() => locker.Lock());
  }
}
