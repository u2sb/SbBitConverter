using System.Text;
using SbBitConverter.SourceGenerator.Modeling;
using static SbBitConverter.SourceGenerator.ConstTable;

namespace SbBitConverter.SourceGenerator.Emitting;

/// <summary>从 <see cref="ArrayModel" /> 生成 [SbBitConverterArray] 的配套代码。</summary>
internal static class ArrayEmitter
{
  /// <summary>生成代码文本（与历史实现输出一致）。</summary>
  public static string Emit(ArrayModel model)
  {
    var structName = model.StructName;
    var elementTypeName = model.ElementTypeName;
    var elementSize = model.ElementSize;

    // 容量预估：头部/尾部固定开销 + 每个元素三条语句的保守估计，避免反复扩容拷贝
    var sb = StringBuilderPool.Rent(512 + model.Length * 256);
    sb.AppendLine("// Auto-generated code");
    sb.AppendLine("#pragma warning disable");
    sb.AppendLine("using System;");
    sb.AppendLine("using System.Runtime.CompilerServices;");
    sb.AppendLine("using System.Runtime.InteropServices;");
    sb.AppendLine("using Sb.Extensions.System;");
    sb.AppendLine("using static Sb.Extensions.System.SbBitConverter;");
    sb.AppendLine("using static Sb.Extensions.System.SpanExtension;");

    if (!model.IsGlobalNamespace)
    {
      sb.AppendLine($"namespace {model.NamespaceName}");
      sb.AppendLine("{");
    }

    if (!model.HasStructLayoutAttribute)
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
        $"[StructLayout(LayoutKind.Explicit, Pack = {pack}, Size = {elementSize * model.Length})]");
    }

    var readonlyPrefix = model.IsReadonlyStruct ? "readonly " : string.Empty;

    sb.AppendLine($"partial struct {structName}");
    sb.AppendLine("{");

    sb.AppendLine(
      $"  public {structName}(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
    sb.AppendLine("  {");
    sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
    for (var i = 0; i < model.Length; i++)
    {
      var offset = elementSize * i;

      string s0;
      if (IsSingleByteElementType(model))
        s0 = $"    this._item{i} = {SingleByteReadExpression(elementTypeName, offset)};";
      else if (model.IsBaseType)
        s0 = $"    this._item{i} = data.Slice({offset}, {elementSize}).ToT<{elementTypeName}>(mode);";
      else if (model.IsReadonlyStruct)
        s0 = $"    this._item{i} = new {elementTypeName}(data.Slice({offset}, {elementSize}), mode);";
      else
        s0 = $"    this._item{i} = default; this._item{i}.ReadFromBytes(data.Slice({offset}, {elementSize}), mode);";

      sb.AppendLine(s0);
    }

    sb.AppendLine("  }");
    sb.AppendLine();

    for (var i = 0; i < model.Length; i++)
    {
      sb.Append($"  [FieldOffset({i * model.ElementSize})]");
      sb.AppendLine($"private {readonlyPrefix}{elementTypeName} _item{i};");
      sb.AppendLine();
    }

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
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
    if (IsSingleByteElementType(model))
    {
      // P3.2 特化：1 字节元素与字节序无关，整块拷贝替代逐元素 WriteTo<T> 调用
      // （元素类型不是 byte 时按字节重解释）
      sb.AppendLine(elementTypeName == "byte"
        ? "    AsSpan().CopyTo(span);"
        : "    MemoryMarshal.AsBytes(AsSpan()).CopyTo(span);");
    }
    else
    {
      for (var i = 0; i < model.Length; i++)
      {
        var offset = elementSize * i;
        var s0 = $"    this._item{i}.WriteTo<{elementTypeName}>(span.Slice({offset}, {elementSize}), mode);";
        sb.AppendLine(s0);
      }
    }

    sb.AppendLine("  }");
    sb.AppendLine();

    if (!model.IsReadonlyStruct)
    {
      sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
      sb.AppendLine(
        $"  public void ReadFromBytes(ReadOnlySpan<byte> data, {BigAndSmallEndianEncodingModeEnum} mode = ({BigAndSmallEndianEncodingModeEnum}){model.Mode})");
      sb.AppendLine("  {");
      sb.AppendLine($"    CheckLength(data, Unsafe.SizeOf<{structName}>());");
      if (IsSingleByteElementType(model))
      {
        // P3.2 特化：直接下标读取，替代逐元素 ReadFromBytes 调用
        for (var i = 0; i < model.Length; i++)
        {
          sb.AppendLine($"    this._item{i} = {SingleByteReadExpression(elementTypeName, elementSize * i)};");
        }
      }
      else
      {
        for (var i = 0; i < model.Length; i++)
        {
          var offset = elementSize * i;
          var s0 = $"    this._item{i}.ReadFromBytes(data.Slice({offset}, {elementSize}), mode);";
          sb.AppendLine(s0);
        }
      }

      sb.AppendLine("  }");
      sb.AppendLine();
    }

    sb.AppendLine($"  public int Length => {model.Length};");
    sb.AppendLine($"  public int Count => {model.Length};");
    sb.AppendLine();
    sb.AppendLine($"  public ref {readonlyPrefix}{elementTypeName} this[int index]");
    sb.AppendLine("  {");
    sb.AppendLine("    [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine("    get");
    sb.AppendLine("    {");
    sb.AppendLine("      return ref AsSpan()[index];");
    sb.AppendLine("    }");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public {(model.IsReadonlyStruct ? "ReadOnly" : string.Empty)}Span<{elementTypeName}> AsSpan()");
    sb.AppendLine("  {");
    sb.AppendLine(
      $"    return Create{(model.IsReadonlyStruct ? "ReadOnly" : string.Empty)}Span({(model.IsReadonlyStruct ? "in" : "ref")} _item0, {model.Length});");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("  [MethodImpl(MethodImplOptions.AggressiveInlining)]");
    sb.AppendLine(
      $"  public {(model.IsReadonlyStruct ? "ReadOnly" : string.Empty)}Span<{elementTypeName}> Slice(int start, int length)");
    sb.AppendLine("  {");
    sb.AppendLine(
      "    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));");
    sb.AppendLine(model.IsReadonlyStruct
      ? "    return CreateReadOnlySpan(in this[start], length);"
      : "    return CreateSpan(ref this[start], length);");
    sb.AppendLine("  }");
    sb.AppendLine();

    sb.AppendLine("}");
    if (!model.IsGlobalNamespace) sb.AppendLine("}");
    sb.AppendLine("#pragma warning restore");
    return StringBuilderPool.Return(sb);
  }

  /// <summary>
  ///   P3.2：1 字节基元元素（byte/sbyte/bool）与字节序无关，可走整块拷贝/直接下标特化路径。
  /// </summary>
  private static bool IsSingleByteElementType(ArrayModel model)
    => model.IsBaseType && !model.IsEnumType && model.ElementSize == 1;

  /// <summary>
  ///   P3.2：生成从字节缓冲读取 1 字节元素的表达式（bool/sbyte 需要显式转换）。
  /// </summary>
  private static string SingleByteReadExpression(string elementTypeName, int offset)
    => elementTypeName switch
    {
      "bool" => $"data[{offset}] != 0",
      "sbyte" => $"unchecked((sbyte)data[{offset}])",
      _ => $"data[{offset}]",
    };
}
