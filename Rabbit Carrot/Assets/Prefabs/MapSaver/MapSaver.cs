using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A component attached to the prefab to save and load the map data.
/// </summary>
public class MapSaver : MonoBehaviour
{
    [ContextMenu("Clear all tiles")]
    public void Clear()
    {
        GetComponent<Tilemap>().ClearAllTiles();
    }
}
