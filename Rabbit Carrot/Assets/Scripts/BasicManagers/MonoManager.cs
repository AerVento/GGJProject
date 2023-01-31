using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Provides Update() methods and coroutines for extern non-mono classes.
/// 
/// </summary>
public class MonoManager : Singleton<MonoManager>
{
    private MonoController controller;
    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// Add update listener to the scene.
    /// </summary>
    /// <param name="fun">Update function.</param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// Remove a update listener from the scene.
    /// </summary>
    /// <param name="fun">Update function.</param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// Start a coroutine in the scene by enumerator.
    /// </summary>
    /// <param name="routine">The enumerator used to start a coroutine.</param>
    /// <returns>The coroutine.</returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    /// <summary>
    /// Start a coroutine in the scene by method name.
    /// </summary>
    /// <param name="methodName">The name of coroutine function.</param>
    /// <param name="value">The paramters of coroutine function.</param>
    /// <returns>The coroutine.</returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// Start a coroutine in the scene by method name.
    /// </summary>
    /// <param name="methodName">The name of coroutine function.</param>
    /// <returns>The coroutine.</returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public void StopCoroutine(Coroutine routine) => controller.StopCoroutine(routine);
    public void StopCoroutine(IEnumerator routine) => controller.StopCoroutine(routine);

}
