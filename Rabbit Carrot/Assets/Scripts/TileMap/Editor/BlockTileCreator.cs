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
        blockId = EditorGUILayout.IntField(new GUIContent("Block Id"), blockId);
        if (BlockTileSO.Instance.GetTile(blockId) != null)
        {
            EditorGUILayout.HelpBox("Warning: A block tile with same id already exists, the original data will be modified.", MessageType.Warning);
        }
        blockName = EditorGUILayout.TextField(new GUIContent("Block Name"), blockName);
        isSolid = EditorGUILayout.Toggle(new GUIContent("IsSolid"), isSolid);
        sprite = EditorGUILayout.ObjectField(new GUIContent("Block Sprite"), sprite, typeof(Sprite), false) as Sprite;

        if (GUILayout.Button(new GUIContent("Create the Block")))
        {
            string path = EditorUtility.SaveFilePanel("Save the tile", Application.dataPath, blockName, "asset");
            if (path == null)
                return;
            string relativePath = path.Substring(path.IndexOf("Assets"));
            
            BlockTile tile = ScriptableObject.CreateInstance<BlockTile>();
            tile.BlockId = blockId;
            tile.BlockName = blockName;
            tile.IsSolid = isSolid;
            tile.sprite = sprite;
            
            AssetDatabase.CreateAsset(tile, relativePath);
            AssetDatabase.SaveAssets();
            Debug.Log("Successfully created the block data.");
            AssetDatabase.Refresh();

            BlockTileSO.Instance.Tiles.Add(tile);
            
            GetWindow<BlockTileCreator>().Close();
        }
    }
}
