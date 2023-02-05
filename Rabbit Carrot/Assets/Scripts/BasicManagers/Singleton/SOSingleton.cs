using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Base class of singleton mode of scriptable objects.
/// </summary>
/// <typeparam name="T">The scriptable object class needed to be singleton mode.</typeparam>
public abstract class SOSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static string DEFAULT_PATH =>"SO/" + typeof(T).Name;
    private static T instance;
    public static T Instance
    {
        get
        {
            string path;
            if (instance == null)
            {
                System.Type type = typeof(T);
                object[] attributes = type.GetCustomAttributes(typeof(SOFilePathAttribute), true);
                if (attributes.Length > 0)
                {
                    path = (attributes[0] as SOFilePathAttribute).Path;
                }
                else
                    path = DEFAULT_PATH;
                instance = ResourceManager.Instance.Load<T>(path);
                
                if (instance == null)
                {
                    Debug.LogError($"SO file of singleton type {typeof(T).Name} cannot be found at \"{path}\".");
                }
            }
            return instance;
        }
    }
}

/// <summary>
/// Attribute to give singleton SO a file path.
/// </summary>
public class SOFilePathAttribute : Attribute
{
    /// <summary>
    /// The path used by assetdatabase to load the file.
    /// </summary>
    public string Path { get; set; }
}