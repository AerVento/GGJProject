using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void BeforeShow()
    {
        GetControl<Button>("StartBtn").onClick.AddListener(StartGameBtn);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("StartBtn").onClick.RemoveListener(StartGameBtn);
    }
    private void StartGameBtn()
    {
        Hide();
        GameController.Instance.GameStart();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
