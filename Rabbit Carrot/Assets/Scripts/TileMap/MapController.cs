using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for displaying tiles.
/// </summary>
public class MapController
{
    /// <summary>
    /// Tilemap Layer
    /// </summary>
    public enum Layer
    {
        /// <summary>
        /// Tilemap layer for sky background
        /// </summary>
        Background,
        /// <summary>
        /// Tilemap layer for solid blocks.
        /// </summary>
        Middleground,
        /// <summary>
        /// Tilemap layer for non-solid blocks.
        /// </summary>
        Foreground,
    }
    /// <summary>
    /// For sky blocks.
    /// </summary>
    private Tilemap background;
    /// <summary>
    /// For solid blocks.
    /// </summary>
    private Tilemap middleground;
    /// <summary>
    /// For non-solid blocks.
    /// </summary>
    private Tilemap foreground;

    private Grid grid;
    /// <summary>
    /// Grid of all tilemap.
    /// </summary>
    public Grid Grid
    {
        get => grid;
    }
    /// <summary>
    /// The map data.
    /// </summary>
    private MapData data;
    
    /// <summary>
    /// All solid blocks of the map.
    /// </summary>
    public TilemapData.BlockData[] SolidBlocks
    {
        get
        {
            return data[Layer.Middleground].Blocks.ToArray();
        }
    }

    /// <summary>
    /// The rect of map area in grid position.
    /// </summary>
    public RectInt MapGridRect
    {
        get
        {
            return data.Rect;
        }
    }
    /// <summary>
    /// The rect of map area in world position.
    /// </summary>
    public Rect MapWorldRect
    {
        get
        {
            ////Use map data as the rect of world area.
            //Vector3 max = TransformGridPosToWorldPos(new Vector3Int(data.Rect.max.x, data.Rect.max.y));
            //Vector3 min = TransformGridPosToWorldPos(new Vector3Int(data.Rect.min.x, data.Rect.min.y));
            //return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

            //Use the view port of main camera as the rect of world area.
            Rect rect = new Rect();
            Vector3 min = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
            Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            rect.min = min;
            rect.max = max;
            return rect;
        }
    }


    /// <summary>
    /// Initialize the map.
    /// </summary>
    public MapController()
    {
        GameObject prefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Tilemaps");
        GameObject maps = GameObject.Instantiate(prefab);
        grid = maps.GetComponent<Grid>();
        Tilemap[] tilemaps = maps.GetComponentsInChildren<Tilemap>();
        for (int i = 0; i < tilemaps.Length; i++)
        {
            switch (tilemaps[i].gameObject.name)
            {
                case "Background":
                    background = tilemaps[i]; break;
                case "Middleground":
                    middleground = tilemaps[i]; break;
                case "Foreground":
                    foreground = tilemaps[i]; break;
            }
        }
    }
    public void Load(string mapFilePath)
    {
        XmlMapHandler handler = new XmlMapHandler();
        data = handler.Load(mapFilePath);
        foreach (var pair in data)//不能直接覆盖，因为在下面修改时还会在调用一遍修改函数
        {
            Tilemap map;
            switch (pair.layer)
            {
                case Layer.Background:
                    map = background;
                    break;
                case Layer.Middleground:
                    map = middleground;
                    break;
                case Layer.Foreground:
                    map = foreground;
                    break;
                default: return;
            }
            foreach (var block in pair.mapData)
            {
                map.SetTile(block.Position, BlockTileSO.Instance.GetTile(block.BlockId));
            }
        }
    }
    public Vector3 TransformGridPosToWorldPos(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }
    public Vector3Int TransformWorldPosToGridPos(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }
}
