using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Tilemaps;
/// <summary>
/// A editor for create block data and initialize the block sprite.
/// </summary>
public class BlockTileCreator : EditorWindow
{
    public static readonly string OUTPUT_TILE_PATH = "Assets/Tile Palette/Terrain Palette";
    
    int blockId;
    string blockName;
    bool isSolid;
    Sprite sprite;

    [MenuItem("TileMap/Create BlockData")]
    private static void OpenInitializeWindow()
    {
        EditorWindow.GetWindow<BlockTileCreator>().Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        blockId = EditorGUILayout.IntField(new GUIContent("Block Id"), blockId);
        blockName = EditorGUILayout.TextField(new GUIContent("Block Name"),blockName);
        isSolid = EditorGUILayout.Toggle(new GUIContent("IsSolid"), isSolid);
        sprite = EditorGUILayout.ObjectField(new GUIContent("Block Sprite"), sprite, typeof(Sprite),false) as Sprite;
        if(GUILayout.Button(new GUIContent("Create the Block")))
        {
            BlockTile tile = ScriptableObject.CreateInstance<BlockTile>();
            tile.BlockId = blockId;
            tile.BlockName = blockName;
            tile.IsSolid = isSolid;
            tile.sprite = sprite;
            AssetDatabase.CreateAsset(tile, OUTPUT_TILE_PATH + $"/{blockName}.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Successfully created the block data.");
            AssetDatabase.Refresh();
            
            BlockTileSO.Instance.Tiles.Add(tile);
            GetWindow<BlockTileCreator>().Close();
        }
        EditorGUILayout.EndVertical();
    }
    private void CreateDataScript()
    {
        //string className = blockName + "Data";
        //string path = OUTPUT_SCRIPT_PATH + $"/{className}.cs";
        //if (File.Exists(path))
        //    File.Delete(path);
        //using (StreamWriter writer = new StreamWriter(File.OpenWrite(path)))
        //{
        //    writer.NewLine = "\n";
        //    writer.WriteLine($"public class {className} : BlockData");
        //    writer.WriteLine("{");
        //    writer.WriteLine($"\tpublic override int BlockId => {blockId};");
        //    writer.WriteLine($"\tpublic override string BlockName => \"{blockName}\";");
        //    string writesIsSolid = isSolid? "true" : "false";
        //    writer.WriteLine($"\tpublic override bool IsSolid => {writesIsSolid};");
        //    writer.WriteLine("}");
        //}
    }
    
}
