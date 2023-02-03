using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private PlayerData data = new PlayerData();
    public int Score { get => data.Score; }

    private PlayerBehaviour playerInstance;
    /// <summary>
    /// The behaviour component of player.
    /// </summary>
    public PlayerBehaviour Player
    {
        get
        {
            return playerInstance;
        }
    }
    public PlayerController(GameObject playerPrefab)
    {
        playerInstance = GameObject.Instantiate(playerPrefab).GetComponent<PlayerBehaviour>();
    }

    /// <summary>
    /// Add score.
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(int value)
    {
        data.Score += value;
    }
    /// <summary>
    /// Remove score.
    /// </summary>
    /// <param name="value"></param>
    public void RemoveScore(int value)
    {
        data.Score -= value;
    }
    /// <summary>
    /// Dispose the player.
    /// </summary>
    public void Dispose()
    {
        GameObject.Destroy(playerInstance.Mole.gameObject);
        GameObject.Destroy(playerInstance.gameObject);
    }
}
