using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of singleton mode.
/// </summary>
/// <typeparam name="T">The class type of singleton class.</typeparam>
public class Singleton<T> where T: new()
{
    private static T instance;
    /// <summary>
    /// ÊµÀý
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}
