using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A object buffer to save GameObject, and recycle it for second use.
/// </summary>
public class ObjectBuffer
{

    private Transform fatherTransform;
    private Dictionary<GameObject, Stack<GameObject>> objDic = new Dictionary<GameObject, Stack<GameObject>>();

    /// <summary>
    /// Initial the object buffer with the transform parent.
    /// It will be the parent object of all GameObjects created by this buffer.
    /// </summary>
    /// <param name="parent">The parent of all GameObjects created by this buffer.</param>
    public ObjectBuffer(Transform parent)
    {
        fatherTransform = parent;
    }
    /// <summary>
    /// Destroy all GameObject created by this buffer.
    /// </summary>
    public void Clear()
    {
        foreach(var item in objDic)
        {
            foreach(var obj in item.Value)
            {
                GameObject.Destroy(obj);
            }
        }
        objDic.Clear();
    }

    /// <summary>
    /// Get a recycled GameObject from buffer with the original prefabs.
    /// </summary>
    /// <param name="key">The original prefab of the GameObject..</param>
    /// <param name="action">Action to game object before enable it.</param>
    /// <returns></returns>
    public GameObject Get(GameObject key, System.Action<GameObject> action = null)
    {
        if (!objDic.ContainsKey(key))
            objDic.Add(key, new Stack<GameObject>());
        
        Stack<GameObject> objList = objDic[key];
        if (objList.Count > 0)
        {
            GameObject obj = objList.Pop();
            action?.Invoke(obj);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(key, fatherTransform);
            action?.Invoke(obj);
            return obj;
        }
    }
    /// <summary>
    /// Put a GameObject into the buffer and recycle it.
    /// </summary>
    /// <param name="key">The original prefab the GameObject.</param>
    /// <param name="obj">The GameObject to be put.</param>
    public void Put(GameObject key, GameObject obj)
    {
        if (!objDic[key].Contains(obj))
        {
            objDic[key].Push(obj);
            obj.SetActive(false);
        }
    }
}
