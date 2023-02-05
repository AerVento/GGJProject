using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

/// <summary>
/// A class to generate the carrots 
/// </summary>
public abstract class CarrotsRefresher
{
    public bool IsStarted { get;private set; }
    protected abstract void RealStartRefresh();
    public void StartRefreshing()
    {
        IsStarted = true;
        RealStartRefresh();

    }
    protected abstract void RealStopRefresh();
    public void StopRefreshing()
    {
        IsStarted = false;
        RealStopRefresh();
    }
}

public class RandomCarrotsRefresher : CarrotsRefresher
{
    private int initialAmount;
    private Coroutine refreshCoroutine;
    private TilemapData.BlockData[] Blocks
    {
        get
        {
            //筛选可以生成的位置
            var array = GameController.Instance.MapController.SolidBlocks;
            var data = array
                .Where((block) =>
                {
                    //在镜头中
                    Vector3 worldPos = GameController.Instance.MapController.TransformGridPosToWorldPos(block.Position);
                    return GameController.Instance.MapController.MapWorldRect.Contains(worldPos);
                })
                .Where((block) =>
                {
                    //下方一格是否已经有方块了
                    return !array.Contains(new TilemapData.BlockData(block.Position + new Vector3Int(0, -1), block.BlockId));
                }).Where((block) =>
                {
                    //此格是否已生成
                    return !activeLocations.ContainsValue(block);
                });

            List<TilemapData.BlockData> list = new List<TilemapData.BlockData>();
            foreach (var block in data)
            {
                list.Add(block);
            }

            return list.ToArray();
        }
    }

    /// <summary>
    /// 已生成的位置
    /// </summary>
    private Dictionary<CarrotBehaviour, TilemapData.BlockData> activeLocations = new Dictionary<CarrotBehaviour, TilemapData.BlockData>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAmount">Carrot's total generate amount before randomly generate</param>
    public RandomCarrotsRefresher(int initialAmount)
    {
        this.initialAmount = initialAmount;
    }
    private void Generate()
    {
        float minRootLength = 1;
        float maxRootLength = 2.5f;
        TilemapData.BlockData[] blocks = Blocks;

        if (blocks.Length == 0)
            return;

        int index = Random.Range(0, blocks.Length);

        TilemapData.BlockData block = blocks[index];

        Vector3 worldPos = GameController.Instance.MapController.TransformGridPosToWorldPos(block.Position);
        worldPos += Vector3.right * GameController.Instance.MapController.Grid.cellSize.x / 2;

        CarrotBehaviour carrot = GameController.Instance.CarrotsController.AddCarrot(worldPos);
        carrot.Angle = 270;
        carrot.GrowAndStart(Random.Range(minRootLength, maxRootLength));


        activeLocations.Add(carrot, block);

        carrot.OnCarrotDestroy += RemoveCarrot;

    }

    private IEnumerator RefreshCoroutine()
    {
        float generateSpaceTime = 4;
        while (IsStarted)
        {
            Generate();
            yield return new WaitForSeconds(generateSpaceTime);
        }
        refreshCoroutine = null;
    }
    private void RemoveCarrot(CarrotBehaviour carrot)
    {
        carrot.OnCarrotDestroy -= RemoveCarrot;
        activeLocations.Remove(carrot);
    }
    protected override void RealStartRefresh()
    {
        if(refreshCoroutine == null)
            refreshCoroutine = MonoManager.Instance.StartCoroutine(RefreshCoroutine());
        for(int i = 0;i < initialAmount; i++)
        {
            Generate();
        }
    }

    protected override void RealStopRefresh()
    {
        if (refreshCoroutine != null)
            MonoManager.Instance.StopCoroutine(refreshCoroutine);
    }
}

public class TestCarrotsRefresher : CarrotsRefresher
{
    private int generateCount;
    private TilemapData.BlockData[] blocks;
    private TilemapData.BlockData[] Blocks
    {
        get
        {
            if (blocks == null)
            {
                //筛选可以生成的位置
                var array = GameController.Instance.MapController.SolidBlocks;
                var data = array.Where<TilemapData.BlockData>((block) =>
                {
                    Vector3 worldPos = GameController.Instance.MapController.TransformGridPosToWorldPos(block.Position);
                    bool isInViewRect = GameController.Instance.MapController.MapWorldRect.Contains(worldPos);
                    //下方一格是否已经有方块了
                    bool isOccupied = array.Contains(new TilemapData.BlockData(block.Position + new Vector3Int(0, -1), block.BlockId));
                    return isInViewRect && !isOccupied;
                });
                List<TilemapData.BlockData> list = new List<TilemapData.BlockData>();
                foreach (var block in data)
                {
                    list.Add(block);
                }
                blocks = list.ToArray();
            }
            return blocks;
        }
    }
    public TestCarrotsRefresher(int generateTotalCount)
    {
        this.generateCount= generateTotalCount;
    }
    private void Generate()
    {
        float minRootLength = 1;
        float maxRootLength = 2.5f;
        for(int i = 0; i < generateCount; i++)
        {
            int index = Random.Range(0, Blocks.Length);
            Vector3 worldPos = GameController.Instance.MapController.TransformGridPosToWorldPos(Blocks[index].Position);
            worldPos += Vector3.right * GameController.Instance.MapController.Grid.cellSize.x / 2;

            CarrotBehaviour carrot = GameController.Instance.CarrotsController.AddCarrot(worldPos);
            carrot.Angle = 270;
            carrot.GrowAndStart(Random.Range(minRootLength, maxRootLength));
        }
    }
    protected override void RealStartRefresh()
    {
        Generate();
    }

    protected override void RealStopRefresh()
    {
        
    }
}