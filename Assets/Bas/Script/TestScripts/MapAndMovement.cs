using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAndMovement : MonoBehaviour {

    private enum InputDir { rust, w, a, s, d };

    public GameObject player;
    public int tileX;
    public int tileY;
    private InputDir moveDir = InputDir.rust;
    private InputDir movePref = InputDir.rust;
    private Vector3 pacmanRot = new Vector3(0, 0, 0);

    public TileType[] tileType;
    public int[,] tiles;
    public int[,] tiles1;
    public int[,] tiles2;
    public int[,] tiles3;
    public int[,] tiles4;
    public int[,] tiles5;
    private int[,] side;

    [Range(0, 1)] public float outline;
    public int mapSize = 21;

    [SerializeField] private bool DoVisualize = false;

    void Start()
    {
        StartCoroutine(DelayedStart());
        #region
        /*
        tileX = mapSize / 2;
        tileY = mapSize / 2;

        GenerateMap(ref tiles);
        GenerateMap(ref tiles1);
        GenerateMap(ref tiles2);
        GenerateMap(ref tiles3);
        GenerateMap(ref tiles4);
        GenerateMap(ref tiles5);

        float halfTileSize = tileType[tiles[0, 0]].tileVisuelPrefabe.transform.lossyScale.x / 2;

        GenerateMapVisual(ref tiles, new Vector3 (0,0,0), new Vector3(0, 0, 0));                                            //bot
        GenerateMapVisual(ref tiles1, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, 0 - halfTileSize));
        GenerateMapVisual(ref tiles2, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, mapSize - halfTileSize));
        GenerateMapVisual(ref tiles3, new Vector3(0, 0, 180), new Vector3(mapSize - halfTileSize*2, mapSize, 0));           //top
        GenerateMapVisual(ref tiles4, new Vector3(-90, 0, -90), new Vector3(mapSize - halfTileSize, 0 + halfTileSize, 0));
        GenerateMapVisual(ref tiles5, new Vector3(-90, 0, 90), new Vector3(0 - halfTileSize, 0 + halfTileSize, mapSize - halfTileSize * 2));

        side = tiles;
        Vector3 marge = transform.position;
        marge.y = -0.5f;
        transform.position = marge;
        */
#endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            movePref = InputDir.w;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movePref = InputDir.s;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movePref = InputDir.a;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            movePref = InputDir.d;
        }

        if (player.transform.position == new Vector3(tileX, 0, tileY) + new Vector3(0.5f, 0.5f, 0.5f) || player.transform.position == new Vector3(tileX, tileY, 0) + new Vector3(0.5f, 0.5f, 0.5f))
        { /// misschien in range voor minder lag
            UpdateWalkPos(1, 1);
        }

        MoveTo(tileX, tileY);
        player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(pacmanRot * 90);
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        tileX = mapSize / 2;
        tileY = mapSize / 2;

        GenerateMap(ref tiles);
        GenerateMap(ref tiles1);
        GenerateMap(ref tiles2);
        GenerateMap(ref tiles3);
        GenerateMap(ref tiles4);
        GenerateMap(ref tiles5);

        float halfTileSize = tileType[tiles[0, 0]].tileVisuelPrefabe.transform.lossyScale.x / 2;

        GenerateMapVisual(ref tiles, new Vector3(0, 0, 0), new Vector3(0, 0, 0));                                            //bot
        GenerateMapVisual(ref tiles1, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, 0 - halfTileSize));
        GenerateMapVisual(ref tiles2, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, mapSize - halfTileSize));
        GenerateMapVisual(ref tiles3, new Vector3(0, 0, 180), new Vector3(mapSize - halfTileSize * 2, mapSize, 0));           //top
        GenerateMapVisual(ref tiles4, new Vector3(-90, 0, -90), new Vector3(mapSize - halfTileSize, 0 + halfTileSize, 0));
        GenerateMapVisual(ref tiles5, new Vector3(-90, 0, 90), new Vector3(0 - halfTileSize, 0 + halfTileSize, mapSize - halfTileSize * 2));

        side = tiles;
        Vector3 marge = transform.position;
        marge.y = -0.5f;
        transform.position = marge;
    }


    private void UpdateWalkPos(int x, int y)
    {
        int ydir = 0;
        int xdir = 0;
        if (movePref == InputDir.w)
        {
            ydir = 1;
        }
        if (movePref == InputDir.s)
        {
            ydir = -1;
        }
        if (movePref == InputDir.a)
        {
            xdir = 1;
        }
        if (movePref == InputDir.d)
        {
            xdir = -1;
        }
        side = GetTileForDirection(xdir, ydir);

        int switchPosNeg = 1;
        if (side == tiles1)
        {
            switchPosNeg = -1;
        }
        else
        {
            switchPosNeg = 1;
        }

        if (movePref == InputDir.w && side[tileX, tileY - switchPosNeg] == 0)
        {
            moveDir = InputDir.w;
        }
        if (movePref == InputDir.s && side[tileX, tileY + switchPosNeg] == 0)
        {
            moveDir = InputDir.s;
        }
        if (movePref == InputDir.a && side[tileX + 1, tileY] == 0)
        {
            moveDir = InputDir.a;
        }
        if (movePref == InputDir.d && side[tileX - 1, tileY] == 0)
        {
            moveDir = InputDir.d;
        }

        if (moveDir == InputDir.w && side[tileX, tileY - switchPosNeg] == 0)
        {
            tileY -= switchPosNeg;
            pacmanRot = Vector3.zero;
        }
        if (moveDir == InputDir.s && side[tileX, tileY + switchPosNeg] == 0)
        {
            tileY += switchPosNeg;
            pacmanRot = Vector3.down * 2;
        }
        if (moveDir == InputDir.a && side[tileX + 1, tileY] == 0)
        {
            tileX += x;
            pacmanRot = Vector3.down;
        }
        if (moveDir == InputDir.d && side[tileX - 1, tileY] == 0)
        {
            tileX -= x;
            pacmanRot = Vector3.up;
        }
    }

    public int[,] GetTileForDirection(int xdir, int ydir)
    {
        if (side != tiles && ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (side != tiles1 && ydir > 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }

        return side;
    }

    public void GenerateMap(ref int[,] arr)
    {
        arr = new int[mapSize, mapSize];
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                arr[x, y] = 0;
            }
        }

        tiles = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles1 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles2 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles3 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles4 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles5 = GridSystem.System.To2DIntArray(CubeSide.bottom);
    }

    public void GenerateMapVisual(ref int[,] arr, Vector3 rot, Vector3 pos)
    {

        string holder = "Generated Map";
        //if (transform.Find(holder)){
        //DestroyImmediate(transform.Find(holder).gameObject);//------ was for update in scene
        //}
        Transform mapHolder = new GameObject(holder).transform;
        mapHolder.parent = transform;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                TileType tt = tileType[arr[x, y]];
                Vector3 tilePos = new Vector3(x, 0, y);
                GameObject go = (GameObject)Instantiate(tt.tileVisuelPrefabe, tilePos, Quaternion.identity);
                go.transform.localScale = new Vector3(1, 0.1f, 1) * (1 - outline);
                go.transform.parent = mapHolder;
            }
        }
        mapHolder.transform.Rotate(rot);
        mapHolder.transform.position = pos;
        mapHolder.gameObject.SetActive(DoVisualize);
    }

    public Vector3 TileCoordtoWorldCoord(int x, int y)
    {
        if (side == tiles || side == tiles5)
        {
            return new Vector3(x, 0, y);
        }
        if (side == tiles1 || side == tiles2)
        {
            return new Vector3(x, y, 0);
        }
        if (side == tiles3 || side == tiles4)
        {
            return new Vector3(0, y, x);
        }
        return new Vector3(x, 0, y);
    }

    public void MoveTo(int x, int y)
    {
        tileX = x;
        tileY = y;

        player.transform.position = Vector3.MoveTowards(player.transform.position,
            TileCoordtoWorldCoord(x, y)
             + new Vector3(0.5f, 0.5f, 0.5f)
            , 3f * Time.deltaTime);

    }
}