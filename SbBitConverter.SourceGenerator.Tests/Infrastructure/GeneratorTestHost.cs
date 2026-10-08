using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SbBitConverter.Attributes;

namespace SbBitConverter.SourceGenerator.Tests.Infrastructure;

/// <summary>
///   源生成器测试宿主：构造内存编译，分别驱动 4.8 / 4.3 两个生成器分支。
/// </summary>
public static class GeneratorTestHost
{
  /// <summary>生成器分支。</summary>
  public enum GeneratorBranch
  {
    /// <summary>Roslyn 4.8 API 分支（直接引用）。</summary>
    Roslyn48,

    /// <summary>Roslyn 4.3 API 分支（AssemblyLoadContext 加载）。</summary>
    Roslyn43
  }

  private static readonly Lazy<ImmutableArray<MetadataReference>> ReferencesLazy = new(BuildReferences);
  private static readonly Lazy<IIncrementalGenerator> Gen43Lazy = new(LoadGen43);

  /// <summary>编译被测源码所需的元数据引用。</summary>
  public static ImmutableArray<MetadataReference> References => ReferencesLazy.Value;

  /// <summary>与运行时测试共享的模型源码。</summary>
  public static string ModelsSource => File.ReadAllText(
    Path.Combine(AppContext.BaseDirectory, "Models", "Models.cs"));

  /// <summary>
  ///   与生产项目 ImplicitUsings=enable 等价的全局 using（被测模型源码依赖它）。
  /// </summary>
  private const string ImplicitUsingsSource = """
    global using global::System;
    global using global::System.Collections.Generic;
    global using global::System.IO;
    global using global::System.Linq;
    global using global::System.Runtime.CompilerServices;
    global using global::System.Runtime.InteropServices;
    global using global::System.Threading;
    global using global::System.Threading.Tasks;
    """;

