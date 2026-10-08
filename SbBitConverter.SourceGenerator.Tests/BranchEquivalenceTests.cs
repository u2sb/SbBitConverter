using SbBitConverter.SourceGenerator.Tests.Infrastructure;

namespace SbBitConverter.SourceGenerator.Tests;

/// <summary>
///   双分支等价性测试：4.3 与 4.8 两个源生成器对同一输入必须产出完全一致的代码。
/// </summary>
public class BranchEquivalenceTests
{
  [Fact]
  public void Both_Branches_Produce_Identical_Output_For_Models()
  {
    var gen48 = GeneratorTestHost.RunOnce(GeneratorTestHost.GeneratorBranch.Roslyn48, GeneratorTestHost.ModelsSource)
      .GeneratedSources();
    var gen43 = GeneratorTestHost.RunOnce(GeneratorTestHost.GeneratorBranch.Roslyn43, GeneratorTestHost.ModelsSource)
      .GeneratedSources();

    Assert.Equal(gen48.Keys, gen43.Keys);

    foreach (var (name, source) in gen48)
    {
      Assert.True(gen43.TryGetValue(name, out var other), $"4.3 分支缺少生成文件：{name}");
      Assert.True(source == other, $"两分支产物不一致：{name}");
    }
  }
}
