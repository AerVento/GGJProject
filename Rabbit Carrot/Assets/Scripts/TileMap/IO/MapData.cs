using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Map data for all layer tilemaps.
/// </summary>
public class MapData :IEnumerable<MapData.Pair>
{
    private static int enumLength = -1;
    private static int EnumLength
    {
        get
        {
            if (enumLength == -1)
                enumLength = System.Enum.GetValues(typeof(MapController.Layer)).Length;
            return enumLength;
        }
    }
    
    private TilemapData[] data;
    public MapData()
    {
        data = new TilemapData[EnumLength];
    }
    /// <summary>
    /// The name of the map.
    /// </summary>
    public string MapName { get; set; }
    public TilemapData this[MapController.Layer layer]
    {
        get
        {
            return data[(int)layer];
        }
        set
        {
            data[(int)layer] = value;
        }
    }
    public struct Pair
    {
        public MapController.Layer layer;
        public TilemapData mapData;

        public Pair(MapController.Layer layer, TilemapData map)
        {
            this.layer = layer;
            this.mapData = map;
        }
    }

    public IEnumerator<Pair> GetEnumerator()
    {
        for(int i = 0; i < data.Length; i++)
        {
            yield return new Pair((MapController.Layer)i, data[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
