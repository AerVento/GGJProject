using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
        
        background = new GameObject("Background").AddComponent<Tilemap>();
        background.transform.SetParent(obj.transform);
        
        middleground = new GameObject("Middleground").AddComponent<Tilemap>();
        middleground.transform.SetParent(obj.transform);
        
        foreground = new GameObject("Foreground").AddComponent<Tilemap>();
        foreground.transform.SetParent(obj.transform);
    }

    public void Load(string mapFilePath)
    {

    }
}
