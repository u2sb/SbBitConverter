using System;

namespace SbBitConverter.Attributes
{
  /// <summary>
  /// </summary>
  [AttributeUsage(AttributeTargets.Struct)]
  public class SbBitConverterArrayAttribute : Attribute
  {
    /// <summary>
    ///   SbBitConverterArray
    /// </summary>
    /// <param name="elementType">元素类型</param>
    /// <param name="length">长度</param>
    /// <param name="mode">编码方式</param>
    public SbBitConverterArrayAttribute(Type elementType,
      int length,
      BigAndSmallEndianEncodingMode mode = BigAndSmallEndianEncodingMode.DCBA)
    {
      ElementType = elementType;
      Length = length;
      Mode = mode;
    }

    /// <summary>
    ///   元素类型
    /// </summary>
    public Type ElementType { get; }

    /// <summary>
    ///   元素数量
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///   元素长度
    /// </summary>
    public int ElementSize { get; set; } = 0;

    /// <summary>
    ///   内存布局方式
    /// </summary>
    public BigAndSmallEndianEncodingMode Mode { get; }
  }
}