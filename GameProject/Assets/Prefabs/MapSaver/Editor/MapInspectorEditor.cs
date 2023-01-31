using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System.Xml;

/// <summary>
/// A inspector editor for map saver.
/// </summary>
[CustomEditor(typeof(MapSaver))]
public class MapInspectorEditor : Editor
{
    private Tilemap current;
    private List<Vector3Int> availables = new List<Vector3Int>();
    private void OnEnable()
    {
        if(current == null)
            current = (target as MapSaver).GetComponent<Tilemap>();

        current.ClearAllTiles();
        availables.Clear();

        typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
        Tilemap.tilemapTileChanged += TileChangeListener;
    }
    private void OnDestroy()
    {
        Tilemap.tilemapTileChanged -= TileChangeListener;
        typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
    }
    private void TileChangeListener(Tilemap tilemap, Tilemap.SyncTile[] tiles)
    {
        if(tilemap == current)
        {
            foreach(var tile in tiles)
            {
                if (tile.tile == null && availables.Contains(tile.position))
                {
                    availables.Remove(tile.position);
                    Debug.Log("Removed " + tile.position + " Now count: " + availables.Count);
                }
                else if (tile.tile != null && !availables.Contains(tile.position))
                {
                    availables.Add(tile.position);
                    Debug.Log("Added " + tile.position + " Now count: " + availables.Count);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button(new GUIContent("Load the Tilemap")))
        {
            current.ClearAllTiles();
            availables.Clear();

            typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
            LoadMap();
        }
        if (GUILayout.Button(new GUIContent("Save the Tilemap")))
        {
            WriteMap();
        }
    }
    private void LoadMap()
    {
        string readPath = EditorUtility.OpenFilePanel("Choose a map file", Application.dataPath, "xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(readPath);
        XmlNode root = doc.SelectSingleNode("map");
        XmlNodeList blocks = root.SelectNodes("block");
        for(int i = 0;i < blocks.Count;i++)
        {
            XmlNode position = blocks[i].SelectSingleNode("position");
            int x = System.Convert.ToInt32(position.Attributes["x"].Value);
            int y = System.Convert.ToInt32(position.Attributes["y"].Value);
            int z = System.Convert.ToInt32(position.Attributes["z"].Value);
            Vector3Int pos = new Vector3Int(x, y, z);
            int blockId = System.Convert.ToInt32(blocks[i].Attributes["id"].Value);
            current.SetTile(pos, BlockTileSO.Instance.GetTile(blockId));
        }
    }
    private void WriteMap()
    {
        string savePath = EditorUtility.SaveFilePanel("Choose a directory", Application.dataPath, "New Map", "xml");
        if (File.Exists(savePath))
            File.Delete(savePath);
        using (FileStream file = File.OpenWrite(savePath))
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "");
            xml.AppendChild(declaration);
            XmlElement root = xml.CreateElement("map");
            xml.AppendChild(root);
            foreach (var pos in availables)
            {
                XmlElement block = xml.CreateElement("block");
                block.SetAttribute("id", current.GetTile<BlockTile>(pos).BlockId.ToString());

                XmlElement position = xml.CreateElement("position");
                position.SetAttribute("x", pos.x.ToString());
                position.SetAttribute("y", pos.y.ToString());
                position.SetAttribute("z", pos.z.ToString());

                block.AppendChild(position);
                root.AppendChild(block);
                Debug.Log(root.ChildNodes.Count);
            }
            Debug.Log("Map Saved," + availables.Count);
            xml.Save(file);
        }
    }
}
