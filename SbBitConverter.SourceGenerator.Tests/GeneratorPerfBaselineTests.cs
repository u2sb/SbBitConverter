using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using SbBitConverter.SourceGenerator.Tests.Infrastructure;
using Xunit.Abstractions;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   生成器构建期性能基线：度量并输出（耗时随机器波动，不做硬断言），
///   供 P3 发射层优化前后对比。
/// </summary>
public class GeneratorPerfBaselineTests(ITestOutputHelper output)
{
  private const int Iterations = 20;

  private const string UnrelatedFile = "namespace Unrelated.Perf { internal sealed class X { } }";

  private static CSharpCompilation FirstCompilation()
    => GeneratorTestHost.CreateCompilation(GeneratorTestHost.ModelsSource, "PerfAsm1");

  private static CSharpCompilation SecondCompilation(CSharpCompilation first)
    => first.AddSyntaxTrees(CSharpSyntaxTree.ParseText(UnrelatedFile, new CSharpParseOptions(LanguageVersion.Preview)));

  /// <summary>冷启动：全新驱动 + 全新编译，必然全量执行。</summary>
  [Fact]
  public void Models_Generator_Cold_Run_Duration()
  {
    GeneratorTestHost.RunOnce(GeneratorTestHost.GeneratorBranch.Roslyn48, GeneratorTestHost.ModelsSource); // 预热

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < Iterations; i++)
    {
      GeneratorTestHost.RunOnce(GeneratorTestHost.GeneratorBranch.Roslyn48, GeneratorTestHost.ModelsSource);
    }

    sw.Stop();
    var perRun = sw.Elapsed.TotalMilliseconds / Iterations;
    output.WriteLine($"冷启动平均耗时：{perRun:F2} ms/次（N={Iterations}）");
    Assert.True(perRun > 0);
  }

  /// <summary>增量运行：跑一次后仅追加无关文件再跑，应命中缓存。</summary>
  [Fact]
  public void Models_Generator_Incremental_Run_Duration()
  {
    var first = FirstCompilation();
    var second = SecondCompilation(first);

    // 预热
    var warm = GeneratorTestHost.CreateDriver(GeneratorTestHost.GeneratorBranch.Roslyn48, trackSteps: true);
    warm = warm.RunGenerators(first);
    warm = warm.RunGenerators(second);

    var coldTotal = TimeSpan.Zero;
    var incrementalTotal = TimeSpan.Zero;
    for (var i = 0; i < Iterations; i++)
    {
      var driver = GeneratorTestHost.CreateDriver(GeneratorTestHost.GeneratorBranch.Roslyn48, trackSteps: true);

      var coldWatch = Stopwatch.StartNew();
      driver = driver.RunGenerators(first);
      coldWatch.Stop();
      coldTotal += coldWatch.Elapsed;

      var incrementalWatch = Stopwatch.StartNew();
      driver = driver.RunGenerators(second);
      incrementalWatch.Stop();
      incrementalTotal += incrementalWatch.Elapsed;
    }

    var cold = coldTotal.TotalMilliseconds / Iterations;
    var incremental = incrementalTotal.TotalMilliseconds / Iterations;
    var speedup = incremental > 0 ? cold / incremental : 0;

    output.WriteLine($"冷启动（首次）：{cold:F2} ms/次");
    output.WriteLine($"增量（第二次，仅加一个无关文件）：{incremental:F2} ms/次");
    output.WriteLine($"增量相对冷启动加速比：{speedup:F2}×（N={Iterations}）");

    Assert.True(incremental > 0);
  }
}
