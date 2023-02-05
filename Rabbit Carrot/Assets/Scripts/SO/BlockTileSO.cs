using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO database of all block tiles.
/// </summary>
[CreateAssetMenu(fileName = "BlockTileSO", menuName = "SO/BlockTileData")]
[SOFilePath(Path = "Tile Palette/BlockTileSO")]
public class BlockTileSO : SOSingleton<BlockTileSO>
{
    public List<BlockTile> Tiles = new List<BlockTile>();
    public BlockTile GetTile(int blockId) => Tiles.Find((block) => block.BlockId == blockId);
}