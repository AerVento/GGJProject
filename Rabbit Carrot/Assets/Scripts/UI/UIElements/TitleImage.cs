using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleImage : MonoBehaviour
{
    public void Disappear()
    {
        GetComponentInChildren<Animator>().SetTrigger("Quit");
    }
    private void RealStartGame()
    {
        UIManager.Instance.HidePanel("TitlePanel");
        GameController.Instance.GameStart();
    }
}
