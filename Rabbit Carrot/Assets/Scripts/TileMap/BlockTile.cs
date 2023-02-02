using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A base class for all tiles in tile map.
/// </summary>
public class BlockTile : Tile
{
    [SerializeField]
    private int blockId;
    public int BlockId { get => blockId; set => blockId = value; }
    [SerializeField]
    private string blockName;
    public string BlockName { get => blockName; set => blockName = value; }
    [SerializeField]
    private bool isSolid;
    public bool IsSolid { get =>isSolid; set =>isSolid = value; }
}
