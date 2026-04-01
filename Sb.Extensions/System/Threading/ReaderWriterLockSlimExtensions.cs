using System;
using System.Threading;

namespace Sb.Extensions.System.Threading;

/// <summary>
///   ReaderWriterLockSlim拓展
/// </summary>
public static class ReaderWriterLockSlimExtensions
{
  extension(ReaderWriterLockSlim locker)
  {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public ReadLockScope EnterReadLockScope()
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      locker.EnterReadLock();
      return new ReadLockScope(locker, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteLockScope EnterWriteLockScope()
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      locker.EnterWriteLock();
      return new WriteLockScope(locker, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public UpgradeableReadLockScope EnterUpgradeableReadLockScope()
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      locker.EnterUpgradeableReadLock();
      return new UpgradeableReadLockScope(locker, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterReadLockScope(TimeSpan timeout, out ReadLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterReadLock(timeout);
      scope = lockTaken ? new ReadLockScope(locker, lockTaken) : default;
      return lockTaken;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterWriteLockScope(TimeSpan timeout,
      out WriteLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterWriteLock(timeout);
      scope = lockTaken ? new WriteLockScope(locker, lockTaken) : default;
      return lockTaken;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterUpgradeableReadLockScope(TimeSpan timeout,
      out UpgradeableReadLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterUpgradeableReadLock(timeout);
      scope = lockTaken ? new UpgradeableReadLockScope(locker, lockTaken) : default;
      return lockTaken;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="millisecondsTimeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterReadLockScope(int millisecondsTimeout,
      out ReadLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterReadLock(millisecondsTimeout);
      scope = lockTaken ? new ReadLockScope(locker, lockTaken) : default;
      return lockTaken;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="millisecondsTimeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterWriteLockScope(int millisecondsTimeout,
      out WriteLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterWriteLock(millisecondsTimeout);
      scope = lockTaken ? new WriteLockScope(locker, lockTaken) : default;
      return lockTaken;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="millisecondsTimeout"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryEnterUpgradeableReadLockScope(int millisecondsTimeout,
      out UpgradeableReadLockScope scope)
    {
      if (locker == null)
      {
        throw new ArgumentNullException(nameof(locker));
      }

      var lockTaken = locker.TryEnterUpgradeableReadLock(millisecondsTimeout);
      scope = lockTaken ? new UpgradeableReadLockScope(locker, lockTaken) : default;
      return lockTaken;
    }
  }

  /// <summary>
  ///   锁区域
  /// </summary>
  public readonly struct ReadLockScope : IDisposable
  {
    private readonly ReaderWriterLockSlim _locker;
    private readonly bool _lockTaken = false;

    internal ReadLockScope(ReaderWriterLockSlim locker, bool lockTaken)
    {
      _locker = locker;
      _lockTaken = lockTaken;
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_lockTaken && _locker is not null)
      {
        _locker.ExitReadLock();
      }
    }
  }

  /// <summary>
  ///   锁区域
  /// </summary>
  public readonly struct WriteLockScope : IDisposable
  {
    private readonly ReaderWriterLockSlim _locker;
    private readonly bool _lockTaken = false;

    internal WriteLockScope(ReaderWriterLockSlim locker, bool lockTaken)
    {
      _locker = locker;
      _lockTaken = lockTaken;
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_lockTaken && _locker is not null)
      {
        _locker.ExitWriteLock();
      }
    }
  }


  /// <summary>
  ///   锁区域
  /// </summary>
  public readonly struct UpgradeableReadLockScope : IDisposable
  {
    private readonly ReaderWriterLockSlim _locker;
    private readonly bool _lockTaken = false;

    internal UpgradeableReadLockScope(ReaderWriterLockSlim locker, bool lockTaken)
    {
      _locker = locker;
      _lockTaken = lockTaken;
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_lockTaken && _locker is not null)
      {
        _locker.ExitUpgradeableReadLock();
      }
    }
  }
}
