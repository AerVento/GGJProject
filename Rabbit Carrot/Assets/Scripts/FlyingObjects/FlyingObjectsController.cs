using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FlyingObjectsController
{
    private Dictionary<Type, GameObject> prefabDic;
    private ObjectBuffer flyingObjectsBuffer; 
    private Dictionary<GameObject,List<GameObject>> activeObjectsDic;
    
    public FlyingObjectsController()
    {
        flyingObjectsBuffer = new ObjectBuffer(new GameObject("FlyingObjects").transform);
        activeObjectsDic = new Dictionary<GameObject, List<GameObject>>();
        prefabDic = new Dictionary<Type, GameObject>();
    }
    private GameObject GetPrefab(Type type)
    {
        GameObject prefab;
        if (prefabDic.ContainsKey(type))
        {
            prefab = prefabDic[type];
        }
        else
        {
            string path;
            var attribute = type.GetCustomAttribute<PrefabFilePathAttribute>();
            if (attribute != null)
            {
                path = attribute.Path;
            }
            else
                path = "Prefabs/" + type.Name;

            prefab = ResourceManager.Instance.Load<GameObject>(path);

            prefabDic.Add(type, prefab);
        }

        return prefab;
    }
    public T AddFlying<T>(Vector3 pos, System.Action<T> callback = null) where T : FlyingObject
    {
        Type t = typeof(T);
        GameObject prefab = GetPrefab(t);
        if(prefab != null)
        {
            GameObject obj = flyingObjectsBuffer.Get(prefab);
            T component = obj.GetComponent<T>();
            obj.transform.position = pos;
            callback?.Invoke(component);

            if (!activeObjectsDic.ContainsKey(prefab))
                activeObjectsDic.Add(prefab, new List<GameObject>());
            activeObjectsDic[prefab].Add(obj);

            return component;
        }
        else
        {
            throw new NullReferenceException($"Cannot found prefab of type {t}");
        }
    }
    public void RemoveFlying<T>(GameObject instance)
    {
        GameObject prefab = GetPrefab(typeof(T));
        flyingObjectsBuffer.Put(prefab, instance);
        activeObjectsDic[prefab].Remove(instance);
    }
    public void Clear()
    {
        flyingObjectsBuffer.Clear();
        foreach(var list in activeObjectsDic.Values)
        {
            foreach(var obj in list)
                GameObject.Destroy(obj);
            list.Clear();
        }
    }
}

public class PrefabFilePathAttribute : Attribute
{
    /// <summary>
    /// The relative path under resources folder.
    /// </summary>
    public string Path { get; set; }
}