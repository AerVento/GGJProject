using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBouncePad : MonoBehaviour, IBouncePad
{
    public E_Team SourceTeam => E_Team.Carrots;

    public E_Team TargetTeam => E_Team.Player;

    public Vector3 GetBouncedDirection(Vector3 bulletDirection, Vector3 bulletPosition)
    {
        return bulletPosition - GameController.Instance.PlayerController.Player.PlayerPosition;
    }
}
