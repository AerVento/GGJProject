using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A base class for all tiles in tile map.
/// </summary>
public class BlockTile : Tile
{
    public int BlockId { get; set; }
    public string BlockName { get; set; }
    public bool IsSolid { get; set; }
}
