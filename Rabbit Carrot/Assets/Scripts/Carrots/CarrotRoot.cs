using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarrotRoot : MonoBehaviour
{
    public abstract Collider2D RootCollider { get; }
    public abstract int RootLength { get;set; }
    public abstract MonoBehaviour Master { get; }
}