using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using static SbBitConverter.SourceGenerator.ConstTable;
using static SbBitConverter.SourceGenerator.Utils;

namespace SbBitConverter.SourceGenerator;

public static class SbBitConverterArrayGenerator
{
  public static void Gen(
    SourceProductionContext context,
    INamedTypeSymbol structSymbol,
    bool isUnsafe,
    LanguageVersion languageVersion,
    Compilation compilation)
  {
    var sbBitConverterArrayInfo = GetSbBitConverterInfo(structSymbol, compilation);
    if (sbBitConverterArrayInfo is null) return;

    var source = GenerateCodeForStruct(structSymbol, sbBitConverterArrayInfo, isUnsafe, languageVersion, compilation);
    var isGlobalNamespace = structSymbol.ContainingNamespace.IsGlobalNamespace;
    var namespaceName = isGlobalNamespace ? string.Empty : $"{structSymbol.ContainingNamespace.ToDisplayString()}_";
    context.AddSource($"{namespaceName}{structSymbol.Name}_SbBitConverterArray.g.cs",
      SourceText.From(source, Encoding.UTF8));
  }

  private static string GenerateCodeForStruct(
    INamedTypeSymbol structSymbol,
    SbBitConverterArrayInfo arrayInfo,
    bool isUnsafe,
    LanguageVersion languageVersion,
    Compilation compilation)
  {
    var structName = structSymbol.Name;
    var isGlobalNamespace = structSymbol.ContainingNamespace.IsGlobalNamespace;
    var namespaceName = structSymbol.ContainingNamespace.ToDisplayString();
    var elementTypeName = arrayInfo.ElementType.ToDisplayString();
    var elementSize = arrayInfo.ElementSize;
    var hasStructLayoutAttribute = arrayInfo.HasStructLayoutAttribute;
    var isReadonlyStruct = structSymbol.IsReadOnly;

    var readonlyPrefix = isReadonlyStruct ? "readonly " : string.Empty;

    var sb = new StringBuilder();
    sb.AppendLine("// Auto-generated code");
    sb.AppendLine("#pragma warning disable");
    sb.AppendLine("using System;");
    sb.AppendLine("using System.Runtime.CompilerServices;");
    sb.AppendLine("using System.Runtime.InteropServices;");
    sb.AppendLine("using Sb.Extensions.System;");
    sb.AppendLine("using static Sb.Extensions.System.SbBitConverter;");
    sb.AppendLine("using static Sb.Extensions.System.SpanExtension;");

    if (!isGlobalNamespace)
    {
      sb.AppendLine($"namespace {namespaceName}");
      sb.AppendLine("{");
    }

    // if (isUnsafe)
    // {
    //   sb.AppendLine($"unsafe partial struct {structName}");
    //   sb.AppendLine("{");
    //   sb.Append("  [FieldOffset(0)]");
    //   sb.AppendLine($"  public fixed byte source[{elementSize * arrayInfo.Length}];");
    //
    //   if (arrayInfo.ElementType.AllowUnsafeSource())
    //   {
    //     sb.Append("  [FieldOffset(0)]");
    //     sb.AppendLine($"  public fixed {elementTypeName} elementSource[{arrayInfo.Length}];");
    //   }
    //
    //   sb.AppendLine("}");
    // }

    // sb.AppendLine();

    if (!hasStructLayoutAttribute)
    {
      var pack = 1;
      if (elementSize % 128 == 0) pack = 128;
      else if (elementSize % 64 == 0) pack = 64;
      else if (elementSize % 32 == 0) pack = 32;
      else if (elementSize % 16 == 0) pack = 16;
      else if (elementSize % 8 == 0) pack = 8;
      else if (elementSize % 4 == 0) pack = 4;
      else if (elementSize % 2 == 0) pack = 2;

      sb.AppendLine(
        $"[StructLayout(LayoutKind.Explicit, Pack = {pack}, Size = {elementSize * arrayInfo.Length})]");
    }

    sb.AppendLine($"partial struct {structName}");
    sb.AppendLine("{");

    sb.AppendLine(
      $"  public {structName}(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){arrayInfo.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    for (var i = 0; i < arrayInfo.Length; i++)
    {
      var offset = elementSize * i;

      var s0 = arrayInfo.IsBaseType
        ? $"    this._item{i} = data.Slice({offset}, {elementSize}).ToT<{elementTypeName}>(mode);"
        : $"    this._item{i} = new {elementTypeName}(data.Slice({offset}, {elementSize}), mode);";

      sb.AppendLine(s0);
    }

    sb.AppendLine("  }");
    sb.AppendLine();

    for (var i = 0; i < arrayInfo.Length; i++)
    {
      sb.Append($"  [FieldOffset({i * arrayInfo.ElementSize})]");
      sb.AppendLine($"private {readonlyPrefix}{elementTypeName} _item{i};");
      sb.AppendLine();
    }


    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public byte[] ToByteArray({BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){arrayInfo.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    var data = new byte[Unsafe.SizeOf<{structName}>()];");
    sb.AppendLine("    var span = data.AsSpan();");
    sb.AppendLine("    WriteTo(span, mode);");
    sb.AppendLine("    return data;");
    sb.AppendLine("  }");
    sb.AppendLine();


    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public void WriteTo(Span<byte> span, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){arrayInfo.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(span, Unsafe.SizeOf<{structName}>());");
    for (var i = 0; i < arrayInfo.Length; i++)
    {
      var offset = elementSize * i;
      var s0 = $"    this._item{i}.WriteTo<{elementTypeName}>(span.Slice({offset}, {elementSize}), mode);";
      sb.AppendLine(s0);
    }

    sb.AppendLine("  }");
    sb.AppendLine();


    sb.AppendLine($"  public int Length => {arrayInfo.Length};");
    sb.AppendLine();
    sb.AppendLine($"  public ref {readonlyPrefix}{elementTypeName} this[int index]");
    sb.AppendLine("  {");
    sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine("    get");
    sb.AppendLine("    {");
    sb.AppendLine("      switch (index)");
    sb.AppendLine("      {");
    for (var i = 0; i < arrayInfo.Length; i++)
    {
      sb.AppendLine($"        case {i}:");
      sb.AppendLine(
        languageVersion >= LanguageVersion.CSharp12
          ? $"          return ref Unsafe.AsRef(in _item{i});"
          : $"          return ref AsSpan()[{i}];"
      );
    }

    sb.AppendLine("        default:");
    sb.AppendLine("          throw new IndexOutOfRangeException();");
    sb.AppendLine("      }");
    sb.AppendLine("    }");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine($"  public {(isReadonlyStruct ? "ReadOnly" : string.Empty)}Span<{elementTypeName}> AsSpan()");
    sb.AppendLine("  {");

    sb.AppendLine(
      $"    return Create{(isReadonlyStruct ? "ReadOnly" : string.Empty)}Span({(isReadonlyStruct ? "in" : "ref")} _item0, {arrayInfo.Length});");

    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public {(isReadonlyStruct ? "ReadOnly" : string.Empty)}Span<{elementTypeName}> Slice(int start, int length)");
    sb.AppendLine("  {");
    sb.AppendLine("    var span = AsSpan();");
    sb.AppendLine("    return span.Slice(start, length);");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("}");
    if (!isGlobalNamespace) sb.AppendLine("}");
    sb.AppendLine("#pragma warning restore");
    return sb.ToString();
  }

  private static SbBitConverterArrayInfo? GetSbBitConverterInfo(INamedTypeSymbol structSymbol, Compilation compilation)
  {
    var sbBitConverterAttr = structSymbol.GetAttribute(SbBitConverterArrayAttributeName);

    if (sbBitConverterAttr?.ConstructorArguments[0].Value is INamedTypeSymbol type &&
        sbBitConverterAttr.ConstructorArguments[1].Value is int length
        && sbBitConverterAttr.ConstructorArguments[2].Value is byte mode)
    {
      var size = sbBitConverterAttr.GetAttributeNamedArguments<int?>("ElementSize") ?? 0;
      if (size <= 0) size = SizeOfType(type, compilation);

      var structLayoutAttr = structSymbol.ContainsAttribute(StructLayoutAttributeName);

      return new SbBitConverterArrayInfo(type, size, length, mode, structLayoutAttr);
    }

    return null;
  }
}

internal class SbBitConverterArrayInfo(
  INamedTypeSymbol elementType,
  int elementSize,
  int length,
  byte mode,
  bool hasStructLayoutAttribute)
{
  public INamedTypeSymbol ElementType { get; } = elementType;

  public int ElementSize { get; } = elementSize;

  public int Length { get; } = length;

  public byte Mode { get; } = mode;

  public bool HasStructLayoutAttribute { get; } = hasStructLayoutAttribute;

  public bool IsBaseType => ElementType.SpecialType is SpecialType.System_Byte or SpecialType.System_SByte
    or SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Int32 or SpecialType.System_UInt32
    or SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Single or SpecialType.System_Double
    or SpecialType.System_Char or SpecialType.System_IntPtr or SpecialType.System_UIntPtr;
}