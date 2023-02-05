using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotsBody : MonoBehaviour
{
    private Action bulletCreating;
    public void Shoot(Action bulletCreating)
    {
        GetComponent<Animator>().SetTrigger("Shoot");
        this.bulletCreating = bulletCreating;
    }
    private void RealShoot()
    {
        bulletCreating?.Invoke();
    }
}
