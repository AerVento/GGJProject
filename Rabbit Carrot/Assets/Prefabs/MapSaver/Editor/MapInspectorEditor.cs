using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// A inspector editor for map saver.
/// </summary>
[CustomEditor(typeof(MapSaver))]
public class MapInspectorEditor : Editor
{
    struct Map
    {
        public Tilemap map;
        public TilemapData data;
        public Map(Tilemap map, TilemapData data)
        {
            this.map = map;
            this.data = data;
        }
    }
    private static XmlMapHandler handler = new XmlMapHandler();

    private static bool isEditing = false;

    private static Grid grid;
    private static Map background;
    private static Map middleground;
    private static Map foreground;

    private void OnEnable()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            if(isEditing == false)
            {
                isEditing = true;
                grid = (target as MapSaver).GetComponent<Grid>();

                Initialize();

                //typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
                Tilemap.tilemapTileChanged += TileChangeListener;
            }
        }
    }
    private void OnDestroy()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() == null)
        {
            if(isEditing == true)
            {
                isEditing = false;
                
                Clear();

                Tilemap.tilemapTileChanged -= TileChangeListener;
                typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
            }
        }
    }
    private void TileChangeListener(Tilemap tilemap, Tilemap.SyncTile[] tiles)
    {
        void AddData(Map map,Tilemap.SyncTile[] tiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.tile == null && map.data.Contains(tile.position)) //if a not-null tile was changed to null
                {
                    //Remove all tile on the position
                    map.data.Blocks.RemoveAll((block) => block.Position == tile.position);
                    Debug.Log("Removed " + tile.position + " Now count: " + map.data.Blocks.Count);
                }
                else if (tile.tile != null && !map.data.Contains(tile.position)) //if a null tile was changed to not-null
                {
                    map.data.Blocks.Add(new TilemapData.BlockData(tile.position, (tile.tile as BlockTile).BlockId));
                    Debug.Log("Added " + tile.position + " Now count: " + map.data.Blocks.Count);
                }
            }
        }
        if(tilemap == background.map)
            AddData(background,tiles);
        else if(tilemap == middleground.map)
            AddData(middleground,tiles);
        else if(tilemap == foreground.map)
            AddData(foreground,tiles);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button(new GUIContent("Load the map")))
        {
            Clear();
            Initialize();

            typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
            LoadMap();
        }
        if (GUILayout.Button(new GUIContent("Save the map")))
        {
            WriteMap();
        }
    }
    private void LoadMap()
    {
        string readPath = EditorUtility.OpenFilePanel("Choose a map file", Application.dataPath, "xml");

        foreach(var pair in handler.Load(readPath))//不能直接覆盖mapData，因为在下面修改时还会在调用一遍修改函数
        {
            Tilemap map;
            switch (pair.layer)
            {
                case MapController.Layer.Background:
                    map = background.map;
                    break;
                case MapController.Layer.Middleground:
                    map = middleground.map;
                    break;
                case MapController.Layer.Foreground:
                    map = foreground.map;
                    break;
                default: return;
            }
            foreach(var block in pair.mapData)
            {
                Debug.Log(block.Position + ":" + pair.layer + ":" + block.BlockId);
                map.SetTile(block.Position, BlockTileSO.Instance.GetTile(block.BlockId));
            }
        }
    }
    private void WriteMap()
    {
        string savePath = EditorUtility.SaveFilePanel("Choose a directory", Application.dataPath, "New Map", "xml");
        if (File.Exists(savePath))
            File.Delete(savePath);
        int rootPos = savePath.LastIndexOf('/');
        int extensionPos = savePath.LastIndexOf(".");
        string fileName = savePath.Substring(rootPos, extensionPos - rootPos);
        
        MapData saves = new MapData() { MapName = fileName };
        saves[MapController.Layer.Background] = background.data;
        saves[MapController.Layer.Middleground] = middleground.data;
        saves[MapController.Layer.Foreground] = foreground.data;
        Debug.Log(background.data);
        

        handler.Save(savePath, saves);
    }

    private void Initialize()
    {
        MapSaver mapSaver = target as MapSaver;
        background = new Map(mapSaver.background,new TilemapData(MapController.Layer.Background));
        mapSaver.background.ClearAllTiles();

        middleground = new Map(mapSaver.middleground, new TilemapData(MapController.Layer.Middleground));
        mapSaver.middleground.ClearAllTiles();

        foreground = new Map(mapSaver.foreground, new TilemapData(MapController.Layer.Foreground));
        mapSaver.foreground.ClearAllTiles();
    }
    private void Clear()
    {
        GC.Collect();
    }
}
