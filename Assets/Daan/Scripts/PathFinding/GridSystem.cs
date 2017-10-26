using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif





public interface IGridSystem {
    Vector3 PlayerGridTile { get; set; }
    Vector3 ToGridPosition(Vector3 gridTile);
    Vector3 ToGridTilePosition(Vector3 pos, CubeSide side);
    Vector3 Position { get; }
    Quaternion Rotation { get; }
    int Count { get; }
    float GridUnitSize { get; }
    Vector3 TransfromPoint(Vector3 localpos);
    Vector3 GetGridCoords(Vector3 pos, bool doClamp);
    Vector3 FromGridCoords(Vector3 coords);
    Vector3 GetPlaneNormal(CubeSide side, NormalSide towards = NormalSide.inside);
    void CreateMeshes();
    void CreateMesh(Texture2D txt, CubeSide side, Material mat);
    bool CanMoveTo(CubeSide side, Vector2 coord);
    Vector3 CoordToPos(CubeSide side, Vector2 coord);
    Vector2 PosToCoord(CubeSide side, Vector3 pos);
    bool[,] To2DArray(CubeSide side);
    int[,] To2DIntArray(CubeSide side);
    void AddOnDataSet(System.Action a);
    float GetWallHight(Color col);
}





public enum CubeSide { top, bottom, left, right, front, back };
public enum NormalSide { inside, outside};




public partial class GridSystem : MonoBehaviour, IGridSystem {

    public static IGridSystem System {
        get {
            return system;
        }
    }
    private static IGridSystem system;

    public Vector3 PlayerGridTile {
        get {
            return playerGridTile;
        }
        set {
            playerGridTile = value;
        }
    }
    private Vector3 playerGridTile = Vector3.zero;


    private List<Mesh> meshes = new List<Mesh>();

    [SerializeField] private int count = 50;            public int Count { get { return count; } }
    [SerializeField] private float gridUnitSize = 2f;   public float GridUnitSize { get { return gridUnitSize; } }

    [SerializeField] private float wallHeight = 1;

    [SerializeField] 
    private Texture bottomMap, topMap, leftMap, rightMap, frontMap, backMap;

    [SerializeField]
    private Material bottomMat, topMat, leftMat, rightMat, frontMat, backMat;

    private Color? gridColorBottom = null, gridColorTop, gridColorLeft, gridColorRight, gridColorFront, gridColorBack;


	private void Awake () {
        system = this;
    }

    private void Start() {
        CreateMeshes();
    }

    private void OnApplicationQuit() {
        foreach(Mesh m in meshes) {
            m.Clear();
            Destroy(m);
        }
        meshes.Clear();
    }


    public Vector3 ToGridPosition(Vector3 gridTile) {
        Vector3 ret = gridTile;
        ret.x += gridUnitSize / 2;
        ret.y += gridUnitSize / 2;
        ret.z += gridUnitSize / 2;
        return ret;
    }


    public Vector3 ToGridTilePosition(Vector3 pos, CubeSide side) {
        Vector3 ret = transform.InverseTransformPoint(pos);
        ret.x = Mathf.Clamp(ret.x, 0, (count - 1)*gridUnitSize);
        ret.y = Mathf.Clamp(ret.y, 0, (count - 1)*gridUnitSize);
        ret.z = Mathf.Clamp(ret.z, 0, (count - 1)*gridUnitSize);

        ret.x /= gridUnitSize;
        ret.y /= gridUnitSize;
        ret.z /= gridUnitSize;

        ret.x = (int)Mathf.Floor(ret.x);
        ret.y = (int)Mathf.Floor(ret.y);
        ret.z = (int)Mathf.Floor(ret.z);

        ret.x *= gridUnitSize;
        ret.y *= gridUnitSize;
        ret.z *= gridUnitSize;

        switch (side) {
            case CubeSide.bottom: {
                    ret.y = 0;
                    break;
            }
            case CubeSide.top: {
                    ret.y = (count - 1)*gridUnitSize;
                    break;
            }
            case CubeSide.front: {
                    ret.z = (count - 1)*gridUnitSize;
                    break;
            }
            case CubeSide.back: {
                    ret.x = 0;
                    break;
            }
            case CubeSide.left: {
                    ret.x = 0;
                    break;
            }
            case CubeSide.right: {
                    ret.x = (count - 1)*gridUnitSize;
                    break;
            }
        }

        return ret;
    }

