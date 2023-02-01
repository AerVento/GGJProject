using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private PlayerData data = new PlayerData();
    public int Score { get => data.Score; }
    
    private GameObject playerPrefab;
    private PlayerBehaviour playerInstance;
    public PlayerBehaviour Player
    {
        get
        {
            if (playerInstance == null)
            {
                playerInstance = GameObject.Instantiate(playerPrefab).GetComponent<PlayerBehaviour>();
            }
            return playerInstance;
        }
    }

    public PlayerController(GameObject playerPrefab)
    {
        this.playerPrefab = playerPrefab;
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
    /// Reset the player.
    /// </summary>
    public void Reset()
    {

    }
}
