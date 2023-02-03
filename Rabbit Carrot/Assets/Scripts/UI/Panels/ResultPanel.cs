using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : BasePanel
{
    public string Label
    {
        get => GetControl<TextMeshProUGUI>("ResultTitle").text;
        set
        {
            GetControl<TextMeshProUGUI>("ResultTitle").text = value;
        }
    }
    protected override void BeforeShow()
    {
        GetControl<Button>("BackToTitleBtn").onClick.AddListener(BackToTitle);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("BackToTitleBtn").onClick.RemoveListener(BackToTitle);
    }
    void BackToTitle()
    {
        Hide();
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
}
