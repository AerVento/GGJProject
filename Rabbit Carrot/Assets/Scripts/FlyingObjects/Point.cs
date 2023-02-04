using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : FlyingObject
{
    public int pointValue;
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    private void Update()
    {
        if (!GameController.Instance.MapController.MapWorldRect.Contains(transform.position))
            DestroySelf();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mole mole = collision.GetComponent<Mole>();
        if (mole != null)
        {
            GameController.Instance.PlayerController.AddScore(pointValue);
            AudioManager.Instance.PlayEffectAudio("point");
            DestroySelf();
        }
    }
    private void DestroySelf()
    {
        GameController.Instance.FlyingObjectsController.RemoveFlying<Point>(gameObject);
    }
}
