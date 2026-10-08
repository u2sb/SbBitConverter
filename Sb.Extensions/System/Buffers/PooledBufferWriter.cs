using System;
using System.Buffers;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace Sb.Extensions.System.Buffers;

/// <summary>
///   池化的 <see cref="IBufferWriter{T}" /> 实现
///   Dispose 时自动归还数组到池。
///   一定要记得 Dispose，或者在 using 中使用
/// </summary>
public sealed class PooledBufferWriter(int minimumLength = 1024) : IBufferWriter<byte>, IDisposable
{
  private bool _disposed;

  private byte[] _buffer = BestArrayPool.Rent(minimumLength);

  /// <summary>已写入部分的内存视图。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public ReadOnlyMemory<byte> WrittenMemory
  {
    get
    {
      ThrowIfDisposed();
      return RawBuffer.AsMemory(0, WrittenCount);
    }
  }

  /// <summary>已写入部分的 Span 视图。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public ReadOnlySpan<byte> WrittenSpan
  {
    get
    {
      ThrowIfDisposed();
      return RawBuffer.AsSpan(0, WrittenCount);
    }
  }

  /// <summary>已写入的字节数。</summary>
  public int WrittenCount { get; private set; }

  /// <summary>底层数组缓冲区。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public byte[] RawBuffer
  {
    get
    {
      ThrowIfDisposed();
      return _buffer;
    }
    private set => _buffer = value;
  }

  /// <summary>将已写入的字节数增加 <paramref name="count" />。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public void Advance(int count)
  {
    ThrowIfDisposed();
    WrittenCount += count;
  }

  /// <summary>获取可写内存。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public Memory<byte> GetMemory(int sizeHint = 0)
  {
    ThrowIfDisposed();
    EnsureCapacity(sizeHint);
    return RawBuffer.AsMemory(WrittenCount);
  }

  /// <summary>获取可写 Span。</summary>
  /// <exception cref="ObjectDisposedException">实例已释放。</exception>
  public Span<byte> GetSpan(int sizeHint = 0)
  {
    ThrowIfDisposed();
    EnsureCapacity(sizeHint);
    return RawBuffer.AsSpan(WrittenCount);
  }

  /// <summary>释放资源，归还底层数组到池。</summary>
  public void Dispose()
  {
    if (_buffer != null!)
    {
      BestArrayPool.Return(_buffer);
      _buffer = null!;
    }

    _disposed = true;
  }

  private void EnsureCapacity(int sizeHint)
  {
    var required = WrittenCount + Math.Max(sizeHint, 1);
    if (required <= RawBuffer.Length)
    {
      return;
    }

    var newSize = Math.Max(RawBuffer.Length * 2, required);
    var newBuffer = BestArrayPool.Rent(newSize);
    RawBuffer.AsSpan(0, WrittenCount).CopyTo(newBuffer);
    BestArrayPool.Return(RawBuffer);
    RawBuffer = newBuffer;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void ThrowIfDisposed()
  {
    if (_disposed)
    {
      throw new ObjectDisposedException(nameof(PooledBufferWriter));
    }
  }
}