  /// <summary>创建内存编译。</summary>
  public static CSharpCompilation CreateCompilation(string source, string assemblyName = "GeneratorTestAsm")
    => CSharpCompilation.Create(
      assemblyName,
      [
        CSharpSyntaxTree.ParseText(ImplicitUsingsSource, ParseOptions),
        CSharpSyntaxTree.ParseText(source, ParseOptions),
      ],
      References,
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

  /// <summary>把生成器产物加入编译，得到「源码 + 生成代码」的完整编译。</summary>
  public static CSharpCompilation CompileWithGenerated(GeneratorBranch branch, string source)
  {
    var trees = RunOnce(branch, source).GetRunResult().Results
      .SelectMany(static r => r.GeneratedSources)
      .Select(static s => s.SyntaxTree);
    return CreateCompilation(source).AddSyntaxTrees(trees);
  }

  /// <summary>创建指定分支的生成器驱动。</summary>
  public static GeneratorDriver CreateDriver(GeneratorBranch branch, bool trackSteps = false)
  {
    var generator = branch switch
    {
      GeneratorBranch.Roslyn48 => new global::SbBitConverter.SourceGenerator.SbStructGenerator(),
      GeneratorBranch.Roslyn43 => Gen43Lazy.Value,
      _ => throw new ArgumentOutOfRangeException(nameof(branch)),
    };

    return CSharpGeneratorDriver.Create(
      [generator.AsSourceGenerator()],
      parseOptions: ParseOptions,
      driverOptions: new GeneratorDriverOptions(
        IncrementalGeneratorOutputKind.None,
        trackIncrementalGeneratorSteps: trackSteps));
  }

  /// <summary>在给定源码上运行一次指定分支的生成器。</summary>
  public static GeneratorDriver RunOnce(GeneratorBranch branch, string source, bool trackSteps = false)
    => CreateDriver(branch, trackSteps).RunGenerators(CreateCompilation(source));

  /// <summary>生成结果的「hintName → 源码文本」映射。</summary>
  public static SortedDictionary<string, string> GeneratedSources(this GeneratorDriver driver)
  {
    var result = new SortedDictionary<string, string>(StringComparer.Ordinal);
    foreach (var r in driver.GetRunResult().Results)
    foreach (var s in r.GeneratedSources)
    {
      result[s.HintName] = s.SourceText.ToString();
    }

    return result;
  }

  /// <summary>生成器的全部诊断。</summary>
  public static ImmutableArray<Diagnostic> GeneratorDiagnostics(this GeneratorDriver driver)
    => driver.GetRunResult().Diagnostics;

  /// <summary>编译错误。</summary>
  public static Diagnostic[] Errors(this Compilation compilation)
    => compilation.GetDiagnostics().Where(static d => d.Severity == DiagnosticSeverity.Error).ToArray();

  /// <summary>基线目录。</summary>
  public static string BaselineDirectory => Path.Combine(AppContext.BaseDirectory, "Baselines");

  /// <summary>读取全部基线：文件名 → 文本。</summary>
  public static SortedDictionary<string, string> ReadBaselines()
  {
    var result = new SortedDictionary<string, string>(StringComparer.Ordinal);
    foreach (var f in Directory.EnumerateFiles(BaselineDirectory, "*.g.cs"))
    {
      result[Path.GetFileName(f)] = File.ReadAllText(f);
    }

    return result;
  }

  /// <summary>判断 DLL 是否包含托管元数据（排除原生 DLL，避免 CS0009）。</summary>
  private static bool HasManagedMetadata(string path)
  {
    try
    {
      using var stream = File.OpenRead(path);
      using var pe = new System.Reflection.PortableExecutable.PEReader(stream);
      return pe.HasMetadata;
    }
    catch (Exception ex) when (ex is IOException or BadImageFormatException)
    {
      return false;
    }
  }

  private static CSharpParseOptions ParseOptions => new(LanguageVersion.Preview);

  /// <summary>用可回收的 AssemblyLoadContext 加载 4.3 分支生成器（与 4.8 同名程序集，不能同域加载）。</summary>
  private static IIncrementalGenerator LoadGen43()
  {
    var path = Path.Combine(AppContext.BaseDirectory, "Gen43", "SbBitConverter.SourceGenerator.dll");
    if (!File.Exists(path)) throw new FileNotFoundException("未找到 4.3 分支生成器 DLL", path);

    var context = new AssemblyLoadContext($"Gen43_{Guid.NewGuid():N}", isCollectible: true);
    var assembly = context.LoadFromAssemblyPath(path);
    var generatorType = assembly.GetTypes()
      .Single(static t => typeof(IIncrementalGenerator).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });
    return (IIncrementalGenerator)Activator.CreateInstance(generatorType)!;
  }

  private static ImmutableArray<MetadataReference> BuildReferences()
  {
    var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var list = new List<MetadataReference>();

    void AddFile(string path)
    {
      if (seen.Add(path)) list.Add(MetadataReference.CreateFromFile(path));
    }

    void Add(Assembly? assembly)
    {
      if (assembly is null || assembly.IsDynamic) return;
      string location;
      try
      {
        location = assembly.Location;
      }
      catch (NotSupportedException)
      {
        return;
      }

      if (string.IsNullOrEmpty(location) || !File.Exists(location)) return;
      AddFile(location);
    }

    // 运行时全部程序集（保证 Span<T> / 异常类型等 facade 齐全），跳过无托管元数据的原生 DLL
    var runtimeDir = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
    foreach (var dll in Directory.EnumerateFiles(runtimeDir, "*.dll"))
    {
      if (!HasManagedMetadata(dll)) continue;
      AddFile(dll);
    }

    foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) Add(a);
    Add(typeof(object).Assembly);
    Add(typeof(Span<>).Assembly);
    Add(typeof(System.Runtime.InteropServices.Marshal).Assembly);
    Add(typeof(SbBitConverterArrayAttribute).Assembly);
    foreach (var name in new[]
             {
               "System.Runtime", "System.Collections", "System.Linq", "netstandard",
               "System.Runtime.InteropServices", "System.Memory"
             })
    {
      try
      {
        Add(Assembly.Load(name));
      }
      catch (Exception ex) when (ex is FileNotFoundException or FileLoadException)
      {
        // 可选程序集，缺失不致命
      }
    }

    return list.ToImmutableArray();
  }
}
