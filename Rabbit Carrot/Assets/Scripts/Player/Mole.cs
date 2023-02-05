using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField]
    private Animator moleBody;
    /// <summary>
    /// Set direction to -1 to active move left animation. Set direction to 1 to active move right animation.
    /// Set direction to 0 to stop animation.
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(int direction)
    {
        moleBody.SetInteger("Direction", direction);
    }
}
