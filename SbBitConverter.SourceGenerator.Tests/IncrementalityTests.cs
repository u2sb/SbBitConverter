using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SbBitConverter.SourceGenerator.Tests.Infrastructure;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   增量性测试（防回归防线）：编辑与本生成器无关的文件后，本生成器自身的步骤不应重跑。
///   <para>
///     跟踪步骤名（<see cref="SbBitConverter.SourceGenerator.Modeling.ModelConstants" />）被改名时，
///     测试会以「未找到跟踪步骤」失败，而不是静默漏测。
///   </para>
/// </summary>
public class IncrementalityTests
{
  /// <summary>本生成器自己的跟踪步骤。</summary>
  private static readonly string[] OwnedStepNames =
  [
    "SbBitConverter.PointerSize",
    "SbBitConverter.ArrayCandidate",
    "SbBitConverter.StructCandidate",
    "SbBitConverter.ArrayModel",
    "SbBitConverter.StructModel",
  ];

  private const string UnrelatedFile = """
    namespace Unrelated.Added
    {
      internal sealed class AddedLater
      {
        public static int Value => 42;
      }
    }
    """;

  /// <summary>跑两次驱动：第二次仅追加一个与本生成器无关的语法树。</summary>
  private static (GeneratorDriver Driver, List<string> Offenders) RunTwiceWithUnrelatedEdit()
  {
    var first = GeneratorTestHost.CreateCompilation(GeneratorTestHost.ModelsSource, "IncrAsm1");
    var second = first.AddSyntaxTrees(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(
      UnrelatedFile, new Microsoft.CodeAnalysis.CSharp.CSharpParseOptions(LanguageVersion.Preview)));

    var driver = GeneratorTestHost.CreateDriver(GeneratorTestHost.GeneratorBranch.Roslyn48, trackSteps: true);
    driver = driver.RunGenerators(first);
    driver = driver.RunGenerators(second);

    var tracked = driver.GetRunResult().Results
      .SelectMany(static r => r.TrackedSteps)
      .ToDictionary(static kv => kv.Key, static kv => kv.Value, StringComparer.Ordinal);

    var offenders = new List<string>();
    foreach (var name in OwnedStepNames)
    {
      Assert.True(tracked.ContainsKey(name),
        $"未找到跟踪步骤「{name}」—— 跟踪名被改名，或该步骤已从管线中移除。当前键：{string.Join(", ", tracked.Keys.OrderBy(static k => k, StringComparer.Ordinal))}");

      foreach (var (value, reason) in tracked[name].SelectMany(static s => s.Outputs))
      {
        if (reason is not (IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged))
        {
          offenders.Add($"{name}: {reason}");
        }
      }
    }

    return (driver, offenders);
  }

  [Fact]
  public void Unrelated_Edit_Does_Not_Reprocess_Owned_Steps()
  {
    var (_, offenders) = RunTwiceWithUnrelatedEdit();

    Assert.True(offenders.Count == 0,
      "以下生成器步骤在无关编辑后重跑（增量缓存失效）：\n" + string.Join("\n", offenders));
  }

  /// <summary>第二次运行时，输出内容必须与第一次完全一致（无重复输出 / 无丢失）。</summary>
  [Fact]
  public void Second_Run_Output_Is_Stable()
  {
    var first = GeneratorTestHost.RunOnce(GeneratorTestHost.GeneratorBranch.Roslyn48, GeneratorTestHost.ModelsSource)
      .GeneratedSources();
    var second = RunTwiceWithUnrelatedEdit().Driver.GeneratedSources();

    Assert.Equal(first.Keys, second.Keys);
    foreach (var (name, source) in first)
    {
      Assert.True(source == second[name], $"第二次运行产物变化：{name}");
    }
  }
}
