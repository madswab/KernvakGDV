using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 

[CustomEditor(typeof(TileMap_))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //TileMap_ map = target as TileMap_;

        //map.GenerateMap();
        //map.GenerateMapVisual();
    }

}
#endif