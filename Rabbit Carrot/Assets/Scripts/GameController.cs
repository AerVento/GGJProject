using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    private PlayerController playerController;
    public PlayerController PlayerController { get => playerController; }

    private CarrotsController carrotsController = new CarrotsController();
    public CarrotsController CarrotsController { get => carrotsController; }

    private FlyingObjectsController flyingObjectsController = new FlyingObjectsController();
    public FlyingObjectsController FlyingObjectsController { get => flyingObjectsController; }
    
    /// <summary>
    /// Whether the game is running.
    /// </summary>
    public bool IsPlaying { get; private set; }

    private void Start()
    {
        
    }

    public void GameStart()
    {
        IsPlaying = true;
    }

   public void GameEnd(bool result)
    {
        IsPlaying= false;
    }
}
