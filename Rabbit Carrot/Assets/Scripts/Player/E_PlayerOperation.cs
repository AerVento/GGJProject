using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PlayerOperation
{
    /// <summary>
    /// The player did not do any operation.
    /// </summary>
    None,
    /// <summary>
    /// The player is climbing up normally.
    /// </summary>
    ClimbUp,
    /// <summary>
    /// The player is climbing down normally.
    /// </summary>
    ClimbDown,
    /// <summary>
    /// The player is climbing up quickly.
    /// </summary>
    ClimbUpQuick,
    /// <summary>
    /// The player is climbing down quickly.
    /// </summary>
    ClimbDownQuick,
    /// <summary>
    /// Moving the mole to left.
    /// </summary>
    MoleMoveLeft,
    /// <summary>
    /// Moving the mole to right.
    /// </summary>
    MoleMoveRight,
}
