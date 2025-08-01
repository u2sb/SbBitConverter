using System;
using Microsoft.CodeAnalysis;

namespace SbBitConverter.SourceGenerator;

internal static class Utils
{
  public static int SizeOfType(ITypeSymbol typeSymbol, Compilation compilation)
  {
    var size = typeSymbol.TypeKind switch
    {
      // 枚举型
      TypeKind.Enum => GetEnumSize(typeSymbol),

      // Struct
      // 处理所有内置Unmanaged结构体类型
      TypeKind.Struct => typeSymbol.SpecialType switch
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
        SpecialType.System_IntPtr or SpecialType.System_UIntPtr => GetPointerSize(compilation),

        // 16字节类型
        SpecialType.System_Decimal => 16,

        // 其他struct返回0
        _ => 0
      },

      _ => int.MinValue
    };

    return size;
  }

  private static int GetEnumSize(ITypeSymbol typeSymbol)
  {
    if (typeSymbol is INamedTypeSymbol { EnumUnderlyingType: ITypeSymbol underlyingType })
      return underlyingType.SpecialType switch
      {
        SpecialType.System_Byte or SpecialType.System_SByte => 1,
        SpecialType.System_Int16 or SpecialType.System_UInt16 => 2,
        SpecialType.System_Int32 or SpecialType.System_UInt32 => 4,
        SpecialType.System_Int64 or SpecialType.System_UInt64 => 8,
        _ => 0
      };

    return 0;
  }

  /// <summary>
  ///   获取指针类型尺寸
  /// </summary>
  /// <param name="compilation"></param>
  /// <returns></returns>
  private static int GetPointerSize(Compilation compilation)
  {
    return compilation?.Options.Platform switch
    {
      Platform.X86 => 4,
      Platform.Arm => 4,
      Platform.X64 => 8,
      Platform.Arm64 => 8,
      Platform.AnyCpu32BitPreferred => 4,
      _ => IntPtr.Size // 回退到当前运行环境
    };
  }
}