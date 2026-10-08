using System.Collections.Immutable;
using System.Diagnostics;
using SbBitConverter.SourceGenerator.Emitting;
using SbBitConverter.SourceGenerator.Modeling;
using Xunit.Abstractions;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   发射器原始性能：绕过 Roslyn 管线，直接循环调用 Emit，度量纯文本生成开销。
/// </summary>
public class EmitterPerfTests(ITestOutputHelper output)
{
  private const int Iterations = 2000;

  private static ArrayModel MakeArrayModel(int length, int elementSize) => new(
    HintName: $"Probe_Arr{length}_SbBitConverterArray.g.cs",
    StructName: $"Arr{length}",
    NamespaceName: "Probe",
    IsGlobalNamespace: false,
    ElementTypeName: "int",
    ElementSize: elementSize,
    Length: length,
    Mode: 0,
    HasStructLayoutAttribute: false,
    IsBaseType: true,
    IsEnumType: false,
    IsReadonlyStruct: false);

  private static readonly ArrayModel SmallArray = MakeArrayModel(4, 4);
  private static readonly ArrayModel LargeArray = MakeArrayModel(256, 4);

  private static readonly StructModel Struct = new(
    HintName: "Probe_S_SbBitConverterStruct.g.cs",
    StructName: "S",
    NamespaceName: "Probe",
    IsGlobalNamespace: false,
    Mode: 0,
    Fields: new EquatableArray<StructFieldModel>(new[]
    {
      new StructFieldModel("A", "byte", 0, 1),
      new StructFieldModel("B", "short", 2, 2),
      new StructFieldModel("C", "int", 4, 4),
      new StructFieldModel("D", "long", 8, 8),
      new StructFieldModel("Inner", "Probe.Inner", 16, 0),
    }.ToImmutableArray()));

  [Fact]
  public void Emitter_Raw_Emit_Duration()
  {
    // 预热
    for (var i = 0; i < 100; i++)
    {
      ArrayEmitter.Emit(SmallArray);
      ArrayEmitter.Emit(LargeArray);
      StructEmitter.Emit(Struct);
    }

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < Iterations; i++)
    {
      _ = ArrayEmitter.Emit(SmallArray);
      _ = ArrayEmitter.Emit(LargeArray);
      _ = StructEmitter.Emit(Struct);
    }

    sw.Stop();
    var perRun = sw.Elapsed.TotalMilliseconds / Iterations;
    output.WriteLine($"原始 Emit 平均耗时：{perRun:F3} ms/组（小数组 + 256 元素大数组 + 5 字段 struct，N={Iterations}）");

    // 宽松上限：纯文本生成不应超过 1ms/组（防发射层意外退化）
    Assert.True(perRun < 1.0, $"发射器耗时异常：{perRun:F3} ms/组");
  }
}
