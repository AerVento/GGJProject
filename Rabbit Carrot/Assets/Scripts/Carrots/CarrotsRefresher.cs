using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Coroutine refreshCoroutine;
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
    private IEnumerator RefreshCoroutine()
    {
        float generateSpaceTime = 4;
        float minRootLength = 1;
        float maxRootLength = 3;
        while (IsStarted)
        {
            int index = Random.Range(0, Blocks.Length);
            Vector3 worldPos = GameController.Instance.MapController.TransformGridPosToWorldPos(Blocks[index].Position);
            worldPos += Vector3.right * GameController.Instance.MapController.Grid.cellSize.x / 2;

            CarrotBehaviour carrot = GameController.Instance.CarrotsController.AddCarrot(worldPos);
            carrot.Angle = 270;
            carrot.GrowAndStart(Random.Range(minRootLength, maxRootLength));
            yield return new WaitForSeconds(generateSpaceTime);
        }
        refreshCoroutine = null;
    }
    protected override void RealStartRefresh()
    {
        if(refreshCoroutine == null)
            refreshCoroutine = MonoManager.Instance.StartCoroutine(RefreshCoroutine());
    }

    protected override void RealStopRefresh()
    {
        if (refreshCoroutine != null)
            MonoManager.Instance.StopCoroutine(refreshCoroutine);
    }
}
