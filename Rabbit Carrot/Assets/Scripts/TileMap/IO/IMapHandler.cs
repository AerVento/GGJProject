using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for handlers of map file.
/// </summary>
public interface IMapHandler
{
    /// <summary>
    /// Load a map data from path.
    /// </summary>
    /// <param name="filePath">Map data path</param>
    /// <returns>Map data</returns>
    public MapData Load(string filePath);
    /// <summary>
    /// Save a map data to path..
    /// </summary>
    /// <param name="filePath">Save path</param>
    /// <param name="mapData">Saved map data</param>
    public void Save(string filePath, MapData mapData);
}
