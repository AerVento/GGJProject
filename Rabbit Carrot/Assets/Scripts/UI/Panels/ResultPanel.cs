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
        GetControl<Button>("PlayAgainBtn").onClick.AddListener(PlayAgain);
        GetControl<Button>("QuitBtn").onClick.AddListener(Quit);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("PlayAgainBtn").onClick.RemoveListener(PlayAgain);
        GetControl<Button>("QuitBtn").onClick.RemoveListener(Quit);
    }
    void PlayAgain()
    {
        Hide();
        GameController.Instance.GameStart();
    }
    private void Quit()
    {
        Application.Quit();
    }
    public void SetResult(bool result)
    {
        GetControl<Image>("ResultImg").sprite = result == true? winLabelSprite : loseLabelSprite;
    }
}
