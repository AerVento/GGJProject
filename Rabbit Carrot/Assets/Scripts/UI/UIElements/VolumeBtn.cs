using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBtn : MonoBehaviour
{
    [Header("������ʱ��Sprite")]
    [SerializeField]
    private Sprite openNormal;
    [SerializeField]
    private SpriteState openState;

    [Header("�����ر�ʱ��Sprite")]
    [SerializeField]
    private Sprite closeNormal;
    [SerializeField]
    private SpriteState closeState;

    private Button btn;
    public Button Button
    {
        get
        {
            if(btn == null)
            {
                btn = GetComponent<Button>();
            }
            return btn;
        }
    }
    private Image img;
    public Image Image
    {
        get
        {
            if (img == null)
            {
                img = GetComponent<Image>();
            }
            return img;
        }
    }
    public void SetStatus(bool status)
    {
        if(status == true) 
        {
            Image.sprite = openNormal;

            Button.spriteState = openState;
        }
        else
        {
            Image.sprite = closeNormal;

            Button.spriteState = closeState;
        }
    }
}
