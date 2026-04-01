using System;
using System.Reflection;
using Microsoft.VisualStudio.Threading;

namespace Sb.Extensions.Singletons;

/// <summary>
///   单例
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>
{
  // ReSharper disable once InconsistentNaming
  private static readonly Lazy<T> instance = new(CreateInstance);

  /// <summary>
  ///   锁
  /// </summary>
  protected ReentrantSemaphore Lock { get; } =
    ReentrantSemaphore.Create(1, null, ReentrantSemaphore.ReentrancyMode.Stack);

  /// <summary>
  ///   单例
  /// </summary>
  public static T Instance => instance.Value;

  private static T CreateInstance()
  {
    if (Activator.CreateInstance(typeof(T), true) is T t)
    {
      return t;
    }

    throw new TargetInvocationException($"Could not create instance of type {typeof(T).FullName}", null);
  }
}
