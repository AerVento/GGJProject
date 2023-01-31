using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for displaying tiles.
/// </summary>
public class TileController : Singleton<TileController>
{
    private Dictionary<string,Tilemap> tilemaps = new Dictionary<string,Tilemap>();

}
