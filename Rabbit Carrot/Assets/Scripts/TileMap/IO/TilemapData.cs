using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tilemap data structure
/// </summary>
public class TilemapData : IEnumerable<TilemapData.BlockData>
{
    private MapController.Layer mapLayer;

    public List<BlockData> Blocks { get; set; } = new List<BlockData>();
    /// <summary>
    /// Initialize a tilemap data with name "NewMap" and Middleground layer.
    /// </summary>
    public TilemapData()
    {
        this.mapLayer = MapController.Layer.Middleground;
    }
    /// <summary>
    /// Initialize a tilemap data with map name and a layer.
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="mapLayer"></param>
    public TilemapData(MapController.Layer mapLayer)
    {
        this.mapLayer = mapLayer;
    }
    /// <summary>
    /// Which layer the tilemap belong to.
    /// </summary>
    public MapController.Layer MapLayer { get => mapLayer; }
    public bool Contains(Vector3Int pos)
    {
        foreach (var block in Blocks)
        {
            if (block.Position == pos)
                return true;
        }
        return false;
    }

    public IEnumerator<BlockData> GetEnumerator()
    {
        return ((IEnumerable<BlockData>)Blocks).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Blocks).GetEnumerator();
    }

    public struct BlockData
    {
        public Vector3Int Position;
        public int BlockId;
        public BlockData(Vector3Int position, int blockId)
        {
            Position = position; BlockId = blockId;
        }
    }

}
