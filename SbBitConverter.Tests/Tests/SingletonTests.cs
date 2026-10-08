// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.Singletons;

namespace SbBitConverter.Tests.Tests;

public class SingletonTests
{
  [Fact]
  public void Instance_SameObject()
  {
    var a = TestSingleton.Instance;
    var b = TestSingleton.Instance;
    Assert.Same(a, b);
  }

  [Fact]
  public void Instance_NotNull() => Assert.NotNull(TestSingleton.Instance);

  [Fact]
  public void ProtectedMembers_AccessibleFromDerived() => Assert.True(DerivedSingleton.Instance.TestLockerAccess());

  private class TestSingleton : Singleton<TestSingleton>
  {
    private TestSingleton()
    {
    }

    public Guid Id { get; } = Guid.NewGuid();
  }

  private class DerivedSingleton : Singleton<DerivedSingleton>
  {
    private DerivedSingleton()
    {
    }

    public bool TestLockerAccess() => Lock != null;
  }
}
