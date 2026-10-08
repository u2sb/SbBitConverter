// 补充覆盖：对应 Sb.Extensions\System\Collections\Generic\CollectionExtensions.cs
// 说明：AddRange(ReadOnlySpan)/InsertRange 仅在 !NET8_0_OR_GREATER 下编译，net10.0 不可测；
//       KeyValuePair.Deconstruct 与 BCL 内置实例方法同名，编译器优先选内置实现，不单独断言。
using System.Collections.ObjectModel;
using Sb.Extensions.System.Collections.Generic;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   CollectionExtensions 测试
/// </summary>
public class CollectionExtensionsTests
{
  // ── Dictionary.Remove(key, out value) ──

  [Fact]
  public void DictionaryRemove_ExistingKey_ReturnsTrueAndValue()
  {
    var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
    var removed = dict.Remove("a", out var value);
    Assert.True(removed);
    Assert.Equal(1, value);
    Assert.False(dict.ContainsKey("a"));
    Assert.Equal(1, dict.Count);
  }

  [Fact]
  public void DictionaryRemove_MissingKey_ReturnsFalse()
  {
    var dict = new Dictionary<string, int> { ["a"] = 1 };
    var removed = dict.Remove("z", out var value);
    Assert.False(removed);
    Assert.Equal(0, value); // default
    Assert.Equal(1, dict.Count); // 未被移除
  }

  [Fact]
  public void DictionaryRemove_SameKeyTwice_SecondReturnsFalse()
  {
    var dict = new Dictionary<int, string> { [1] = "x" };
    Assert.True(dict.Remove(1, out var first));
    Assert.Equal("x", first);
    Assert.False(dict.Remove(1, out var second));
    Assert.Null(second);
  }

  // ── SortedDictionary.Remove(key, out value) ──

  [Fact]
  public void SortedDictionaryRemove_ExistingKey_ReturnsTrueAndValue()
  {
    var dict = new SortedDictionary<int, string> { [1] = "a", [2] = "b" };
    var removed = dict.Remove(2, out var value);
    Assert.True(removed);
    Assert.Equal("b", value);
    Assert.Equal(1, dict.Count);
  }

  [Fact]
  public void SortedDictionaryRemove_MissingKey_ReturnsFalse()
  {
    var dict = new SortedDictionary<int, string> { [1] = "a" };
    var removed = dict.Remove(42, out var value);
    Assert.False(removed);
    Assert.Null(value);
    Assert.Equal(1, dict.Count);
  }

  // ── ObservableCollection.AddRange ──

  [Fact]
  public void ObservableCollectionAddRange_FromEnumerable_AppendsAll()
  {
    var collection = new ObservableCollection<int> { 1 };
    collection.AddRange(new[] { 2, 3, 4 });
    Assert.Equal(new[] { 1, 2, 3, 4 }, collection);
  }

  [Fact]
  public void ObservableCollectionAddRange_FromSpan_AppendsAll()
  {
    var collection = new ObservableCollection<string>();
    collection.AddRange(new[] { "x", "y" }.AsSpan());
    Assert.Equal(new[] { "x", "y" }, collection);
  }

  [Fact]
  public void ObservableCollectionAddRange_Empty_NoChange()
  {
    var collection = new ObservableCollection<int> { 7 };
    collection.AddRange(new int[0]);
    Assert.Equal(new[] { 7 }, collection);
  }

  [Fact]
  public void ObservableCollectionAddRange_RaisesCollectionChangedPerItem()
  {
    var collection = new ObservableCollection<int>();
    var changes = 0;
    collection.CollectionChanged += (_, _) => changes++;
    collection.AddRange(new[] { 1, 2, 3 });
    Assert.Equal(3, changes);
  }

  // ── ObservableCollection.SetRange ──

  [Fact]
  public void SetRange_ShorterThanCollection_RemovesTrailingItems()
  {
    var collection = new ObservableCollection<int> { 1, 2, 3, 4 };
    collection.SetRange(new[] { 9 });
    Assert.Equal(new[] { 9 }, collection);
  }

  [Fact]
  public void SetRange_LongerThanCollection_AppendsMissing()
  {
    var collection = new ObservableCollection<int> { 1 };
    collection.SetRange(new[] { 1, 2, 3 });
    Assert.Equal(new[] { 1, 2, 3 }, collection);
  }

  [Fact]
  public void SetRange_SameLength_ReplacesInPlace()
  {
    var collection = new ObservableCollection<int> { 1, 2 };
    collection.SetRange(new[] { 8, 9 });
    Assert.Equal(new[] { 8, 9 }, collection);
  }

  [Fact]
  public void SetRange_Empty_ClearsCollection()
  {
    var collection = new ObservableCollection<int> { 1, 2, 3 };
    collection.SetRange([]);
    Assert.Empty(collection);
  }
}
