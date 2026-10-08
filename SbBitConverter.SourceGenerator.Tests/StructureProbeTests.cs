using Microsoft.CodeAnalysis;
using SbBitConverter.SourceGenerator.Tests.Infrastructure;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   生成器结构探针：命名空间形态、多特性组合、HintName 约定等端到端行为。
/// </summary>
public class StructureProbeTests
{
  public static TheoryData<GeneratorTestHost.GeneratorBranch> Branches()
    => new() { { GeneratorTestHost.GeneratorBranch.Roslyn48 }, { GeneratorTestHost.GeneratorBranch.Roslyn43 } };

  private const string Header = "using System.Runtime.InteropServices;\nusing Sb.Extensions.System;\nusing SbBitConverter.Attributes;\n";

  /// <summary>全局命名空间下的 struct：产物不含 namespace 声明，HintName 无前缀。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Global_Namespace_Struct_Generates_Without_Namespace(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      [SbBitConverterArray(typeof(int), 4)]
      public partial struct GlobalArr { }
      """;

      var sources = GeneratorTestHost.RunOnce(branch, Header + body).GeneratedSources();

      var pair = Assert.Single(sources);
      Assert.Equal("GlobalArr_SbBitConverterArray.g.cs", pair.Key);
      Assert.DoesNotContain("namespace ", pair.Value, StringComparison.Ordinal);
  }

  /// <summary>嵌套命名空间：HintName 使用完整命名空间前缀。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Nested_Namespace_Struct_Gets_Full_HintName(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace A.B.C
      {
        [SbBitConverterArray(typeof(int), 4)]
        public partial struct Deep { }
      }
      """;

      var sources = GeneratorTestHost.RunOnce(branch, Header + body).GeneratedSources();

      var pair = Assert.Single(sources);
      Assert.Equal("A.B.C_Deep_SbBitConverterArray.g.cs", pair.Key);
      Assert.Contains("namespace A.B.C", pair.Value, StringComparison.Ordinal);
  }

  /// <summary>同一 struct 标注两个特性：SBBIT0007（Error），不产出任何代码（双特性成员重名缺陷的防护）。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Struct_With_Both_Attributes_Reports_SBBIT0007_And_Generates_Nothing(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 4)]
        [SbBitConverterStruct]
        [StructLayout(LayoutKind.Explicit)]
        public partial struct Combo
        {
          [FieldOffset(0)] public int Value;
        }
      }
      """;

      var driver = GeneratorTestHost.RunOnce(branch, Header + body);
      var diagnostics = driver.GeneratorDiagnostics();

      Assert.Contains(diagnostics, static d => d.Id == "SBBIT0007" && d.Severity == DiagnosticSeverity.Error);

      var sources = driver.GeneratedSources();
      Assert.Empty(sources);
  }

  /// <summary>readonly struct + 嵌套用户 struct 元素（用户自定义 ElementSize）：产物可编译。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Readonly_Container_With_Nested_Elements_Compiles(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterStruct]
        [StructLayout(LayoutKind.Explicit)]
        public partial struct Inner
        {
          [FieldOffset(0)] public int Value;
        }

        [SbBitConverterArray(typeof(Inner), 3, Sb.Extensions.System.BigAndSmallEndianEncodingMode.ABCD, ElementSize = 16)]
        public readonly partial struct InnerArray3 { }
      }
      """;

      var errors = GeneratorTestHost.CompileWithGenerated(branch, Header + body).Errors();
      Assert.Empty(errors);
  }
}
