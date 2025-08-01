using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SbBitConverter.SourceGenerator;

internal static class SymbolExtensions
{
  public static bool ContainsAttribute(this ISymbol symbol, INamedTypeSymbol attribtue)
  {
    return symbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribtue));
  }

  public static bool ContainsAttribute(this ISymbol symbol, string attribtueName)
  {
    return symbol.GetAttributes()
      .Any(x => x.AttributeClass != null && x.AttributeClass.ToDisplayString() == attribtueName);
  }

  public static AttributeData? GetAttribute(this ISymbol symbol, INamedTypeSymbol attribtue)
  {
    return symbol.GetAttributes()
      .FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribtue));
  }

  public static AttributeData? GetAttribute(this ISymbol symbol, string attribtueName)
  {
    return symbol.GetAttributes()
      .FirstOrDefault(x => x.AttributeClass != null && x.AttributeClass.ToDisplayString() == attribtueName);
  }

  public static T? GetAttributeNamedArguments<T>(this AttributeData attributeData, string name)
  {
    foreach (var argument in attributeData.NamedArguments)
      if (argument.Key == name)
        return (T?)argument.Value.Value;

    return default;
  }

  public static AttributeData? GetImplAttribute(this ISymbol symbol, INamedTypeSymbol implAttribtue)
  {
    return symbol.GetAttributes().FirstOrDefault(x =>
    {
      if (x.AttributeClass == null) return false;
      if (x.AttributeClass.EqualsUnconstructedGenericType(implAttribtue)) return true;

      return x.AttributeClass.GetAllBaseTypes().Any(item => item.EqualsUnconstructedGenericType(implAttribtue));
    });
  }

  public static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol, bool withoutOverride = true)
  {
    // Iterate Parent -> Derived
    if (symbol.BaseType != null)
      foreach (var item in GetAllMembers(symbol.BaseType))
        // override item already iterated in parent type
        if (!withoutOverride || !item.IsOverride)
          yield return item;

    foreach (var item in symbol.GetMembers())
      if (!withoutOverride || !item.IsOverride)
        yield return item;
  }

  public static bool InheritsFrom(this INamedTypeSymbol symbol, INamedTypeSymbol baseSymbol)
  {
    var baseName = baseSymbol.ToString();
    while (true)
    {
      if (symbol.ToString() == baseName) return true;
      if (symbol.BaseType != null)
      {
        symbol = symbol.BaseType;
        continue;
      }

      break;
    }

    return false;
  }

  public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol symbol)
  {
    var t = symbol.BaseType;
    while (t != null)
    {
      yield return t;
      t = t.BaseType;
    }
  }

  public static bool EqualsUnconstructedGenericType(this INamedTypeSymbol left, INamedTypeSymbol right)
  {
    var l = left.IsGenericType ? left.ConstructUnboundGenericType() : left;
    var r = right.IsGenericType ? right.ConstructUnboundGenericType() : right;
    return SymbolEqualityComparer.Default.Equals(l, r);
  }

  public static bool AllowUnsafeSource(this ITypeSymbol typeSymbol)
  {
    return typeSymbol.SpecialType switch
    {
      // 1字节类型
      SpecialType.System_Byte or SpecialType.System_SByte or SpecialType.System_Boolean => true,

      // 2字节类型
      SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Char => true,

      // 4字节类型
      SpecialType.System_Int32 or SpecialType.System_UInt32 or SpecialType.System_Single => true,

      // 8字节类型
      SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Double => true,

      // 16字节类型
      SpecialType.System_Decimal => true,

      _ => false
    };
  }
}