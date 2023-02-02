using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Instance class of handler of xml map files.
/// </summary>
public class XmlMapHandler : IMapHandler
{
    public MapData Load(string filePath)
    {
        MapData data = new MapData();

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        XmlNode root = doc.SelectSingleNode("map");

        XmlNode rectNode = root.SelectSingleNode("rect");
        RectInt rect = new RectInt();
        rect.xMin = Convert.ToInt32(rectNode.Attributes["xMin"].Value);
        rect.xMax = Convert.ToInt32(rectNode.Attributes["xMax"].Value);
        rect.yMin = Convert.ToInt32(rectNode.Attributes["yMin"].Value);
        rect.yMax = Convert.ToInt32(rectNode.Attributes["yMax"].Value);
        data.Rect = rect;

        Type enumType = typeof(MapController.Layer);
        string[] arr = Enum.GetNames(enumType);
        foreach(var layerString in arr)
        {
            XmlNode layer = root.SelectSingleNode(layerString);

            MapController.Layer layerEnum = (MapController.Layer)Enum.Parse(enumType, layerString);
            
            TilemapData tilemapData = new TilemapData(layerEnum);

            XmlNodeList blocks = layer.SelectNodes("block");
            for (int i = 0; i < blocks.Count; i++)
            {
                XmlNode position = blocks[i].SelectSingleNode("position");
                int x = Convert.ToInt32(position.Attributes["x"].Value);
                int y = Convert.ToInt32(position.Attributes["y"].Value);
                int z = Convert.ToInt32(position.Attributes["z"].Value);
                Vector3Int pos = new Vector3Int(x, y, z);
                int blockId = Convert.ToInt32(blocks[i].Attributes["id"].Value);

                tilemapData.Blocks.Add(new TilemapData.BlockData(pos, blockId));
            }

            data[layerEnum] = tilemapData;
        }
        return data;
    }

    public void Save(string filePath, MapData mapData)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
        using (FileStream file = File.OpenWrite(filePath))
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "");
            xml.AppendChild(declaration);
            XmlElement root = xml.CreateElement("map");
            xml.AppendChild(root);

            XmlElement rectNode = xml.CreateElement("rect");
            RectInt rect = mapData.Rect;
            rectNode.SetAttribute("xMin", rect.xMin.ToString());
            rectNode.SetAttribute("xMax",rect.xMax.ToString());
            rectNode.SetAttribute("yMin",rect.yMin.ToString());
            rectNode.SetAttribute("yMax",rect.yMax.ToString());
            root.AppendChild(rectNode);

            foreach(var pair in mapData)
            {
                XmlElement layer = xml.CreateElement(pair.layer.ToString());
                root.AppendChild(layer);
                foreach (var blockData in pair.mapData)
                {
                    XmlElement block = xml.CreateElement("block");
                    layer.AppendChild(block);
                    block.SetAttribute("id", blockData.BlockId.ToString());

                    XmlElement position = xml.CreateElement("position");
                    position.SetAttribute("x", blockData.Position.x.ToString());
                    position.SetAttribute("y", blockData.Position.y.ToString());
                    position.SetAttribute("z", blockData.Position.z.ToString());

                    block.AppendChild(position);
                }
            }
            xml.Save(file);
        }
    }
}
