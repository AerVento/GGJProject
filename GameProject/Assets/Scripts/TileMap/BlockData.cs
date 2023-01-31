using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all block data.
/// </summary>
public abstract class BlockData
{
    /// <summary>
    /// Id of Block.
    /// </summary>
    public abstract int BlockId { get; }
    /// <summary>
    /// Name of block.
    /// </summary>
    public abstract string BlockName { get; }
    /// <summary>
    /// Determines if the block can be passed through.
    /// </summary>
    public abstract bool IsSolid { get; }
}
