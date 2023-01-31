using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO database of all block tiles.
/// </summary>
[CreateAssetMenu(fileName = "BlockTileSO", menuName = "SO/BlockTileData")]
public class BlockTileSO : ScriptableObject
{
    private static BlockTileSO instance;
    /// <summary>
    /// BlockTileSO Instance
    /// </summary>
    public static BlockTileSO Instance
    {
        get
        {
            if(instance == null)
            {
                instance = ResourceManager.Instance.Load<BlockTileSO>("SO/BlockTileSO");
            }
            return instance;
        }
    }
    public List<BlockTile> Tiles = new List<BlockTile>();
    public BlockTile GetTile(int blockId) => Tiles.Find((block) => block.BlockId == blockId);
}
