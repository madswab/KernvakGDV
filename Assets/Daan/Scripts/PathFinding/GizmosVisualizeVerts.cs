#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosVisualizeVerts : MonoBehaviour {

    private Mesh mesh;
    private Vector3[] verts;

    private void Awake() {
        if(GetComponent<MeshFilter>() != null) {
            mesh = GetComponent<MeshFilter>().sharedMesh;
            verts = mesh.vertices;
        }
    }

    private void OnDrawGizmosSelected() {
        if (verts.Length > 0)
        {
            foreach (Vector3 vert in verts)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.TransformPoint(vert), 0.05f);
            }
        }
    }	

}

#endif
