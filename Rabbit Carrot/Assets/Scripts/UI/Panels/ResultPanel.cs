using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : BasePanel
{
    [SerializeField]
    private Sprite winLabelSprite;
    [SerializeField]
    private Sprite loseLabelSprite;
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
    public void SetResult(bool result)
    {
        GetControl<Image>("ResultImg").sprite = result == true? winLabelSprite : loseLabelSprite;
    }
}
