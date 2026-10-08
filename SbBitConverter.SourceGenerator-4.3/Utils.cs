using System;
using Microsoft.CodeAnalysis;

namespace SbBitConverter.SourceGenerator;

internal static class Utils
{
  /// <summary>
  ///   把类型尺寸解码为不含指针宽度的可比较编码（第一阶段用）：
  ///   枚举 → 底层类型的 SpecialType 值；struct → SpecialType 值；其余 → <see cref="UndeterminableSizeCode" />。
  /// </summary>
  public static int GetSizeCode(ITypeSymbol typeSymbol)
  {
    if (typeSymbol.TypeKind == TypeKind.Enum)
    {
      return typeSymbol is INamedTypeSymbol { EnumUnderlyingType: ITypeSymbol underlyingType }
        ? (int)underlyingType.SpecialType
        : UndeterminableSizeCode;
    }

    if (typeSymbol.TypeKind == TypeKind.Struct) return (int)typeSymbol.SpecialType;

    return UndeterminableSizeCode;
  }

  /// <summary>尺寸无法确定的编码。</summary>
  public const int UndeterminableSizeCode = -1;

  /// <summary>
  ///   把 <see cref="GetSizeCode" /> 的编码还原为字节尺寸（第二阶段用，需要指针宽度）。
  ///   与旧 SizeOfType 语义一致：未知 struct → 0（嵌套类型路径）。
  /// </summary>
  public static int SizeFromCode(int code, int pointerSize)
  {
    if (code == UndeterminableSizeCode) return 0;

    return (SpecialType)code switch
    {
      // 1字节类型
      SpecialType.System_Byte or SpecialType.System_SByte or SpecialType.System_Boolean => 1,

      // 2字节类型
      SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Char => 2,

      // 4字节类型
      SpecialType.System_Int32 or SpecialType.System_UInt32 or SpecialType.System_Single => 4,

      // 8字节类型
      SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Double => 8,

      // 平台相关类型
      SpecialType.System_IntPtr or SpecialType.System_UIntPtr => pointerSize,

      // 16字节类型
      SpecialType.System_Decimal => 16,

      // 未知 struct → 0（嵌套类型路径）
      _ => 0
    };
  }

  /// <summary>
  ///   判断是否为已知大小的内置 unmanaged 类型，作为 IsBaseType 的单一来源。
  /// </summary>
  public static bool IsKnownUnmanagedType(ITypeSymbol typeSymbol)
  {
    if (typeSymbol.TypeKind == TypeKind.Enum) return true;

    return typeSymbol.SpecialType switch
    {
      SpecialType.System_Byte or SpecialType.System_SByte or SpecialType.System_Boolean
      or SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Char
      or SpecialType.System_Int32 or SpecialType.System_UInt32 or SpecialType.System_Single
      or SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Double
      or SpecialType.System_IntPtr or SpecialType.System_UIntPtr => true,
      _ => false
    };
  }

  /// <summary>
  ///   获取平台指针类型尺寸（字节）。只依赖 <see cref="Platform" />，可投影为 int 参与增量缓存。
  /// </summary>
  public static int GetPointerSize(Platform platform)
  {
    return platform switch
    {
      Platform.X86 => 4,
      Platform.Arm => 4,
      Platform.X64 => 8,
      Platform.Arm64 => 8,
      Platform.AnyCpu32BitPreferred => 4,
      _ => 8 // AnyCpu 及未知平台按 64 位处理
    };
  }
}
