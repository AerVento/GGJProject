using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRoot : Root
{
    private BoxCollider2D boxCollider;

    private float angle;
    private float length;

    [SerializeField]
    private float initalWidth;
    [SerializeField]
    private float initialLength;
    [SerializeField]
    private float initialAngle;
    [SerializeField]
    private Vector3 initialPosition;

    public override Collider2D RootCollider => boxCollider;

    public override Vector3 Origin
    {
        //根的Sprite锚点就是根的开始位置
        get => transform.position;
        set
        {
            transform.position = value;
        }
    }

    public override Vector3 Bottom
    {
        get
        {
            Vector3 normalized = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            return Origin + normalized * length;
        }
        set
        {
            Vector3 delta = value - Origin;
            length = delta.magnitude;

            if (length != 0)
            {
                Vector3 normalized = delta.normalized;
                float ang = Mathf.Rad2Deg * Mathf.Atan2(normalized.y, normalized.x);
                angle = normalized.y > 0 ? ang : ang + 180;
            }
        }
    }

    public override float RootLength
    {
        get
        {
            return length;
        }
        set
        {
            length = value;

            Vector3 scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }
    }

    public override float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;

            Vector3 euler = transform.eulerAngles;
            euler.z = value;
            transform.eulerAngles = euler;
        }
    }

    public override E_Team Team => E_Team.Player;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        Origin = initialPosition;
        RootLength = initialLength; //Set the default length;
        Angle = initialAngle;
        transform.localScale = new Vector3(initialLength, initalWidth);
    }

    public void GenerateRoot()
    {
        gameObject.SetActive(true);
    }
    public override void DestroyRoot()
    {
        gameObject.SetActive(false);
        GameController.Instance.GameEnd(false);
    }
}
