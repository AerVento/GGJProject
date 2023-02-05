using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarrotRoot : Root
{
    [SerializeField]
    private CarrotBehaviour carrot;

    private float length;
    private float angle;
    private E_RootDirection direction;
    private BoxCollider2D boxCollider;

    public event UnityAction OnRootDestroyed;

    /// <summary>
    /// The direction of root
    /// </summary>
    public E_RootDirection Direction
    {
        get => direction;
        set
        {
            switch (value)
            {
                case E_RootDirection.Up:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case E_RootDirection.Down:
                    transform.eulerAngles = new Vector3(0, 0, 270);
                    break;
                case E_RootDirection.Left:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case E_RootDirection.Right:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
            direction = value;
        }
    }

    public override Collider2D RootCollider => boxCollider;

    public override Vector3 Origin
    {
        //����Spriteê����Ǹ��Ŀ�ʼλ��
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

            if(length != 0)
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

            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            float pixelSize = sprite.pixelsPerUnit;
            float spriteLength = sprite.texture.width / pixelSize;
            Vector3 scale = transform.localScale;
            scale.x = value / spriteLength;
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

    public override E_Team Team => E_Team.Carrots;
    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public override void DestroyRoot()
    {
        OnRootDestroyed?.Invoke();
    }
}
