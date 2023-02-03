using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A bounce pad can turn every source team bullet to target team bullet.
/// </summary>
public interface IBouncePad
{
    /// <summary>
    /// The source team of the transation.
    /// </summary>
    public E_Team SourceTeam { get; }
    /// <summary>
    /// The target team of transation.
    /// </summary>
    public E_Team TargetTeam { get; }
    /// <summary>
    /// Get bounced bullets direction.
    /// </summary>
    /// <param name="bulletDirection"></param>
    /// <param name="bulletPosition"></param>
    /// <returns></returns>
    public Vector3 GetBouncedDirection(Vector3 bulletDirection, Vector3 bulletPosition);
}
