using System.Buffers;

#pragma warning disable IDE0130
namespace Sb.Extensions.System.Buffers;

/// <summary>
///   <see cref="ArrayPool{T}" /> 的包装类，提供带刷零选项的租用和归还操作。
/// </summary>
public static class BestArrayPool
{
  private static readonly ArrayPool<byte> Pool = ArrayPool<byte>.Create();

  /// <summary>
  ///   从池中租用指定最小长度的字节数组。若长度小于等于 0，返回空数组。
  /// </summary>
  /// <param name="minimumLength">最小数组长度</param>
  /// <returns>租用的字节数组</returns>
  public static byte[] Rent(int minimumLength) => minimumLength <= 0 ? [] : Pool.Rent(minimumLength);

  /// <summary>
  ///   将字节数组归还到池中。若数组为 <see langword="null" /> 或长度为 0，不执行任何操作。
  /// </summary>
  /// <param name="array">要归还的字节数组</param>
  public static void Return(byte[]? array)
  {
    if (array is not null && array.Length > 0)
    {
      Pool.Return(array);
    }
  }
}
