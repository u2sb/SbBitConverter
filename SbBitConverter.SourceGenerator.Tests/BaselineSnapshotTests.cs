using Microsoft.CodeAnalysis;
using SbBitConverter.SourceGenerator.Tests.Infrastructure;
using Xunit.Abstractions;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   基线快照测试：两个分支对共享模型全集的生成产物必须与基线逐字节一致，
///   且「源码 + 生成代码」必须零编译错误。
/// </summary>
public class BaselineSnapshotTests(ITestOutputHelper output)
{
  public static TheoryData<GeneratorTestHost.GeneratorBranch> Branches()
    => new() { { GeneratorTestHost.GeneratorBranch.Roslyn48 }, { GeneratorTestHost.GeneratorBranch.Roslyn43 } };

  /// <summary>「源码 + 生成代码」零编译错误（生成代码可用性的硬门禁）。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Models_With_Generated_Source_Compiles_Without_Errors(GeneratorTestHost.GeneratorBranch branch)
  {
    var errors = GeneratorTestHost.CompileWithGenerated(branch, GeneratorTestHost.ModelsSource).Errors();

    output.WriteLine($"[{branch}] 编译错误数：{errors.Length}");
    foreach (var e in errors.Take(30))
    {
      output.WriteLine($"  {e.Id} {e.GetMessage()} @ {e.Location.GetLineSpan().StartLinePosition}");
    }

    Assert.Empty(errors);
  }

  /// <summary>生成器自身不得报 Error 级诊断。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Models_Generated_Source_Has_No_Generator_Errors(GeneratorTestHost.GeneratorBranch branch)
  {
    var diagnostics = GeneratorTestHost.RunOnce(branch, GeneratorTestHost.ModelsSource).GeneratorDiagnostics();
    Assert.DoesNotContain(diagnostics, static d => d.Severity == DiagnosticSeverity.Error);
  }

  /// <summary>生成产物与基线逐字节一致。</summary>
  [Theory]
  [MemberData(nameof(Branches))]
  public void Generated_Sources_Match_Baselines(GeneratorTestHost.GeneratorBranch branch)
  {
    var generated = GeneratorTestHost.RunOnce(branch, GeneratorTestHost.ModelsSource).GeneratedSources();
    var baselines = GeneratorTestHost.ReadBaselines();

    Assert.NotEmpty(baselines);
    Assert.Equal(baselines.Keys, generated.Keys);

    foreach (var (name, expected) in baselines)
    {
      Assert.True(generated.TryGetValue(name, out var actual), $"[{branch}] 缺少生成文件：{name}");
      Assert.True(expected == actual,
        $"[{branch}] 产物与基线不一致：{name}");
    }
  }
}
