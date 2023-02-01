using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for displaying tiles.
/// </summary>
public class MapController : Singleton<MapController>
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
    /// <summary>
    /// Grid of all tilemap.
    /// </summary>
    private Grid grid;

    /// <summary>
    /// Initialize the map.
    /// </summary>
    public MapController()
    {
        GameObject obj = new GameObject("Tilemaps");
        grid = obj.AddComponent<Grid>();

        GameObject back = new GameObject("Background");
        background = back.AddComponent<Tilemap>();
        back.AddComponent<TilemapRenderer>();
        background.transform.SetParent(obj.transform);

        GameObject middle = new GameObject("Middleground");
        middleground = middle.AddComponent<Tilemap>();
        middle.AddComponent<TilemapRenderer>();
        middle.AddComponent<TilemapCollider2D>(); //Add collider for blocks on middle ground
        middleground.transform.SetParent(obj.transform);

        GameObject fore = new GameObject("Foreground");
        foreground = fore.AddComponent<Tilemap>();
        fore.AddComponent<TilemapRenderer>();
        foreground.transform.SetParent(obj.transform);
    }
    public void Load(string mapFilePath)
    {
        XmlMapHandler handler = new XmlMapHandler();
        foreach (var pair in handler.Load(mapFilePath))//不能直接覆盖，因为在下面修改时还会在调用一遍修改函数
        {
            Tilemap map;
            switch (pair.layer)
            {
                case MapController.Layer.Background:
                    map = background;
                    break;
                case MapController.Layer.Middleground:
                    map = middleground;
                    break;
                case MapController.Layer.Foreground:
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
}
