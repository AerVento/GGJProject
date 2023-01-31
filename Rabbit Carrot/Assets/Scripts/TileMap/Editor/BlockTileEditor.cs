using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockTile))]
public class BlockTileEditor : Editor
{
    BlockTile tile;
    private void OnEnable()
    {
        tile = (BlockTile)target;
    }
    public override void OnInspectorGUI()
    {
        tile.BlockId = EditorGUILayout.IntField(new GUIContent("Block Id"),tile.BlockId);
        tile.BlockName = EditorGUILayout.TextField(new GUIContent("Block Name"), tile.BlockName);
        tile.IsSolid = EditorGUILayout.Toggle(new GUIContent("Is Solid"), tile.IsSolid);
        base.OnInspectorGUI();
    }
}
