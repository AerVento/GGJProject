using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A component attached to the prefab to save and load the map data.
/// </summary>
public class MapSaver : MonoBehaviour
{
    public Tilemap background;
    public Tilemap middleground;
    public Tilemap foreground;
}
