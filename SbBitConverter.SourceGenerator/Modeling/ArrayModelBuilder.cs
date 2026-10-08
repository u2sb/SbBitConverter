using Microsoft.CodeAnalysis;
using static SbBitConverter.SourceGenerator.Modeling.ModelConstants;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   把标注了 [SbBitConverterArray] 的结构体符号投影为 <see cref="ArrayModel" />。
///   <para>
///     分两个阶段保证增量可比较性：
///     <see cref="TryBuild" /> 在 transform 阶段把符号解码为不含指针宽度的
///     <see cref="ArrayCandidate" />（值可比较）；
///     <see cref="Complete" /> 在 Combine(pointerSize) 之后的第二阶段补全尺寸。
///     严禁让 <see cref="ITypeSymbol" /> / <see cref="Compilation" /> 进入缓存模型。
///   </para>
/// </summary>
internal static class ArrayModelBuilder
{
  /// <summary>第一阶段：符号 → 可比较候选模型（尺寸以编码表示，不含指针宽度）。</summary>
  public static BuildResult<ArrayCandidate> TryBuild(INamedTypeSymbol structSymbol, AttributeData attribute)
  {
    var location = GetLocation(structSymbol);

    // 1. 解析特性构造参数：(元素类型, 长度, 编码模式)
    var ctorArgs = attribute.ConstructorArguments;
    if (ctorArgs.Length < 3
        || ctorArgs[0].Value is not INamedTypeSymbol elementType
        || ctorArgs[1].Value is not int length
        || !TryGetByte(ctorArgs[2].Value, out var mode))
    {
      return BuildResult<ArrayCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.ArrayAttributeInvalid, location, structSymbol.Name));
    }

    // 2. SBBIT0007：同一 struct 同时标注两个特性会产出重名成员，拒绝生成
    if (structSymbol.ContainsAttribute(ConstTable.SbBitConverterStructAttributeName))
    {
      return BuildResult<ArrayCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.BothAttributesOnSameStruct, location, structSymbol.Name));
    }

    // 3. 嵌套 / 泛型 struct 不支持
    if (structSymbol.ContainingType is not null || structSymbol.Arity > 0)
    {
      return BuildResult<ArrayCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.StructNestedOrGeneric, location, structSymbol.Name));
    }

    // 4. 非 partial struct 无法生成
    if (!IsPartial(structSymbol))
    {
      return BuildResult<ArrayCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.StructNotPartial, location, structSymbol.Name));
    }

    var isGlobalNamespace = structSymbol.ContainingNamespace.IsGlobalNamespace;
    var namespaceName = isGlobalNamespace ? string.Empty : structSymbol.ContainingNamespace.ToDisplayString();
    var structName = structSymbol.Name;

    // 5. 解析可选的 ElementSize 命名参数（<=0 时回退到类型尺寸推断）
    var explicitElementSize = GetNamedInt(attribute, "ElementSize") ?? 0;

    var candidate = new ArrayCandidate(
      HintName: isGlobalNamespace
        ? $"{structName}_SbBitConverterArray.g.cs"
        : $"{namespaceName}_{structName}_SbBitConverterArray.g.cs",
      StructName: structName,
      NamespaceName: namespaceName,
      IsGlobalNamespace: isGlobalNamespace,
      ElementTypeName: elementType.ToDisplayString(),
      ElementSizeCode: Utils.GetSizeCode(elementType),
      IsBaseType: Utils.IsKnownUnmanagedType(elementType),
      IsEnumType: elementType.TypeKind == TypeKind.Enum,
      ExplicitElementSize: explicitElementSize,
      Length: length,
      Mode: mode,
      HasStructLayoutAttribute: structSymbol.ContainsAttribute(
        "System.Runtime.InteropServices.StructLayoutAttribute"),
      IsReadonlyStruct: structSymbol.IsReadOnly,
      Location: LocationInfo.From(location));

    return BuildResult<ArrayCandidate>.From(candidate);
  }

  /// <summary>第二阶段：结合指针宽度补全尺寸并做最终校验。</summary>
  public static BuildResult<ArrayModel> Complete(ArrayCandidate candidate, int pointerSize)
  {
    var elementSize = candidate.ExplicitElementSize;
    if (elementSize <= 0) elementSize = Utils.SizeFromCode(candidate.ElementSizeCode, pointerSize);

    if (candidate.Length <= 0 || elementSize <= 0)
    {
      return BuildResult<ArrayModel>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.ArraySizeInvalid, candidate.Location,
          candidate.StructName, candidate.Length, elementSize));
    }

    return BuildResult<ArrayModel>.From(new ArrayModel(
      candidate.HintName,
      candidate.StructName,
      candidate.NamespaceName,
      candidate.IsGlobalNamespace,
      candidate.ElementTypeName,
      elementSize,
      candidate.Length,
      candidate.Mode,
      candidate.HasStructLayoutAttribute,
      candidate.IsBaseType,
      candidate.IsEnumType,
      candidate.IsReadonlyStruct));
  }

  internal static int? GetNamedInt(AttributeData attribute, string name)
  {
    foreach (var argument in attribute.NamedArguments)
    {
      if (argument.Key == name) return argument.Value.Value as int?;
    }

    return null;
  }

  internal static bool TryGetByte(object? value, out byte result)
  {
    switch (value)
    {
      case byte b: result = b; return true;
      case int i and (>= 0 and <= 255): result = (byte)i; return true;
      default: result = 0; return false;
    }
  }

  internal static Location? GetLocation(INamedTypeSymbol structSymbol)
  {
    foreach (var reference in structSymbol.DeclaringSyntaxReferences)
    {
      var location = reference.GetSyntax().GetLocation();
      if (location.IsInSource) return location;
    }

    return null;
  }

  internal static bool IsPartial(INamedTypeSymbol structSymbol)
  {
    foreach (var reference in structSymbol.DeclaringSyntaxReferences)
    {
      if (reference.GetSyntax() is Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax declaration
          && declaration.Modifiers.Any(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword))
      {
        return true;
      }
    }

    return false;
  }
}
