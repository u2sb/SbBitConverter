using Microsoft.CodeAnalysis;
using SbBitConverter.SourceGenerator.Tests.Infrastructure;
using Xunit.Abstractions;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   诊断测试：SBBIT0001~0006 的触发与不误报，双分支一致。
/// </summary>
public class DiagnosticTests(ITestOutputHelper output)
{
  private const string Header =
    "using System.Runtime.InteropServices;\nusing Sb.Extensions.System;\nusing SbBitConverter.Attributes;\n";

  public static TheoryData<GeneratorTestHost.GeneratorBranch> Branches()
    => new() { { GeneratorTestHost.GeneratorBranch.Roslyn48 }, { GeneratorTestHost.GeneratorBranch.Roslyn43 } };

  private static Diagnostic[] DiagnosticsOf(GeneratorTestHost.GeneratorBranch branch, string body)
    => GeneratorTestHost.RunOnce(branch, Header + body).GeneratorDiagnostics().ToArray();

  private static string[] IdsOf(GeneratorTestHost.GeneratorBranch branch, string body)
    => DiagnosticsOf(branch, body).Select(static d => d.Id).Distinct().OrderBy(static id => id, StringComparer.Ordinal).ToArray();

  // ══════════════════ SBBIT0001：特性参数非法 ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Array_With_Zero_Length_Reports_SBBIT0002(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 0)]
        public partial struct Bad { }
      }
      """;

    Assert.Contains("SBBIT0002", IdsOf(branch, body));
  }

  // ══════════════════ SBBIT0003：非 partial ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Non_Partial_Struct_Reports_SBBIT0003(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 4)]
        public struct NotPartial { }
      }
      """;

    Assert.Contains("SBBIT0003", IdsOf(branch, body));
  }

  // ══════════════════ SBBIT0004：嵌套 / 泛型 ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Nested_Struct_Reports_SBBIT0004(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        public static class Wrapper
        {
          [SbBitConverterArray(typeof(int), 4)]
          public partial struct Nested { }
        }
      }
      """;

    Assert.Contains("SBBIT0004", IdsOf(branch, body));
  }

  [Theory]
  [MemberData(nameof(Branches))]
  public void Generic_Struct_Reports_SBBIT0004(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 4)]
        public partial struct Generic<T> { }
      }
      """;

    Assert.Contains("SBBIT0004", IdsOf(branch, body));
  }

  // ══════════════════ SBBIT0005：无 FieldOffset 字段 ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Struct_Without_FieldOffset_Reports_SBBIT0005(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterStruct]
        [StructLayout(LayoutKind.Explicit)]
        public partial struct Empty { }
      }
      """;

    Assert.Contains("SBBIT0005", IdsOf(branch, body));
  }

  // ══════════════════ SBBIT0006：字段类型尺寸无法确定 ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Struct_With_Unsupported_Field_Reports_SBBIT0006(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterStruct]
        [StructLayout(LayoutKind.Explicit)]
        public partial struct HasString
        {
          [FieldOffset(0)] public string Name;
        }
      }
      """;

    Assert.Contains("SBBIT0006", IdsOf(branch, body));
  }

  // ══════════════════ 不误报 ══════════════════

  [Theory]
  [MemberData(nameof(Branches))]
  public void Valid_Array_Produces_No_Diagnostics(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 4)]
        public partial struct Fine { }
      }
      """;

    Assert.Empty(DiagnosticsOf(branch, body));
  }

  [Theory]
  [MemberData(nameof(Branches))]
  public void Valid_Struct_Produces_No_Diagnostics(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterStruct]
        [StructLayout(LayoutKind.Explicit)]
        public partial struct Fine
        {
          [FieldOffset(0)] public int Value;
        }
      }
      """;

    Assert.Empty(DiagnosticsOf(branch, body));
  }

  /// <summary>诊断失败用例不生成任何代码（失败时无产物可用）。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Invalid_Cases_Produce_No_Generated_Sources(GeneratorTestHost.GeneratorBranch branch)
  {
    var body = """
      namespace Probe
      {
        [SbBitConverterArray(typeof(int), 0)]
        public partial struct Bad { }

        [SbBitConverterArray(typeof(int), 4)]
        public struct NotPartial { }
      }
      """;

    var sources = GeneratorTestHost.RunOnce(branch, Header + body).GeneratedSources();
    output.WriteLine($"[{branch}] 生成文件数：{sources.Count}");
    Assert.Empty(sources);
  }
}
