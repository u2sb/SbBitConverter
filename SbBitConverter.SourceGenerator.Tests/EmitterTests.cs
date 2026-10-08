using System.Collections.Immutable;
using SbBitConverter.SourceGenerator.Emitting;
using SbBitConverter.SourceGenerator.Modeling;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   发射器（Emitter）单元测试：模型 → 代码文本的关键形状（利用 InternalsVisibleTo 直接调用 internal 层）。
/// </summary>
public class EmitterTests
{
  private static ArrayModel MakeArrayModel(
    int length = 4, int elementSize = 4, bool isReadonly = false, byte mode = 0, bool hasLayout = false)
    => new(
      HintName: $"Probe_Arr{length}_SbBitConverterArray.g.cs",
      StructName: $"Arr{length}",
      NamespaceName: "Probe",
      IsGlobalNamespace: false,
      ElementTypeName: "int",
      ElementSize: elementSize,
      Length: length,
      Mode: mode,
      HasStructLayoutAttribute: hasLayout,
      IsBaseType: true,
      IsEnumType: false,
      IsReadonlyStruct: isReadonly);

  private static StructModel MakeStructModel(params (string Name, string Type, int Offset, int Size)[] fields)
    => new(
      HintName: "Probe_S_SbBitConverterStruct.g.cs",
      StructName: "S",
      NamespaceName: "Probe",
      IsGlobalNamespace: false,
      Mode: 0,
      Fields: new EquatableArray<StructFieldModel>(
        fields.Select(static f => new StructFieldModel(f.Name, f.Type, f.Offset, f.Size))
          .ToImmutableArray()));

  [Fact]
  public void ArrayEmitter_Contains_StructLayout_When_Missing()
  {
    var code = ArrayEmitter.Emit(MakeArrayModel(length: 4, elementSize: 4, hasLayout: false));
    Assert.Contains("[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]", code, StringComparison.Ordinal);
  }

  [Fact]
  public void ArrayEmitter_Omits_StructLayout_When_Present()
  {
    var code = ArrayEmitter.Emit(MakeArrayModel(hasLayout: true));
    Assert.DoesNotContain("[StructLayout", code, StringComparison.Ordinal);
  }

  [Fact]
  public void ArrayEmitter_FieldCount_Matches_Length()
  {
    var code = ArrayEmitter.Emit(MakeArrayModel(length: 8, elementSize: 1));
    for (var i = 0; i < 8; i++)
    {
      Assert.Contains($"_item{i}", code, StringComparison.Ordinal);
    }

    Assert.DoesNotContain("_item8", code, StringComparison.Ordinal);
  }

  [Fact]
  public void ArrayEmitter_Readonly_Struct_Uses_ReadOnlySpan()
  {
    var code = ArrayEmitter.Emit(MakeArrayModel(isReadonly: true));
    // 与历史实现一致：struct 声明不加 readonly，readonly 体现在字段与 ref 返回上
    Assert.Contains("partial struct Arr4", code, StringComparison.Ordinal);
    Assert.Contains("private readonly int _item0;", code, StringComparison.Ordinal);
    Assert.Contains("ReadOnlySpan<int> AsSpan()", code, StringComparison.Ordinal);
    Assert.DoesNotContain("public void ReadFromBytes", code, StringComparison.Ordinal);
  }

  [Fact]
  public void ArrayEmitter_Global_Namespace_Has_No_Namespace_Declaration()
  {
    var model = MakeArrayModel() with { IsGlobalNamespace = true, NamespaceName = string.Empty };
    var code = ArrayEmitter.Emit(model);
    Assert.DoesNotContain("namespace ", code, StringComparison.Ordinal);
  }

  [Fact]
  public void StructEmitter_Nested_Field_Uses_Default_Init()
  {
    var model = MakeStructModel(("Inner", "Probe.Inner", 0, 0), ("Value", "int", 12, 4));
    var code = StructEmitter.Emit(model);

    Assert.Contains("this.Inner = default; this.Inner.ReadFromBytes(data.Slice(0, Unsafe.SizeOf<Probe.Inner>()), mode);",
      code, StringComparison.Ordinal);
    Assert.Contains("this.Value = data.Slice(12, 4).ToT<int>(mode);", code, StringComparison.Ordinal);
    Assert.Contains("this.Inner.WriteTo(span.Slice(0, Unsafe.SizeOf<Probe.Inner>()), mode);", code, StringComparison.Ordinal);
  }

  [Fact]
  public void StructEmitter_Unsupported_Field_Size_Produces_Blank_Line()
  {
    // 尺寸未知的字段（如引用类型）旧行为：生成空行（静默跳过），由诊断 SBBIT0006 告知
    var model = MakeStructModel(("Name", "string", 0, -1));
    var code = StructEmitter.Emit(model);

    Assert.Contains("partial struct S", code, StringComparison.Ordinal);
    Assert.DoesNotContain("this.Name", code, StringComparison.Ordinal);
  }

  [Fact]
  public void Both_Emitters_End_With_Pragma_Restore()
  {
    Assert.EndsWith("#pragma warning restore" + Environment.NewLine, ArrayEmitter.Emit(MakeArrayModel()), StringComparison.Ordinal);
    Assert.EndsWith("#pragma warning restore" + Environment.NewLine, StructEmitter.Emit(MakeStructModel(("V", "int", 0, 4))), StringComparison.Ordinal);
  }
}
