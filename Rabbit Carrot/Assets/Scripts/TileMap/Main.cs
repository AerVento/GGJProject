using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main : MonoSingleton<Main>
{
    public Texture2D normalCursor;
    public Texture2D targetingCursor;

    private CursorController cursor;
    public CursorController Cursor => cursor;
    void Start()
    {
        cursor = new CursorController(normalCursor,targetingCursor);

        AudioManager.Instance.MusicVolume = 1;
        AudioManager.Instance.EffectVolume = 1;

        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
