using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    /// <summary>
    /// Whether the sound volume is open or closed.
    /// </summary>
    private bool nowVolumeStatus = true;

    // Start is called before the first frame update
    protected override void BeforeShow()
    {
        AudioManager.Instance.PlayBackgroundMusic("InGame");
        GetControl<Button>("VolumeBtn").onClick.AddListener(SetVolume);
        GetControl<Button>("HelpBtn").onClick.AddListener(ShowTip);
    }
    protected override void BeforeHide()
    {
        AudioManager.Instance.StopBackgroundAudio();
        GetControl<Button>("VolumeBtn").onClick.RemoveListener(SetVolume);
        GetControl<Button>("HelpBtn").onClick.RemoveListener(ShowTip);
    }
    private void ShowTip()
    {
        GameObject obj = GetControl<Image>("Tip").gameObject;
        obj.SetActive(!obj.activeSelf);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void SetVolume()
    {
        nowVolumeStatus = !nowVolumeStatus;
        
        GetControl<Button>("VolumeBtn").GetComponent<VolumeBtn>().SetStatus(nowVolumeStatus);

        int volume = nowVolumeStatus == true ? 1:0;
        AudioManager.Instance.MusicVolume = volume;
        AudioManager.Instance.EffectVolume = volume;
        
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void SetScore(int score)
    {
        GetControl<TextMeshProUGUI>("ScoreText").text = score.ToString();
    }
    protected override void Awake()
    {
        base.Awake();
        Button volumeBtn = GetControl<Button>("VolumeBtn");
        volumeBtn.GetComponent<VolumeBtn>().SetStatus(nowVolumeStatus);
    }
    private void Start()
    {
        GetControl<Image>("Tip").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsPlaying)
        {
            SetScore(GameController.Instance.PlayerController.Score);
        }
    }
}
