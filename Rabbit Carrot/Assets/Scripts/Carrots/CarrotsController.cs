using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotsController
{
    private GameObject prefab;
    private ObjectBuffer carrotBuffer;
    private List<CarrotBehaviour> activeCarrots;



    public CarrotsController(GameObject carrotPrefab)
    {
        prefab = carrotPrefab;
        carrotBuffer = new ObjectBuffer(new GameObject("Carrots").transform);
        activeCarrots = new List<CarrotBehaviour>();
    }
    public CarrotBehaviour AddCarrot(Vector3 pos)
    {
        CarrotBehaviour instance = carrotBuffer.Get(prefab).GetComponent<CarrotBehaviour>();
        instance.transform.position = pos;
        activeCarrots.Add(instance);
        return instance;
    }
    public void RemoveCarrot(CarrotBehaviour carrot)
    {
        activeCarrots.Remove(carrot);
        carrotBuffer.Put(prefab, carrot.gameObject);
    }
    public void ClearAllCarrots()
    {
        foreach(var obj in activeCarrots)
        {
            GameObject.Destroy(obj.gameObject);
        }
        activeCarrots.Clear();
        carrotBuffer.Clear();

    }

}
