using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField]
    private TextAsset mapData;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject carrotPrefab;
    [SerializeField]
    private int scoreToWin;
    
    private PlayerController playerController;
    public PlayerController PlayerController { get => playerController; }

    private CarrotsController carrotsController;
    public CarrotsController CarrotsController { get => carrotsController; }

    private FlyingObjectsController flyingObjectsController;
    public FlyingObjectsController FlyingObjectsController { get => flyingObjectsController; }

    private MapController mapController;
    public MapController MapController { get => mapController; }

    private CarrotsRefresher refresher;

    [Header("¸úËæÍæ¼ÒµÄÐéÄâÉãÏñ»ú")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// Whether the game is running.
    /// </summary>
    public bool IsPlaying { get; private set; }

    private void Start()
    {
        
    }
    private void Update()
    {
        if(IsPlaying && playerController.Score >= scoreToWin)
        {
            GameEnd(true);
        }
    }

    public void StartGenerateCarrots() => refresher.StartRefreshing();


    public void GameStart()
    {
        void InitializeCamera()
        {
            virtualCamera.Follow = playerController.Player.transform;
            GameObject border = new GameObject("MapBorderCollider");
            PolygonCollider2D collider = border.AddComponent<PolygonCollider2D>();

            Vector3 gridSize = MapController.Grid.cellSize;
            Vector2 size = MapController.MapWorldRect.size;
            Vector2 origin = mapController.Grid.transform.position;
            Vector2[] points = new Vector2[]
            {
                new Vector2(origin.x - size.x/2  - gridSize.x/2, origin.y - size.y/2), //left bottom corner 
                new Vector2(origin.x - size.x/2 - gridSize.x/2 , origin.y + size.y/2 + gridSize.y),//left up corner
                new Vector2(origin.x + size.x/2 + gridSize.x/2, origin.y + size.y/2 + gridSize.y),//right up corner
                new Vector2(origin.x + size.x/2 + gridSize.x/2 , origin.y - size.y/2), //right bottom corner 
            };
            collider.SetPath(0, points);
            virtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = collider;
        }


        IsPlaying = true;
        playerController = new PlayerController(playerPrefab);
        carrotsController = new CarrotsController(carrotPrefab);
        flyingObjectsController = new FlyingObjectsController();
        mapController = new MapController();

        mapController.Load(mapData);

        refresher = new RandomCarrotsRefresher(5);

        Main.Instance.Cursor.CursorStatus = CursorController.Status.None;
        
        UIManager.Instance.ShowPanel<InGamePanel>("InGamePanel");
        //InitializeCamera();
    }

   public void GameEnd(bool result)
    {
        IsPlaying = false;
        playerController.Dispose();
        carrotsController.ClearAllCarrots();
        flyingObjectsController.Clear();
        Destroy(mapController.Grid.gameObject);
        refresher.StopRefreshing();

        UIManager.Instance.HidePanel("InGamePanel");

        AudioManager.Instance.StopAllEffectAudio();
        AudioManager.Instance.PlayEffectAudio(result == true ? "win" : "lose");
        UIManager.Instance.ShowPanel<ResultPanel>("ResultPanel",UIManager.UILayer.Mid,(panel) => panel.SetResult(result));

    }
}
