using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    void Awake()
    {
        MapController.Instance.Load("Assets/XmlTileMapData/TestMap.xml");
        GameController.Instance.GameStart();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(bullet);

            Vector3 rotate = obj.transform.eulerAngles;
            rotate.z = Random.Range(0, 360);
            obj.transform.eulerAngles = rotate;

            obj.GetComponent<Bullet>().Speed = 2f;
        }
    }
}
