using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manager for resources loads.
/// </summary>
public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// Load resource synchronizely.
    /// </summary>
    /// <typeparam name="T">The type of resource.</typeparam>
    /// <param name="path">The relative path of resource in the folder "Resources".</param>
    /// <returns>The resource.</returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        return res;
    }

    /// <summary>
    /// Load resource asynchronizely.
    /// </summary>
    /// <typeparam name="T">The type of resource.</typeparam>
    /// <param name="path">The relative path of resource in the folder "Resources".</param>
    /// <param name="callback">The function called when the loading was complete.</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync(path, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        callback(r.asset as T);
    }

}
