using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TurnbasedGame.CustomUnityEditor
{
    [CustomEditor(typeof(BattleMapController))]
    public class BattleMapControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            BattleMapController bMC = (BattleMapController)target;
            
            // Create a button to easily allow us to compress Unity's Tilemap bounds
            if(GUILayout.Button("Compress Unity Tilemap's Bounds"))
            {
                bMC.CompressUnityTileMapBounds();
            }
        }
    }
}
