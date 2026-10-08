using System.Text;
using SbBitConverter.SourceGenerator.Modeling;
using static SbBitConverter.SourceGenerator.ConstTable;

namespace SbBitConverter.SourceGenerator.Emitting;

/// <summary>从 <see cref="StructModel" /> 生成 [SbBitConverterStruct] 的配套代码。</summary>
internal static class StructEmitter
{
  /// <summary>生成代码文本（与历史实现输出一致）。</summary>
  public static string Emit(StructModel model)
  {
    var structName = model.StructName;

    // 容量预估：固定开销 + 每字段三条语句的保守估计，避免反复扩容拷贝
    var sb = StringBuilderPool.Rent(512 + model.Fields.Count * 256);
    var toTStringBuilder = StringBuilderPool.Rent(model.Fields.Count * 128);
    var toBytesStringBuilder = StringBuilderPool.Rent(model.Fields.Count * 128);
    var readFromBytesStringBuilder = StringBuilderPool.Rent(model.Fields.Count * 128);

    foreach (var field in model.Fields)
    {
      toTStringBuilder.AppendLine(ToTLine(field));
      toBytesStringBuilder.AppendLine(WriteToLine(field));
      readFromBytesStringBuilder.AppendLine(ReadFromBytesLine(field));
    }

    sb.AppendLine("// Auto-generated code");
    sb.AppendLine("#pragma warning disable");
    sb.AppendLine("using System;");
    sb.AppendLine("using System.Runtime.CompilerServices;");
    sb.AppendLine("using Sb.Extensions.System;");
    sb.AppendLine("using static Sb.Extensions.System.SbBitConverter;");
    sb.AppendLine("using static Sb.Extensions.System.SpanExtension;");

    if (!model.IsGlobalNamespace)
    {
      sb.AppendLine($"namespace {model.NamespaceName}");
      sb.AppendLine("{");
    }

    sb.AppendLine($"partial struct {structName}");
    sb.AppendLine("{");

    sb.AppendLine(
      $"  public {structName}(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    sb.Append(toTStringBuilder);
    sb.AppendLine();
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine(
      $"  public byte[] ToByteArray({BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    var data = new byte[Unsafe.SizeOf<{structName}>()];");
    sb.AppendLine("    WriteTo(data, mode);");
    sb.AppendLine("    return data;");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public void WriteTo(Span<byte> span, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(span, Unsafe.SizeOf<{structName}>());");
    sb.Append(toBytesStringBuilder);
    sb.AppendLine();
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public void ReadFromBytes(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    sb.Append(readFromBytesStringBuilder);
    sb.AppendLine();
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("}");
    if (!model.IsGlobalNamespace) sb.AppendLine("}");
    sb.AppendLine("#pragma warning restore");

    StringBuilderPool.Return(toTStringBuilder);
    StringBuilderPool.Return(toBytesStringBuilder);
    StringBuilderPool.Return(readFromBytesStringBuilder);

    return StringBuilderPool.Return(sb);
  }

  /// <summary>构造函数中的字段读取语句。</summary>
  private static string ToTLine(StructFieldModel field) => field.Size switch
  {
    0 =>
      $"    this.{field.Name} = default; this.{field.Name}.ReadFromBytes(data.Slice({field.Offset}, Unsafe.SizeOf<{field.TypeName}>()), mode);",
    1 or 2 or 4 or 8 =>
      $"    this.{field.Name} = data.Slice({field.Offset}, {field.Size}).ToT<{field.TypeName}>(mode);",
    _ => string.Empty
  };

  /// <summary>WriteTo 中的字段写入语句。</summary>
  private static string WriteToLine(StructFieldModel field) => field.Size switch
  {
    0 =>
      $"    this.{field.Name}.WriteTo(span.Slice({field.Offset}, Unsafe.SizeOf<{field.TypeName}>()), mode);",
    1 or 2 or 4 or 8 =>
      $"    this.{field.Name}.WriteTo<{field.TypeName}>(span.Slice({field.Offset}, {field.Size}), mode);",
    _ => string.Empty
  };

  /// <summary>ReadFromBytes 中的字段读取语句。</summary>
  private static string ReadFromBytesLine(StructFieldModel field) => field.Size switch
  {
    0 =>
      $"    this.{field.Name}.ReadFromBytes(data.Slice({field.Offset}, Unsafe.SizeOf<{field.TypeName}>()), mode);",
    1 or 2 or 4 or 8 =>
      $"    this.{field.Name} = data.Slice({field.Offset}, {field.Size}).ToT<{field.TypeName}>(mode);",
    _ => string.Empty
  };
}
