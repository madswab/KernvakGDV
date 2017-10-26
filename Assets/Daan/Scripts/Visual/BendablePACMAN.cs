using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendablePACMAN : MonoBehaviour {


    [SerializeField] private Material material;
    private Mesh mesh;

    [SerializeField]
    private float BendAngle = 0;

    private void Awake() {
        if(mesh == null) {
            mesh = new Mesh();

            Vector3[] verts = new Vector3[6];

            verts[0] = transform.InverseTransformPoint(new Vector3(0.5f, 0, -0.5f));
            verts[1] = transform.InverseTransformPoint(new Vector3(-0.5f, 0, -0.5f));

            verts[2] = transform.InverseTransformPoint(new Vector3(0.5f, 0, 0));
            verts[3] = transform.InverseTransformPoint(new Vector3(-0.5f, 0, 0));

            verts[4] = transform.InverseTransformPoint(new Vector3(0.5f, 0, 0.5f));
            verts[5] = transform.InverseTransformPoint(new Vector3(-0.5f, 0, 0.5f));

            
            List<int> tris = new List<int>();

            tris.Add(0);
            tris.Add(1);
            tris.Add(2);

            tris.Add(2);
            tris.Add(1);
            tris.Add(3);

            tris.Add(2);
            tris.Add(3);
            tris.Add(4);

            tris.Add(4);
            tris.Add(3);
            tris.Add(5);


            Vector2[] uvs = new Vector2[6];

            /*
            uvs[0] = new Vector2(1, 0);
            uvs[1] = new Vector2(0, 0);

            uvs[2] = new Vector2(1, 0.5f);
            uvs[3] = new Vector2(0, 0.5f);

            uvs[4] = new Vector2(1, 1);
            uvs[5] = new Vector2(0, 1);
            */

            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(0, 1);

            uvs[2] = new Vector2(0.5f, 0);
            uvs[3] = new Vector2(0.5f, 1);

            uvs[4] = new Vector2(1, 0);
            uvs[5] = new Vector2(1, 1);


            mesh.vertices = verts;
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs;

            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if(renderer == null) {
                renderer = gameObject.AddComponent<MeshRenderer>();
            }
            renderer.sharedMaterial = material;

            MeshFilter filter = GetComponent<MeshFilter>();
            if(filter == null) {
                filter = gameObject.AddComponent<MeshFilter>();
            }
            filter.mesh = mesh;

        }
    }

    /*
    private void FixedUpdate() {

        Vector3[] verts = mesh.vertices;

        verts[4] = Quaternion.Euler(transform.InverseTransformPoint(BendAngle, 0, 0)) * transform.InverseTransformPoint(new Vector3(0.5f, 0, 0.5f));
        verts[5] = Quaternion.Euler(transform.InverseTransformPoint(BendAngle, 0, 0)) * transform.InverseTransformPoint(new Vector3(-0.5f, 0, 0.5f));

        mesh.vertices = verts;

    }
    */

}