    public Vector3 GetGridCoords(Vector3 pos, bool doClamp) {
        Vector3 ret = transform.InverseTransformPoint(pos);
        if (doClamp) {
            ret.x = Mathf.Clamp(ret.x, 0, (count - 1) * gridUnitSize);
            ret.y = Mathf.Clamp(ret.y, 0, (count - 1) * gridUnitSize);
            ret.z = Mathf.Clamp(ret.z, 0, (count - 1) * gridUnitSize);
        }

        ret.x /= gridUnitSize;
        ret.y /= gridUnitSize;
        ret.z /= gridUnitSize;

        ret.x = (int)Mathf.Floor(ret.x);
        ret.y = (int)Mathf.Floor(ret.y);
        ret.z = (int)Mathf.Floor(ret.z);

        return ret;
    }

    public Vector3 FromGridCoords(Vector3 coords) {
        Vector3 ret = transform.InverseTransformPoint(coords);

        ret.x = (int)Mathf.Floor(ret.x);
        ret.y = (int)Mathf.Floor(ret.y);
        ret.z = (int)Mathf.Floor(ret.z);

        ret.x *= gridUnitSize;
        ret.y *= gridUnitSize;
        ret.z *= gridUnitSize;

        return ret;
    }


    public Texture GetTextureFromCubeSide(CubeSide side) {
        switch (side) {
            case CubeSide.bottom: {
                    return bottomMap;
                }
            case CubeSide.top: {
                    return topMap;
                }
            case CubeSide.front: {
                    return frontMap;
                }
            case CubeSide.back: {
                    return backMap;
                }
            case CubeSide.left: {
                    return leftMap;
                }
            case CubeSide.right: {
                    return rightMap;
                }
        }
        return null;
    }

    public Vector3 Position {
        get {
            return transform.position;
        }
    }
    public Quaternion Rotation {
        get {
            return transform.rotation;
        }
    }

    public Vector3 TransfromPoint(Vector3 localpos) {
        return transform.TransformPoint(localpos);
    }

    public Vector3 GetPlaneNormal(CubeSide side, NormalSide towards = NormalSide.inside) {
        switch(side) {
            case CubeSide.bottom: {
                    return -(transform.TransformPoint(0, 0, 0) - transform.TransformPoint(0, Count, 0)).normalized;
            }
            case CubeSide.top: {
                    return (transform.TransformPoint(0, 0, 0) - transform.TransformPoint(0, Count, 0)).normalized;
            }
            case CubeSide.left: {
                    return -(transform.TransformPoint(0, 0, 0) - transform.TransformPoint(Count, 0, 0)).normalized;
            }
            case CubeSide.right: {
                    return (transform.TransformPoint(0, 0, 0) - transform.TransformPoint(Count, 0, 0)).normalized;
            }
            case CubeSide.front: {
                    return (transform.TransformPoint(0, 0, 0) - transform.TransformPoint(0, 0, Count)).normalized;
            }
            case CubeSide.back: {
                    return -(transform.TransformPoint(0, 0, 0) - transform.TransformPoint(0, 0, Count)).normalized;
            }
        }
        return Vector3.zero;
    }


    public void CreateMeshes() {

        CreateMesh(bottomMap    as Texture2D, CubeSide.bottom   , bottomMat );
        CreateMesh(topMap       as Texture2D, CubeSide.top      , topMat    );
        CreateMesh(frontMap     as Texture2D, CubeSide.front    , frontMat  );
        CreateMesh(backMap      as Texture2D, CubeSide.back     , backMat   );
        CreateMesh(leftMap      as Texture2D, CubeSide.left     , leftMat   );
        CreateMesh(rightMap     as Texture2D, CubeSide.right    , rightMat  );

    }

