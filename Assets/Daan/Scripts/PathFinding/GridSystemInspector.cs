#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(GridSystem))]
public class GridSystemInspector : Editor {

    private bool showEditorFunctions = false;
    private CubeSide selectedSide = CubeSide.bottom;
    private int xSelected = 0, ySelected = 0;
    private float localWallHeight = 0;
    private bool isCreatingMesh = false;
    private float meshCreationProgress = 0;

    /*
    public override void OnInspectorGUI()
    {
        showEditorFunctions = EditorGUILayout.Foldout(showEditorFunctions, "Show editor functions");
        if (showEditorFunctions)
        {

            GridSystem system = (GridSystem)target;
            
            selectedSide = (CubeSide)EditorGUILayout.EnumPopup("Side to edit", selectedSide);

            if (!isCreatingMesh)
            {
                if (GUILayout.Button("Create side from texture"))
                {

                    Texture2D txt = system.GetTextureFromCubeSide(selectedSide) as Texture2D;

                    createMesh(txt,selectedSide);

                }
            }
            else {
                Rect r = EditorGUILayout.BeginHorizontal();
                GUILayout.Label("");
                EditorGUI.ProgressBar(r, meshCreationProgress, "creating mesh");
                EditorGUILayout.EndHorizontal();
            }

        }
        GUILayout.Label("");
        
        base.OnInspectorGUI();
    }

    */

    private struct GridPanel {
        public int i00;
        public int i10;
        public int i01;
        public int i11;
        public Vector3 p00;
        public Vector3 p10;
        public Vector3 p01;
        public Vector3 p11;
        public float height;
        public GridPanel(int _i00, int _i10, int _i01, int _i11, Vector3 _p00, Vector3 _p10, Vector3 _p01, Vector3 _p11, float h) {
            i00 = _i00;
            i10 = _i10;
            i01 = _i01;
            i11 = _i11;
            p00 = _p00;
            p01 = _p01;
            p10 = _p10;
            p11 = _p11;
            height = h;
        }
    }

    private void createMesh(Texture2D txt, CubeSide side)
    {
        isCreatingMesh = true;

        GameObject obj = new GameObject();
        obj.name = side.ToString();
        obj.transform.position = GridSystem.System.Position;
        obj.transform.rotation = GridSystem.System.Rotation;

        Color[] pixels = txt.GetPixels();
        GridPanel[] panels = new GridPanel[pixels.Length];

        //panels[0] = CreatePanel(0,0, pixels[0]);


        isCreatingMesh = false;
    }

}


#endif