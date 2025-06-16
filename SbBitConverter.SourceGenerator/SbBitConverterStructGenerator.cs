using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static SbBitConverter.SourceGenerator.ConstTable;
using static SbBitConverter.SourceGenerator.Utils;

namespace SbBitConverter.SourceGenerator;

public static class SbBitConverterStructGenerator
{
  public static void Gen(GeneratorExecutionContext context, INamedTypeSymbol structSymbol, bool isUnsafe)
  {
    var sbBitConverterAttr = structSymbol.GetAttribute(SbBitConverterStructAttributeName);

    if (sbBitConverterAttr == null) return;

    var encodingMode = GetEncodingMode(sbBitConverterAttr);
    var fieldInfos = CollectFieldData(structSymbol).ToList();

    if (fieldInfos.Count == 0) return;

    var source = GenerateCodeForStruct(context, structSymbol, encodingMode, fieldInfos, isUnsafe);
    context.AddSource($"{structSymbol.Name}_SbBitConverterStruct.g.cs", SourceText.From(source, Encoding.UTF8));
  }

  private static string GenerateCodeForStruct(GeneratorExecutionContext context, INamedTypeSymbol structSymbol,
    byte encodingMode,
    List<FieldInfo> fieldInfos, bool isUnsafe)
  {
    var structName = structSymbol.Name;
    var isGlobalNamespace = structSymbol.ContainingNamespace.IsGlobalNamespace;
    var namespaceName = structSymbol.ContainingNamespace.ToDisplayString();

    var toTStringBuilder = new StringBuilder();
    var toBytesStringBuilder = new StringBuilder();

    foreach (var fieldInfo in fieldInfos)
    {
      toTStringBuilder.AppendLine(BitConverterToTString(fieldInfo, context.Compilation));
      toBytesStringBuilder.AppendLine(BitConverterToBytesString(fieldInfo, context.Compilation));
    }


    var sb = new StringBuilder();
    sb.AppendLine("// Auto-generated code");
    sb.AppendLine("#pragma warning disable");
    sb.AppendLine("using SbBitConverter.Utils;");
    sb.AppendLine("using System;");
    sb.AppendLine("using System.Runtime.CompilerServices;");
    sb.AppendLine("using System.Runtime.InteropServices;");
    sb.AppendLine("using static SbBitConverter.Utils.Utils;");

    if (!isGlobalNamespace)
    {
      sb.AppendLine($"namespace {namespaceName}");
      sb.AppendLine("{");
    }

    sb.AppendLine($"partial struct {structName}");
    sb.AppendLine("{");

    sb.AppendLine(
      $"  public {structName}(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){encodingMode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    sb.AppendLine($"{toTStringBuilder}");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine(
      $"  public {structName}(ReadOnlySpan<ushort> data0, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){encodingMode})");
    sb.AppendLine("  {");
    sb.AppendLine("    var data = MemoryMarshal.AsBytes(data0);");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    sb.AppendLine($"{toTStringBuilder}");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine(
      $"  public byte[] ToByteArray({BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){encodingMode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    var data = new byte[Unsafe.SizeOf<{structName}>()];");
    sb.AppendLine("    var span = data.AsSpan();");
    sb.AppendLine("    WriteTo(span, mode);");
    sb.AppendLine("    return data;");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public void WriteTo(Span<byte> span, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){encodingMode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(span, Unsafe.SizeOf<{structName}>());");
    sb.AppendLine($"{toBytesStringBuilder}");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("}");
    if (!isGlobalNamespace) sb.AppendLine("}");
    sb.AppendLine("#pragma warning restore");

    return sb.ToString();
  }

  private static string BitConverterToTString(FieldInfo fieldInfo, Compilation compilation)
  {
    var size = SizeOfType(fieldInfo.Type, compilation);
    return size switch
    {
      0 =>
        $"    this.{fieldInfo.Name} = new {fieldInfo.Type.ToDisplayString()}(data.Slice({fieldInfo.Offset}, Unsafe.SizeOf<{fieldInfo.Type.ToDisplayString()}>()), mode);",
      1 or 2 or 4 or 8 =>
        $"    this.{fieldInfo.Name} = data[{fieldInfo.Offset}..{fieldInfo.Offset + size}].ToT<{fieldInfo.Type.ToDisplayString()}>(mode);",
      _ => string.Empty
    };
  }

  private static string BitConverterToBytesString(FieldInfo fieldInfo, Compilation compilation)
  {
    var size = SizeOfType(fieldInfo.Type, compilation);

    return size switch
    {
      0 =>
        $"    this.{fieldInfo.Name}.WriteTo(span.Slice({fieldInfo.Offset}, Unsafe.SizeOf<{fieldInfo.Type.ToDisplayString()}>()), mode);",
      1 or 2 or 4 or 8 =>
        $"    this.{fieldInfo.Name}.WriteTo<{fieldInfo.Type.ToDisplayString()}>(span[{fieldInfo.Offset}..{fieldInfo.Offset + size}], mode);",
      _ => string.Empty
    };
  }

  /// <summary>
  ///   获取编码方式
  /// </summary>
  /// <param name="attribute"></param>
  /// <returns></returns>
  private static byte GetEncodingMode(AttributeData attribute)
  {
    if (attribute.ConstructorArguments[0].Value is byte mode) return mode;
    return 0;
  }

  /// <summary>
  ///   获取信息
  /// </summary>
  /// <param name="structSymbol"></param>
  /// <returns></returns>
  private static IEnumerable<FieldInfo> CollectFieldData(INamedTypeSymbol structSymbol)
  {
    foreach (var member in structSymbol.GetMembers())
      switch (member)
      {
        case IFieldSymbol { IsImplicitlyDeclared: false } field:
        {
          if (GetFieldOffset(field) is { } offset)
            yield return new FieldInfo(
              field.Name,
              field.Type,
              offset,
              false);
          break;
        }
        case IPropertySymbol property:
        {
          var backingField = structSymbol
            .GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f =>
              f.IsImplicitlyDeclared &&
              f.AssociatedSymbol?.Equals(property, SymbolEqualityComparer.Default) == true);

          if (backingField != null && GetFieldOffset(backingField) is { } offset)
            yield return new FieldInfo(
              property.Name,
              property.Type,
              offset,
              true);
          break;
        }
      }
  }

  private static int? GetFieldOffset(IFieldSymbol field)
  {
    var attr1 = field.GetAttribute(FieldOffsetAttributeName);
    return attr1?.ConstructorArguments[0].Value as int?;
  }
}

internal class FieldInfo(string name, ITypeSymbol type, int offset, bool isPropertyBackingField)
{
  public string Name { get; } = name;

  public ITypeSymbol Type { get; } = type;

  public int Offset { get; } = offset;

  public bool IsPropertyBackingField { get; } = isPropertyBackingField;
}