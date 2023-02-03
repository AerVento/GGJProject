using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.IsPlaying)
            Debug.Log("Score:" + GameController.Instance.PlayerController.Score);
    }
}
