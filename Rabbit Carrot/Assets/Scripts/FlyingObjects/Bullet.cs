using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for all bullets
/// </summary>
public class Bullet : FlyingObject
{
    [SerializeField]
    private float bulletSpeed;
    public float Speed { get => bulletSpeed; set { bulletSpeed = value; } }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer
    {
        get
        {
            if(spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return spriteRenderer;
        }
    }

    private E_Team team;
    public E_Team Team
    {
        get => team;
        set
        {
            //set the image
            switch (value)
            {
                case E_Team.Player:
                    SpriteRenderer.color = Color.white; break;
                case E_Team.Carrots:
                    SpriteRenderer.color = Color.red; break;
            }
            team = value;
        }
    }
    private void Update()
    {
        transform.position += bulletSpeed * Time.deltaTime * transform.right;
    }
}
