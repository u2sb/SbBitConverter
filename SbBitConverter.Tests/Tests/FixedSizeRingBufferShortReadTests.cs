// 补充覆盖：FixedSizeRingBuffer.ReadFromStream 短读场景（W5）
using System.IO;
using Sb.Extensions.System;
using Sb.Extensions.System.Buffers.RingBuffers;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   模拟每次 Read 只返回部分字节（或分多段返回）的流。
/// </summary>
internal class PartialReadStream(byte[] data, int maxChunkSize) : Stream
{
  private int _position;

  public override bool CanRead => true;
  public override bool CanSeek => false;
  public override bool CanWrite => false;
  public override long Length => data.Length;
  public override long Position { get => _position; set => throw new NotSupportedException(); }

  public override int Read(byte[] buffer, int offset, int count)
  {
    var remaining = data.Length - _position;
    var toRead = Math.Min(Math.Min(count, maxChunkSize), remaining);
    if (toRead <= 0)
    {
      return 0;
    }

    Array.Copy(data, _position, buffer, offset, toRead);
    _position += toRead;
    return toRead;
  }

  public override void Flush() { }
  public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
  public override void SetLength(long value) => throw new NotSupportedException();
  public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

public class FixedSizeRingBufferShortReadTests
{
  [Fact]
  public void ReadFromStream_PartialReads_SingleSegment_ReadsAll()
  {
    var data = new byte[] { 1, 2, 3, 4, 5 };
    using var stream = new PartialReadStream(data, maxChunkSize: 2); // 每次最多 2 字节
    var buffer = new FixedSizeRingBuffer<byte>(16);

    var read = buffer.ReadFromStream(stream);

    Assert.Equal(5, read);
    Assert.Equal(data, buffer.ToArray());
  }

  [Fact]
  public void ReadFromStream_PartialReads_AcrossWrap_ReadsAll()
  {
    var data = new byte[] { 10, 20, 30, 40, 50, 60 };
    using var stream = new PartialReadStream(data, maxChunkSize: 1); // 每次只回 1 字节
    var buffer = new FixedSizeRingBuffer<byte>(8);

    // 先制造环绕状态
    buffer.AddLastRange([0xEE, 0xEF, 0xF0, 0xF1, 0xF2, 0xF3]);
    buffer.RemoveFirst(6);

    var read = buffer.ReadFromStream(stream);

    Assert.Equal(data.Length, read);
    Assert.Equal(data, buffer.ToArray());
  }

  [Fact]
  public void ReadFromStream_StreamShorterThanRequest_ReturnsOnlyAvailable()
  {
    var data = new byte[] { 1, 2, 3 };
    using var stream = new PartialReadStream(data, maxChunkSize: 2);
    var buffer = new FixedSizeRingBuffer<byte>(16);

    var read = buffer.ReadFromStream(stream);

    Assert.Equal(3, read);
    Assert.Equal(data, buffer.ToArray());
  }

  [Fact]
  public void ReadFromStream_EmptyStream_ReturnsZero()
  {
    using var stream = new PartialReadStream(Array.Empty<byte>(), maxChunkSize: 2);
    var buffer = new FixedSizeRingBuffer<byte>(16);

    Assert.Equal(0, buffer.ReadFromStream(stream));
    Assert.Equal(0, buffer.Count);
  }
}
