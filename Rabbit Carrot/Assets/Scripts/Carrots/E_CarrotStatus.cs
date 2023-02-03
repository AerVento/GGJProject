using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CarrotStatus
{
    /// <summary>
    /// when the carrot was growing
    /// </summary>
    Grow,
    /// <summary>
    /// when carrots was finding player
    /// </summary>
    Guard,
    /// <summary>
    /// when carrots found player and prepare to shoot.
    /// </summary>
    Lock,
    /// <summary>
    /// when carrots is reloading the bullet
    /// </summary>
    Reload,
    /// <summary>
    /// when carrots shoot
    /// </summary>
    Shoot,
    /// <summary>
    /// when carrots was dead and wait for be destroyed.
    /// </summary>
    Die,
}
