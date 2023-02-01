using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    InputCalculator calculator = new InputCalculator();
    void Start()
    {
        MapController.Instance.Load("Assets/XmlTileMapData/TestMap.xml");
        GameController.Instance.GameStart();
        calculator.ClimbUpKey = (KeyCodeSource)KeyCode.W;
        calculator.ClimbDownKey= (KeyCodeSource)KeyCode.S;
        calculator.MoveLeftKey = (KeyCodeSource)KeyCode.A;
        calculator.MoveRightKey = (KeyCodeSource)KeyCode.D;
        calculator.StartListening();
        
    }
    string str = "";
    // Update is called once per frame
    void Update()
    {
        string debug = "";
        foreach(var operation in calculator.GetOperation())
        {
            debug += operation.ToString();
        }
        //if(debug != str)
        //{
        //    str = debug;
           Debug.Log(debug);
        //}
    }
}