    public void CreateMesh(Texture2D txt, CubeSide side, Material mat) {

        GameObject plane = new GameObject();
        plane.name = "Cube " + side.ToString();
        Mesh mesh = new Mesh();
        Color[] pixels = ( txt.GetPixels(0,0,count,count) );

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int y = 0; y<(Count); y++) {

            for(int x = 0; x<(Count); x++) {

                float height = GetWallHight(pixels[x + (y * (Count))]);
                

                if(height > 0.1f) {

                    int index = verts.Count;
                    

                    verts.Add(new Vector3(x * gridUnitSize, 0, y * gridUnitSize));
                    verts.Add(new Vector3((x+1) * gridUnitSize, 0, y * gridUnitSize));
                    verts.Add(new Vector3(x * gridUnitSize, 0, (y+1) * gridUnitSize));
                    verts.Add(new Vector3((x + 1) * gridUnitSize, 0, (y+1) * gridUnitSize));

                    verts.Add(new Vector3(x * gridUnitSize, wallHeight, y * gridUnitSize));
                    verts.Add(new Vector3((x + 1) * gridUnitSize, wallHeight, y * gridUnitSize));
                    verts.Add(new Vector3(x * gridUnitSize, wallHeight, (y + 1) * gridUnitSize));
                    verts.Add(new Vector3((x + 1) * gridUnitSize, wallHeight, (y + 1) * gridUnitSize));

                    //SIDE  //back
                    if (y>0) {
                        /*
                        if (Mathf.Min(
                                (pixels[x + ((y - 1) * (Count))].r),
                                (pixels[x + ((y - 1) * (Count))].g),
                                (pixels[x + ((y - 1) * (Count))].b)
                                ) >= 0.8f)
                        */
                        if(GetWallHight(pixels[x + ((y - 1) * (Count))]) <= 0.5f)

                        {
                            tris.Add(index + 4);
                            tris.Add(index + 1);
                            tris.Add(index);

                            tris.Add(index + 5);
                            tris.Add(index + 1);
                            tris.Add(index + 4);
                        }
                    }

                    //SIDE  //left
                    if (x > 0) {
                        /*
                        if (Mathf.Min(
                                   (pixels[(x - 1) + (y * (Count))].r),
                                   (pixels[(x - 1) + (y * (Count))].g),
                                   (pixels[(x - 1) + (y * (Count))].b)
                                   ) >= 0.8f)
                        */
                        if(GetWallHight(pixels[(x - 1) + (y * (Count))]) <= 0.5f) {
                            tris.Add(index);
                            tris.Add(index + 2);
                            tris.Add(index + 6);

                            tris.Add(index + 6);
                            tris.Add(index + 4);
                            tris.Add(index);
                        }
                    }

                    //SIDE  //right
                    if(y < Count - 1) {
                        /*
                        if (Mathf.Min(
                                   (pixels[x + ((y + 1) * (Count))].r),
                                   (pixels[x + ((y + 1) * (Count))].g),
                                   (pixels[x + ((y + 1) * (Count))].b)
                                   ) >= 0.8f)
                        */
                        if(GetWallHight(pixels[x + ((y + 1) * (Count))]) <= 0.5f) {
                            tris.Add(index + 7);
                            tris.Add(index + 2);
                            tris.Add(index + 3);

                            tris.Add(index + 2);
                            tris.Add(index + 7);
                            tris.Add(index + 6);
                        }
                    }

                    //SIDE  //front
                    if (x < Count-1) {
                        /*
                        if (Mathf.Min(
                                   (pixels[(x + 1) + (y * (Count))].r),
                                   (pixels[(x + 1) + (y * (Count))].g),
                                   (pixels[(x + 1) + (y * (Count))].b)
                                   ) >= 0.8f)
                        */
                        if(GetWallHight(pixels[(x + 1) + (y * (Count))]) <= 0.5f) {
                            tris.Add(index + 3);
                            tris.Add(index + 5);
                            tris.Add(index + 7);

                            tris.Add(index + 3);
                            tris.Add(index + 1);
                            tris.Add(index + 5);
                        }
                    }

                    //SIDE  //top
                    tris.Add(index + 6);
                    tris.Add(index + 5);
                    tris.Add(index + 4);

                    tris.Add(index + 7);
                    tris.Add(index + 5);
                    tris.Add(index + 6);
                                        
                                                       
                    uvs.Add(new Vector2(1f / Count * (x+1), 1f / Count * (y+1)));
                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * (y+1)));
                    uvs.Add(new Vector2(1f / Count * (x+1), 1f / Count * y)); 
                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * y));    

                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * y));
                    uvs.Add(new Vector2(1f / Count * (x + 1), 1f / Count * y));
                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * (y + 1)));
                    uvs.Add(new Vector2(1f / Count * (x + 1), 1f / Count * (y + 1)));

                }
                else {

                    int index = verts.Count;


                    verts.Add(new Vector3(x * gridUnitSize, 0, y * gridUnitSize));
                    verts.Add(new Vector3((x + 1) * gridUnitSize, 0, y * gridUnitSize));
                    verts.Add(new Vector3(x * gridUnitSize, 0, (y + 1) * gridUnitSize));
                    verts.Add(new Vector3((x + 1) * gridUnitSize, 0, (y + 1) * gridUnitSize));

                    tris.Add(index + 2);
                    tris.Add(index + 1);
                    tris.Add(index);

                    tris.Add(index + 3);
                    tris.Add(index + 1);
                    tris.Add(index + 2);

                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * y));
                    uvs.Add(new Vector2(1f / Count * (x + 1), 1f / Count * y));
                    uvs.Add(new Vector2(1f / Count * x, 1f / Count * (y + 1)));
                    uvs.Add(new Vector2(1f / Count * (x + 1), 1f / Count * (y + 1)));

                }

            }

        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        //mesh.RecalculateNormals();

        MeshRenderer renderer = plane.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = mat;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        plane.AddComponent<MeshFilter>().sharedMesh = mesh;

        plane.transform.parent = transform;

        meshes.Add(mesh);

        switch(side) {
            case CubeSide.bottom: {
                    plane.transform.localPosition = Vector3.zero;
                    plane.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    break;
            }
            case CubeSide.top: {
                    plane.transform.localPosition = new Vector3(Count*gridUnitSize, Count * gridUnitSize, 0);
                    plane.transform.localRotation = Quaternion.Euler(180, 180, 0);
                    break;
            }
            case CubeSide.front: {
                    plane.transform.localPosition = new Vector3(0, 0, Count * gridUnitSize);
                    plane.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    break;
            }
            case CubeSide.left: { 
                    plane.transform.localPosition = new Vector3(0, 0, 0);
                    plane.transform.localRotation = Quaternion.Euler(-90, -180, 90);
                    break;
            }
            case CubeSide.right: {
                    plane.transform.localPosition = new Vector3(Count * gridUnitSize, 0, count * gridUnitSize);
                    plane.transform.localRotation = Quaternion.Euler(-90, 90, 0);
                    break;
            }
            case CubeSide.back: {
                    plane.transform.localPosition = new Vector3(Count * gridUnitSize, 0, 0);
                    plane.transform.localRotation = Quaternion.Euler(-90, 90, 90);
                    break;
            }
        }

        SetData(side, txt);

    }


    public float GetWallHight(Color col) {
        float ret = 0;

        /*
        ret = (1 - Mathf.Min(
                (col.r),
                (col.g),
                (col.b)
                ));
        */

        if(col.r > 0.5f || col.g > 0.5f || col.b > 0.5f) {
            ret = 1;
        }

        return ret;
    }


    #if UNITY_EDITOR

    private void OnDrawGizmos() {
        if(!gridColorBottom.HasValue) {
            Color col = Color.Lerp(Color.yellow, Color.red, 0.4f);
            col.a = 0.5f;
            gridColorBottom = col;
        }
        if(!gridColorFront.HasValue) {
            Color col = Color.Lerp(Color.blue, Color.white, 0.2f);
            col.a = 0.3f;
            gridColorFront = col;
        }
        if(!gridColorLeft.HasValue) {
            Color col = Color.Lerp(Color.cyan, Color.green, 0.4f);
            col.a = 0.4f;
            gridColorLeft = col;
        }
        if(!gridColorRight.HasValue) {
            Color col = Color.Lerp(Color.yellow, Color.red, 0.2f);
            col.a = 0.2f;
            gridColorRight = col;
        }
        if(!gridColorBack.HasValue) {
            Color col = Color.Lerp(Color.blue, Color.white, 0.9f);
            col.a = 0.2f;
            gridColorBack = col;
        }
        if(!gridColorTop.HasValue) {
            Color col = Color.Lerp(Color.red, Color.white, 0.4f);
            col.a = 0.2f;
            gridColorTop = col;
        }

        Handles.Label(transform.TransformPoint(Vector3.zero), "0,0,0");
        Handles.Label(transform.TransformPoint(new Vector3(count * gridUnitSize, 0, count * gridUnitSize)), count + ",0," + count);
        Handles.Label(transform.TransformPoint(new Vector3(count * gridUnitSize, 0, 0)), count + ",0,0");
        Handles.Label(transform.TransformPoint(new Vector3(0, 0, count * gridUnitSize)), "0,0," + count);

        Handles.Label(transform.TransformPoint(new Vector3(0, count * gridUnitSize, count * gridUnitSize)), "0," + count + "," + count);
        Handles.Label(transform.TransformPoint(new Vector3(count * gridUnitSize, count * gridUnitSize, count * gridUnitSize)), count + "," + count + "," + count);

        Handles.Label(transform.TransformPoint(new Vector3(0, count * gridUnitSize, 0)), "0," + count + ",0");

        Handles.Label(transform.TransformPoint(new Vector3(count * gridUnitSize, count * gridUnitSize, 0)), count + "," + count + ",0");

        //BOTTOM PLANE

        for(int x=0; x<=count; x++) {
            Gizmos.color = gridColorBottom.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(x * gridUnitSize, 0, 0)), transform.TransformPoint(new Vector3(x * gridUnitSize, 0, count * gridUnitSize)));
        }
        for(int y=0; y<=count; y++) {
            Gizmos.color = gridColorBottom.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, 0, y * gridUnitSize)), transform.TransformPoint(new Vector3(count * gridUnitSize, 0, y * gridUnitSize)));
        }

        //END BOTTOM PLANE
        //FRONT PLANE

        for (int x = 0; x <= count; x++) {
            Gizmos.color = gridColorFront.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(x * gridUnitSize, 0, count * gridUnitSize)), transform.TransformPoint(new Vector3(x * gridUnitSize, count * gridUnitSize, count * gridUnitSize)));
        }
        for (int y = 0; y <= count; y++) {
            Gizmos.color = gridColorFront.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, y * gridUnitSize, count * gridUnitSize)), transform.TransformPoint(new Vector3(count * gridUnitSize, y * gridUnitSize, count * gridUnitSize)));
        }

        //END FRONT PLANE
        //LEFT PLANE

        for (int x = 0; x <= count; x++) {
            Gizmos.color = gridColorLeft.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, 0, x * gridUnitSize)), transform.TransformPoint(new Vector3(0, count * gridUnitSize, x * gridUnitSize)));
        }
        for (int y = 0; y <= count; y++) {
            Gizmos.color = gridColorLeft.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, y * gridUnitSize, 0)), transform.TransformPoint(new Vector3(0, y * gridUnitSize, count * gridUnitSize)));
        }

        //END LEFT PLANE
        //RIGHT PLANE

        for (int x = 0; x <= count; x++) {
            Gizmos.color = gridColorRight.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(count * gridUnitSize, 0, x * gridUnitSize)), transform.TransformPoint(new Vector3(count * gridUnitSize, count * gridUnitSize, x * gridUnitSize)));
        }
        for (int y = 0; y <= count; y++) {
            Gizmos.color = gridColorRight.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(count * gridUnitSize, y * gridUnitSize, 0)), transform.TransformPoint(new Vector3(count * gridUnitSize, y * gridUnitSize, count * gridUnitSize)));
        }

        //END RIGHT PLANE
        //BACK PLANE

        for (int x = 0; x <= count; x++) {
            Gizmos.color = gridColorBack.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(x * gridUnitSize, 0, 0)), transform.TransformPoint(new Vector3(x * gridUnitSize, count * gridUnitSize, 0)));
        }
        for (int y = 0; y <= count; y++) {
            Gizmos.color = gridColorBack.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, y * gridUnitSize, 0)), transform.TransformPoint(new Vector3(count * gridUnitSize, y * gridUnitSize, 0)));
        }

        //END BACK PLANE
        //TOP PLANE

        for (int x = 0; x <= count; x++) {
            Gizmos.color = gridColorTop.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(x * gridUnitSize, count * gridUnitSize, 0)), transform.TransformPoint(new Vector3(x * gridUnitSize, count * gridUnitSize, count * gridUnitSize)));
        }
        for (int y = 0; y <= count; y++) {
            Gizmos.color = gridColorTop.Value;
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(0, count * gridUnitSize, y * gridUnitSize)), transform.TransformPoint(new Vector3(count * gridUnitSize, count * gridUnitSize, y * gridUnitSize)));
        }

        //END TOP PLANE


        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(ToGridPosition(playerGridTile), new Vector3(gridUnitSize, gridUnitSize, gridUnitSize));


    }
	
    #endif

}
