using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for all bullets
/// </summary>
public class Bullet : FlyingObject
{

    private float bulletSpeed;
    public float Speed
    {
        get => bulletSpeed;
        set
        {
            bulletSpeed = value;
            Rigid.velocity = transform.right * bulletSpeed;
        }
    }

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

    private Rigidbody2D rigid;
    private Rigidbody2D Rigid
    {
        get
        {
            if (rigid == null)
            {
                rigid = GetComponent<Rigidbody2D>();
            }
            return rigid;
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
        if(!GameController.Instance.MapController.MapWorldRect.Contains(transform.position)) 
        { 
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool CheckTerrain()
        {
            if (collision.gameObject.tag == "Terrain") //碰到地形摧毁自己
            {
                DestroySelf();
                return true;
            }
            else
                return false;
        }
        bool CheckRoot()
        {
            Root root = collision.GetComponent<Root>();
            if (root != null && root.Team != Team)
            {
                DestroySelf();
                root.DestroyRoot();
                return true;
            }
            return false;
        }
        bool CheckBouncePad()
        {
            IBouncePad pad = collision.GetComponent<IBouncePad>();
            if(pad != null && pad.SourceTeam == Team)
            {
                Team = pad.TargetTeam;
                pad.Bounce(this);
                return true;
            }
            return false;
        }
        Func<bool>[] operations = new Func<bool>[] { CheckTerrain, CheckRoot, CheckBouncePad };
        foreach(var func in operations)
        {
            if (func.Invoke() == true)
                return;
        }
    }
    private void DestroySelf()
    {
        if(GameController.Instance.IsPlaying)
            GameController.Instance.FlyingObjectsController.RemoveFlying<Bullet>(gameObject);
        else
            Destroy(gameObject);
    }
}
