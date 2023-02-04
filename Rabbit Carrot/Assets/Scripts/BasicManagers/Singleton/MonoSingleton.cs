using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton of mono scripts.
/// </summary>
/// <typeparam name="T">The class type of singleton mono script.</typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get => instance;
    }
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
