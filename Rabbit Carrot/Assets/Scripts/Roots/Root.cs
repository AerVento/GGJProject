using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Root : MonoBehaviour
{
    /// <summary>
    /// The collider of the root;
    /// </summary>
    public abstract Collider2D RootCollider { get; }
    /// <summary>
    /// The source position of a root;
    /// </summary>
    public abstract Vector3 Origin { get; set; }
    /// <summary>
    /// The bottom position of a root.
    /// </summary>
    public abstract Vector3 Bottom { get; set; }
    /// <summary>
    /// The length of the root
    /// </summary>
    public abstract float RootLength { get; set; }
    /// <summary>
    /// The angle of the root.
    /// </summary>
    public abstract float Angle { get; set; }
    /// <summary>
    /// The team belong to.
    /// </summary>
    public abstract E_Team Team { get; }

    /// <summary>
    /// Destroy the root.
    /// </summary>
    public abstract void DestroyRoot();
}
