using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using static SbBitConverter.SourceGenerator.Modeling.ModelConstants;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   把标注了 [SbBitConverterStruct] 的结构体符号投影为 <see cref="StructModel" />。
///   两阶段建模，见 <see cref="ArrayModelBuilder" /> 的注释。
/// </summary>
internal static class StructModelBuilder
{
  /// <summary>第一阶段：符号 → 可比较候选模型。</summary>
  public static BuildResult<StructCandidate> TryBuild(INamedTypeSymbol structSymbol, AttributeData attribute)
  {
    var location = ArrayModelBuilder.GetLocation(structSymbol);

    // 1. 编码模式（缺省 DCBA = 0）
    var mode = (byte)0;
    if (attribute.ConstructorArguments.Length > 0
        && !ArrayModelBuilder.TryGetByte(attribute.ConstructorArguments[0].Value, out mode))
    {
      mode = 0;
    }

    // 2. SBBIT0007：同一 struct 同时标注两个特性会产出重名成员，拒绝生成
    if (structSymbol.ContainsAttribute("SbBitConverter.Attributes.SbBitConverterArrayAttribute"))
    {
      return BuildResult<StructCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.BothAttributesOnSameStruct, location, structSymbol.Name));
    }

    // 3. 嵌套 / 泛型 struct 不支持
    if (structSymbol.ContainingType is not null || structSymbol.Arity > 0)
    {
      return BuildResult<StructCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.StructNestedOrGeneric, location, structSymbol.Name));
    }

    // 3. 非 partial struct 无法生成
    if (!ArrayModelBuilder.IsPartial(structSymbol))
    {
      return BuildResult<StructCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.StructNotPartial, location, structSymbol.Name));
    }

    // 4. 收集 [FieldOffset] 字段（与旧实现一致：显式字段 + 自动属性的 backing field）
    var fields = new List<StructFieldCandidate>();
    foreach (var (name, type, offset) in CollectFieldData(structSymbol))
    {
      fields.Add(new StructFieldCandidate(name, type.ToDisplayString(), offset, Utils.GetSizeCode(type)));
    }

    // 5. 无字段 → 不生成代码，仅报告诊断（旧实现静默跳过）
    if (fields.Count == 0)
    {
      return BuildResult<StructCandidate>.From(null,
        DiagnosticInfo.Create(DiagnosticDescriptors.NoFieldOffsetMembers, location, structSymbol.Name));
    }

    var isGlobalNamespace = structSymbol.ContainingNamespace.IsGlobalNamespace;
    var namespaceName = isGlobalNamespace ? string.Empty : structSymbol.ContainingNamespace.ToDisplayString();

    var candidate = new StructCandidate(
      HintName: isGlobalNamespace
        ? $"{structSymbol.Name}_SbBitConverterStruct.g.cs"
        : $"{namespaceName}_{structSymbol.Name}_SbBitConverterStruct.g.cs",
      StructName: structSymbol.Name,
      NamespaceName: namespaceName,
      IsGlobalNamespace: isGlobalNamespace,
      Mode: mode,
      Fields: new EquatableArray<StructFieldCandidate>(fields.ToImmutableArray()),
      Location: Modeling.LocationInfo.From(location));

    return BuildResult<StructCandidate>.From(candidate);
  }

  /// <summary>第二阶段：结合指针宽度补全尺寸并做最终校验。</summary>
  public static BuildResult<StructModel> Complete(StructCandidate candidate, int pointerSize)
  {
    var fields = new List<StructFieldModel>(candidate.Fields.Count);
    var undeterminable = new List<DiagnosticInfo>();
    foreach (var field in candidate.Fields)
    {
      var size = Utils.SizeFromCode(field.SizeCode, pointerSize);
      if (field.SizeCode == Utils.UndeterminableSizeCode)
      {
        // 尺寸完全无法确定的类型（引用类型等）：保持 -1，发射器按旧行为生成空行（静默跳过）
        size = Utils.UndeterminableSizeCode;
        undeterminable.Add(DiagnosticInfo.Create(
          DiagnosticDescriptors.FieldSizeUndeterminable, candidate.Location,
          candidate.StructName, field.Name, field.TypeName));
      }
      else if (size is not (0 or 1 or 2 or 4 or 8))
      {
        // 尺寸可确定但不在支持列表（如 decimal=16）：旧行为同样生成空行
        undeterminable.Add(DiagnosticInfo.Create(
          DiagnosticDescriptors.FieldSizeUndeterminable, candidate.Location,
          candidate.StructName, field.Name, field.TypeName));
      }

      fields.Add(new StructFieldModel(field.Name, field.TypeName, field.Offset, size));
    }

    return BuildResult<StructModel>.From(new StructModel(
      candidate.HintName,
      candidate.StructName,
      candidate.NamespaceName,
      candidate.IsGlobalNamespace,
      candidate.Mode,
      new EquatableArray<StructFieldModel>(fields.ToImmutableArray())),
      undeterminable.ToArray());
  }

  private static IEnumerable<(string Name, ITypeSymbol Type, int Offset)> CollectFieldData(INamedTypeSymbol structSymbol)
  {
    foreach (var member in structSymbol.GetMembers())
    {
      switch (member)
      {
        case IFieldSymbol { IsImplicitlyDeclared: false } field:
        {
          if (GetFieldOffset(field) is { } offset)
            yield return (field.Name, field.Type, offset);
          break;
        }
        case IPropertySymbol property:
        {
          foreach (var f in structSymbol.GetMembers())
          {
            if (f is IFieldSymbol { IsImplicitlyDeclared: true } backingField
                && backingField.AssociatedSymbol?.Equals(property, SymbolEqualityComparer.Default) == true
                && GetFieldOffset(backingField) is { } offset)
            {
              yield return (property.Name, property.Type, offset);
            }
          }

          break;
        }
      }
    }
  }

  private static int? GetFieldOffset(IFieldSymbol field)
  {
    foreach (var attribute in field.GetAttributes())
    {
      if (attribute.AttributeClass is not null
          && attribute.AttributeClass.ToDisplayString() == "System.Runtime.InteropServices.FieldOffsetAttribute"
          && attribute.ConstructorArguments.Length > 0
          && attribute.ConstructorArguments[0].Value is int offset)
      {
        return offset;
      }
    }

    return null;
  }
}
