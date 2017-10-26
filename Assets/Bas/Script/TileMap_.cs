using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap_ : MonoBehaviour {

    private enum InputDir { rust, w, a, s, d };

    public GameObject player;
    public int tileX;
    public int tileY;
    private int YswitchPosNeg   = 1;
    private int XswitchPosNeg   = 1;
    private InputDir moveDir    = InputDir.rust;
    private InputDir movePref   = InputDir.rust;
    private Vector3 pacmanRot   = new Vector3(0, 0, 0);
    private float pacmanSize;

    public TileType[] tileType;
    public int[,] tiles;
    public int[,] tiles1;
    public int[,] tiles2;
    public int[,] tiles3;
    public int[,] tiles4;
    public int[,] tiles5;
    private int[,] side;

    [Range(0, 1)] public float outline;
    public int mapSize          = 21;

    [SerializeField] private bool DoVisualize = false;

    private void Start() {
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

    private void Update() { 
        if (side == tiles4 || side == tiles3){
            if (Input.GetKeyDown(KeyCode.W)){
                movePref = InputDir.d;
            }
            if (Input.GetKeyDown(KeyCode.S)){
                movePref = InputDir.a;
            }
            if (Input.GetKeyDown(KeyCode.A)){
                movePref = InputDir.w;
            }
            if (Input.GetKeyDown(KeyCode.D)){
                movePref = InputDir.s;
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.W)) {
                movePref = InputDir.w;
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                movePref = InputDir.s;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                movePref = InputDir.a;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                movePref = InputDir.d;
            }
        }
        if (player.transform.position == TileCoordtoWorldCoord(tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)){
        UpdateWalkPos(1, 1);
        }

        MoveTo(tileX, tileY);
        player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(pacmanRot * 90);
    }

    private IEnumerator DelayedStart(){ // voor daan zijn scipts een korte delay
        yield return new WaitForEndOfFrame();
        pacmanSize  = player.transform.lossyScale.x;
        tileX       = mapSize / 2;
        tileY       = mapSize / 2;

        GenerateMap(ref tiles);
        GenerateMap(ref tiles1);
        GenerateMap(ref tiles2);
        GenerateMap(ref tiles3);
        GenerateMap(ref tiles4);
        GenerateMap(ref tiles5);

        float halfTileSize = tileType[tiles[0, 0]].tileVisuelPrefabe.transform.lossyScale.x / 2;

        GenerateMapVisual(ref tiles,    new Vector3(  0,   0,    0),    new Vector3(0,                          0,                  0                           ));           //bot
        GenerateMapVisual(ref tiles1,   new Vector3(-90,   0,    0),    new Vector3(0,                          0 + halfTileSize,   0 - halfTileSize            ));
        GenerateMapVisual(ref tiles2,   new Vector3(-90,   0,    0),    new Vector3(0,                          0 + halfTileSize,   mapSize - halfTileSize      ));
        GenerateMapVisual(ref tiles3,   new Vector3(  0,   0,  180),    new Vector3(mapSize - halfTileSize * 2, mapSize,            0                           ));           //top
        GenerateMapVisual(ref tiles4,   new Vector3(-90,   0,  -90),    new Vector3(mapSize - halfTileSize,     0 + halfTileSize,   0                           ));
        GenerateMapVisual(ref tiles5,   new Vector3(-90,   0,   90),    new Vector3(0 - halfTileSize,           0 + halfTileSize,   mapSize - halfTileSize * 2  ));

        side                = tiles;
        Vector3 marge       = transform.position;
        marge.y             = -0.5f;
        transform.position  = marge;
    }


    private void UpdateWalkPos(int x, int y){ // movement van de player
        int yDiraction = 0;
        int xDiraction = 0;
        if (movePref == InputDir.w) {
            yDiraction    = -1;
        }
        if (movePref == InputDir.s) {
            yDiraction    = 1;
        }
        if (movePref == InputDir.a) {
            xDiraction    = 1;
        }
        if (movePref == InputDir.d) {
            xDiraction    = -1;
        }        
        side = GetTileForDirection(xDiraction, yDiraction);

        if (side == tiles4 || side == tiles1) {
            YswitchPosNeg = -1;
        }
        else {
            YswitchPosNeg = 1;
        }
        if (side == tiles5){
            XswitchPosNeg = -1;
        }
        else{
            XswitchPosNeg = 1;
        }
        
        if (movePref == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0) {
            moveDir = InputDir.w;
        }
        if (movePref == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0) {
            moveDir = InputDir.s;
        }
        if (movePref == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0) {
            moveDir = InputDir.a;           
        }
        if (movePref == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0) {
           moveDir = InputDir.d;           
        }

        if (moveDir == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0) {
            tileY -= YswitchPosNeg;
            if (side == tiles3 || side == tiles4){
                pacmanRot = Vector3.down;
            }
            else{
                pacmanRot = Vector3.zero;
            }
        }
        if (moveDir == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0) {
            tileY += YswitchPosNeg;
            if (side == tiles3 || side == tiles4){
                pacmanRot = Vector3.up;
            }
            else{
                pacmanRot = Vector3.down * 2;
            }
        }
        if (moveDir == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0) {
            tileX += XswitchPosNeg;
            if (side == tiles3 || side == tiles4){
                pacmanRot = Vector3.down * 2;
            }
            else{
                pacmanRot = Vector3.down;
            }
        }
        if (moveDir == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0) {
            tileX -= XswitchPosNeg;
            if (side == tiles3 || side == tiles4){
                pacmanRot = Vector3.zero;
            }
            else{
                pacmanRot = Vector3.up;
            }
        }
    }

    public int[,] GetTileForDirection(int xdir, int ydir) { // voor switching van zijden 
        if (side == tiles){
            if (ydir < 0 && tileY == 0) {
                player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                return tiles1;
            }
            if (ydir > 0 && tileY == mapSize - 1) {
                player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                if (tileY >= mapSize - 1){
                    tileY   = 0;
                }
                return tiles2;
            }

            if (xdir < 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                if (tileX >= 0){
                    int temp = tileY;
                    tileY    = tileX;
                    tileX    = temp;
                    moveDir  = InputDir.s;
                    movePref = InputDir.s;
                }
                return tiles3;
            }
            if (xdir > 0 && tileX == mapSize - 1){
                if (tileX >= mapSize - 1){
                    tileX    = mapSize / 2;
                    tileY    = 0;
                    moveDir  = InputDir.w;
                    movePref = InputDir.w;
                }
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                return tiles4;
            }
        } 

        if (side == tiles1){
            if (ydir > 0 && tileY == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                return tiles;
            }
            if (ydir < 0 && tileY == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(180, 180, 0));
                if (tileY >= mapSize - 1){
                    tileX       = mapSize/2;
                    tileY       = 1;
                    moveDir     = InputDir.s;
                    movePref    = InputDir.s;
                }
                return tiles5;
            }

            if (xdir < 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                moveDir         = InputDir.a;
                movePref        = InputDir.a;
                return tiles3;
            }
            if (xdir > 0 && tileX == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                if (tileX >= mapSize - 1){
                    tileX       = 0;
                    tileY       = mapSize / 2;
                }
                return tiles4;
            }
        }

        if (side == tiles2){
            if (ydir < 0 && tileY == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (tileY <= 0){
                    tileY       = mapSize - 1;
                }
                return tiles;
            }
            if (ydir > 0 && tileY == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(180, 180, 0));
                if (tileY >= mapSize - 1){
                    moveDir     = InputDir.w;
                    movePref    = InputDir.w;
                    tileY       = mapSize - 1;
                }
                return tiles5;
            }

            if (xdir < 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                if (tileX == 0){
                    tileX       = mapSize - 1;
                }
                return tiles3;
            }
            if (xdir > 0 && tileX == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                moveDir         = InputDir.d;
                movePref        = InputDir.d;
                return tiles4;
            }
        }

        if (side == tiles3){
            if (ydir < 0 && tileY == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (tileY <= 0){
                    int temp    = tileY;
                    tileY       = tileX;
                    tileX       = temp;
                    moveDir     = InputDir.a;
                    movePref    = InputDir.a;
                }
                return tiles;
            }
            if (ydir > 0 && tileY == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                if (tileY >= mapSize - 1){
                    tileX       = 0;
                    tileY       = mapSize/2;
                    moveDir     = InputDir.d;
                    movePref    = InputDir.d;
                }
                return tiles5;
            }

            if (xdir < 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                if (tileX <= 0){
                    moveDir     = InputDir.a;
                    movePref    = InputDir.a;
                }
                return tiles1;
            }
            if (xdir > 0 && tileX ==  mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                if (tileX == mapSize -1){
                    tileX       = 0;
                }
                return tiles2;
            }
        }

        if (side == tiles4){
            if (ydir > 0 && tileY == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (tileY <= 0){
                    tileX       = mapSize - 1 - 1;
                    tileY       = mapSize / 2;
                    moveDir     = InputDir.d;
                    movePref    = InputDir.d;
                }
                return tiles;
            }
            if (ydir < 0 && tileY == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                if (tileY >= mapSize -1){
                    int temp    = tileX;
                    tileX       = tileY;
                    tileY       = temp;
                    moveDir     = InputDir.a;
                    movePref    = InputDir.a;
                }
                return tiles5;
            }

            if (xdir < 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                if (tileX <= 0){
                    tileX       = mapSize - 1;
                }
                return tiles1;
            }
            if (xdir > 0 && tileX == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                moveDir         = InputDir.d;
                movePref        = InputDir.d;
                return tiles2;
            }
        }

        if (side == tiles5){
            if (ydir < 0 && tileY == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                if (tileY <= 0){
                    moveDir     = InputDir.s;
                    movePref    = InputDir.s;
                    tileY       = mapSize - 1;
                }
                return tiles1;
            }
            if (ydir > 0 && tileY == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                moveDir         = InputDir.w;
                movePref        = InputDir.w;
                return tiles2;
            }

            if (xdir > 0 && tileX == 0){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                if (tileX <= 0){
                    tileX       = mapSize / 2;
                    tileY       = mapSize - 1;
                    moveDir     = InputDir.w;
                    movePref    = InputDir.w;
                }
                return tiles3;
            }
            if (xdir < 0 && tileX == mapSize - 1){
                player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                if (tileX >= mapSize - 1){
                    int temp   = tileX;
                    tileX      = tileY;
                    tileY      = temp;
                    moveDir    = InputDir.s;
                    movePref   = InputDir.s;
                }
                return tiles4;
            }
        }

        return side;
    }

    public void GenerateMap(ref int[,] arr) {
        arr = new int[mapSize, mapSize];
        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                arr[x, y] = 0;
            }
        }

        tiles  = GridSystem.System.To2DIntArray(CubeSide.bottom); 
        tiles1 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles2 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles3 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles4 = GridSystem.System.To2DIntArray(CubeSide.bottom);
        tiles5 = GridSystem.System.To2DIntArray(CubeSide.bottom);
    }

    public void GenerateMapVisual(ref int[,] arr, Vector3 rot, Vector3 pos) { // uiteindelijk niet in final maar voor dingen uit te werken helpt het om een beeld te krijgen.

        string holder = "Generated Map";
        //if (transform.Find(holder)){
        //DestroyImmediate(transform.Find(holder).gameObject);//------ was for update in scene
        //}
        Transform mapHolder = new GameObject(holder).transform;
        mapHolder.parent = transform;

        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                TileType tt             = tileType[arr[x, y]];
                Vector3 tilePos         = new Vector3(x, 0, y);
                GameObject go           = Instantiate(tt.tileVisuelPrefabe, tilePos, Quaternion.identity);
                go.transform.localScale = new Vector3(1, 0.1f, 1) * (1 - outline);
                go.transform.parent     = mapHolder;
            }
        }
        mapHolder.transform.Rotate(rot);
        mapHolder.transform.position = pos;
        mapHolder.gameObject.SetActive(DoVisualize);
    }

    public Vector3 TileCoordtoWorldCoord(int x, int y) {
        if (side == tiles1){
            return new Vector3(x, y, 0);
        }
        if (side == tiles2){
            return new Vector3(x, y, mapSize - (int)pacmanSize);
        }
        if (side == tiles3){
            return new Vector3(0, y, x);
        }
        if (side == tiles4){
            return new Vector3(mapSize - (int)pacmanSize, y, x);    
        }
        if (side == tiles5){
            return new Vector3(x, mapSize - (int)pacmanSize, y);
        }
        return new Vector3(x, 0, y);
    }   
    
    public void MoveTo(int x, int y){// uiteindelijke beweging. 
        tileX = x;
        tileY = y;

        player.transform.position = Vector3.MoveTowards(player.transform.position, TileCoordtoWorldCoord(x, y) + new Vector3(0.5f, 0.5f, 0.5f), 6f * Time.deltaTime);

    }
} 
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum WalkableOrNot { Walk, Wall };

public class TileMap : MonoBehaviour {

    private enum InputDir { rust, w, a, s, d };

    public GameObject player;
    public int tileX;
    public int tileY;
    private InputDir moveDir = InputDir.rust;
    private InputDir movePref = InputDir.rust;
    private Vector3 pacmanRot = new Vector3(0, 0, 0);

    //public PlayerControllerGrid playerGrid;
    public TileType[] tileType;
    public int[,] tiles;
    public int[,] tiles1;
    public int[,] tiles2;
    public int[,] tiles3;
    public int[,] tiles4;
    public int[,] tiles5;
    private int[,] side;

   [Range(0,1)]public float outline;
    public int mapSize = 21;

    void Start() {
        //playerGrid = player.GetComponent<PlayerControllerGrid>();

        tileX = 10;//(map.mapSize / 2);
        tileY = 0;//(map.mapSize / 2);

        GenerateMap(ref tiles);
        GenerateMap(ref tiles1);
        GenerateMap(ref tiles2);
        GenerateMap(ref tiles3);
        GenerateMap(ref tiles4);
        GenerateMap(ref tiles5);

        float halfTileSize = tileType[tiles[0, 0]].tileVisuelPrefabe.transform.lossyScale.x / 2;
        float posMapParts = halfTileSize + mapSize;

        GenerateMapVisual(ref tiles, new Vector3 (0,0,0), new Vector3(0, 0, 0));                                            //bot
        GenerateMapVisual(ref tiles1, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, 0 - halfTileSize));
        GenerateMapVisual(ref tiles2, new Vector3(-90, 0, 0), new Vector3(0, 0 + halfTileSize, mapSize - halfTileSize));
        GenerateMapVisual(ref tiles3, new Vector3(0, 0, 180), new Vector3(mapSize - halfTileSize*2, mapSize, 0));           //top
        GenerateMapVisual(ref tiles4, new Vector3(-90, 0, -90), new Vector3(mapSize - halfTileSize, 0 + halfTileSize, 0));
        GenerateMapVisual(ref tiles5, new Vector3(-90, 0, 90), new Vector3(0 - halfTileSize, 0 + halfTileSize, mapSize - halfTileSize * 2));
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.W)){
            movePref = InputDir.w;
        }
        if (Input.GetKeyDown(KeyCode.S)){
            movePref = InputDir.s;
        }
        if (Input.GetKeyDown(KeyCode.A)){
            movePref = InputDir.a;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            movePref = InputDir.d;
        }

        if (player.transform.position == new Vector3(tileX, 0, tileY) || player.transform.position == new Vector3(tileX, tileY, 0)){ /// misschien in range voor minder lag
            UpdateWalkPos(1, 1);
        }

        MoveTo(tileX, tileY);
        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(pacmanRot);

    }
    private void UpdateWalkPos(int x, int y)
    {
        int ydir = 0;
        int xdir = 0;
        if (movePref == InputDir.w){
            ydir = 1;
        }
        if (movePref == InputDir.s){
            ydir = -1;
        }
        if (movePref == InputDir.a){
            xdir = 1;
        }
        if (movePref == InputDir.d)
        {
            xdir = -1;
        }
        side = GetTileForDirection(xdir, ydir);
        int switchPosNeg = 1;
        if (side == tiles1){
            switchPosNeg = -1;
        }
        else{
            switchPosNeg = 1;
        }
        if (movePref == InputDir.w && side[tileX, tileY - switchPosNeg] == 0){
            moveDir = InputDir.w;
        }
        if (movePref == InputDir.s && side[tileX, tileY + switchPosNeg] == 0){
            moveDir = InputDir.s;
        }
        if (movePref == InputDir.a && side[tileX + 1, tileY] == 0){
            moveDir = InputDir.a;
        }
        if (movePref == InputDir.d && side[tileX - 1, tileY] == 0){
            moveDir = InputDir.d;
        }

        if (moveDir == InputDir.w && side[tileX, tileY - switchPosNeg] == 0){
            tileY -= switchPosNeg;
            pacmanRot = Vector3.left;
        }
        if (moveDir == InputDir.s && side[tileX, tileY + switchPosNeg] == 0){
            tileY += switchPosNeg;
            pacmanRot = Vector3.right;
        }
        if (moveDir == InputDir.a && side[tileX + 1, tileY] == 0){
            tileX += x;
            pacmanRot = Vector3.forward;
        }
        if (moveDir == InputDir.d && side[tileX - 1, tileY] == 0){
            tileX -= x;
            pacmanRot = Vector3.back;
        }
    }
    


    //public int[,] GetTileForDirection(int xdir, int ydir, ref TileMap map ){
    public int[,] GetTileForDirection(int xdir, int ydir /*,ref int[,] arr){  
        if (side != tiles1 && ydir > 0 && tileY == 0)
        {
            print("a");
player.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;          
        }
        if (side != tiles && ydir > 0 && tileY == 0)
        {
            print("a");
player.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        return tiles1;

        //if out of range
            //check dir for neighbour
            //set map to neighbour
            //return neighbour tile position
        //else 
            //return my tile position

        //if out of array check for hit and get that transform of grid.
        //out of range links is zelfde pos maar dan rechts of - dus;
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

    #region
    /*
    arr[2, 2] = 1;

    arr[5, 5] = 1;
    arr[5, 6] = 1;
    arr[5, 7] = 1;
    arr[5, 8] = 1;

    arr[9, 5] = 1;
    arr[9, 6] = 1;
    arr[9, 7] = 1;
    arr[9, 8] = 1;
    arr[9, 9] = 1;

    arr[5, 10] = 1;
    arr[6, 10] = 1;
    arr[7, 10] = 1;
    arr[8, 10] = 1;
    
}
#endregion

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
            //Vector3 tilePos = new Vector3(-mapSize/2 + (tt.tileVisuelPrefabe.transform.lossyScale.x) + x, 0, -mapSize / 2 + (tt.tileVisuelPrefabe.transform.lossyScale.y) + y);
            Vector3 tilePos = new Vector3(x, 0, y);
            GameObject go = (GameObject)Instantiate(tt.tileVisuelPrefabe, tilePos, Quaternion.identity);
            go.transform.localScale = new Vector3(1, 0.1f, 1) * (1 - outline);
            go.transform.parent = mapHolder;
        }
    }
    mapHolder.transform.Rotate(rot);
    mapHolder.transform.position = pos;
}

public Vector3 TileCoordtoWorldCoord(int x, int y)
{
    if (side == tiles1)
    {
        print("world");
        return new Vector3(x, y, 0); // y+1
    }
    return new Vector3(x, 0, y);
}

public void MoveTo(int x, int y)
{
    //playerGrid.tileX = x;
    //playerGrid.tileY = y;
    tileX = x;
    tileY = y;

    //       player.transform.position = TileCoordtoWorldCoord(x, y);
    player.transform.position = Vector3.MoveTowards(player.transform.position, TileCoordtoWorldCoord(x, y), 3f * Time.deltaTime);

}
}


*/
 

    /*
     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum WalkableOrNot { Walk, Wall };

public class TileMap_ : MonoBehaviour {

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

    void Start() {
        StartCoroutine(DelayedStart());
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

    if (player.transform.position == new Vector3(tileX, 0, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
        || player.transform.position == new Vector3(tileX, tileY, 0) + new Vector3(0.5f, 0.5f, 0.5f)
        || player.transform.position == new Vector3(0, tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
        || player.transform.position == new Vector3(tileX, tileY, 20) + new Vector3(0.5f, 0.5f, 0.5f)
        || player.transform.position == new Vector3(20, tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
        || player.transform.position == new Vector3(tileX, 20, tileY) + new Vector3(0.5f, 0.5f, 0.5f))
    {
        /// misschien in range voor minder lag
        UpdateWalkPos(1, 1);
        print("s");
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
        ydir = -1;
    }
    if (movePref == InputDir.s)
    {
        ydir = 1;
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
    int YswitchPosNeg = 1;
    int XswitchPosNeg = 1;
    if (side == tiles1 || side == tiles2)
    {
        YswitchPosNeg = -1;
    }
    else
    {
        YswitchPosNeg = 1;
    }
    if (side == tiles3 || side == tiles3)
    {
        XswitchPosNeg = -1;
    }
    else
    {
        XswitchPosNeg = 1;
    }

    if (movePref == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0)
    {
        moveDir = InputDir.w;
    }
    if (movePref == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0)
    {
        moveDir = InputDir.s;
    }
    if (movePref == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0)
    {
        moveDir = InputDir.a;
    }
    if (movePref == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0)
    {
        moveDir = InputDir.d;
    }

    if (moveDir == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0)
    {
        tileY -= YswitchPosNeg;
        pacmanRot = Vector3.zero;
    }
    if (moveDir == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0)
    {
        tileY += YswitchPosNeg;
        pacmanRot = Vector3.down * 2;
    }
    if (moveDir == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0)
    {
        tileX += XswitchPosNeg;
        pacmanRot = Vector3.down;
    }
    if (moveDir == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0)
    {
        tileX -= XswitchPosNeg;
        pacmanRot = Vector3.up;
    }
}

public int[,] GetTileForDirection(int xdir, int ydir)
{
    /*
        if (side != tiles && ydir < 0 && tileY == 0) {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (side != tiles1 && ydir > 0 && tileY == 0) {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
    if (side == tiles)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
        if (xdir > 0 && tileX == 20)
        {
            if (tileX >= 20)
            {
                print("d");
                tileX = 0;
            }
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
    }

    if (side == tiles1)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            return tiles5;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
    }

    if (side == tiles2)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles5;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
    }

    if (side == tiles3)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            return tiles5;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles1;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles2;
        }
    }

    if (side == tiles4)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            return tiles5;
        }
    }

    if (side == tiles5)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
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
    //if (side == tiles || side == tiles5) {
    //    return new Vector3(x, 0, y);
    //}
    if (side == tiles1)
    {
        return new Vector3(x, y, 0);
    }
    if (side == tiles2)
    {
        return new Vector3(x, y, mapSize);
    }
    if (side == tiles3)
    {
        return new Vector3(0, y, x);
    }
    if (side == tiles4)
    {
        return new Vector3(mapSize, x, y);
    }
    if (side == tiles5)
    {
        return new Vector3(x, mapSize, y);
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
     */

    /*
     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum WalkableOrNot { Walk, Wall };

public class TileMap_ : MonoBehaviour {

    private enum InputDir { rust, w, a, s, d };

    public GameObject player;
    public int tileX;
    public int tileY;
    private InputDir moveDir = InputDir.rust;
    private InputDir movePref = InputDir.rust;
    private Vector3 pacmanRot = new Vector3(0, 0, 0);
    private float pacmanSize;

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

    private void Start() {
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
        
        #endregion
    }

    private void Update()
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

    if (/*player.transform.position == new Vector3(tileX, 0, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
            || player.transform.position == new Vector3(tileX, tileY, 0) + new Vector3(0.5f, 0.5f, 0.5f)
            || player.transform.position == new Vector3(0, tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
            || player.transform.position == new Vector3(tileX, tileY, mapSize - pacmanSize) + new Vector3(0.5f, 0.5f, 0.5f)
            || player.transform.position == new Vector3(mapSize - pacmanSize, tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
            || player.transform.position == new Vector3(tileX, mapSize - pacmanSize, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
        player.transform.position == TileCoordtoWorldCoord(tileX, tileY) + new Vector3(0.5f, 0.5f, 0.5f)
        )
    {
        /// misschien in range voor minder lag
        UpdateWalkPos(1, 1);
        print("can move");
    }

    MoveTo(tileX, tileY);
    player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(pacmanRot * 90);
}

private IEnumerator DelayedStart()
{
    yield return new WaitForEndOfFrame();
    pacmanSize = player.transform.lossyScale.x;
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
        ydir = -1;
    }
    if (movePref == InputDir.s)
    {
        ydir = 1;
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
    int YswitchPosNeg = 1;
    int XswitchPosNeg = 1;
    if (side == tiles4 || side == tiles1)
    {
        YswitchPosNeg = -1;
    }
    else
    {
        YswitchPosNeg = 1;
    }
    if (side == tiles5)
    {
        XswitchPosNeg = -1;
    }
    else
    {
        XswitchPosNeg = 1;
    }

    if (movePref == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0)
    {
        if (side == tiles3 || side == tiles4)
        {
            moveDir = InputDir.d;
        }
        else
        {
            moveDir = InputDir.w;
        }
    }
    if (movePref == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0)
    {
        if (side == tiles3 || side == tiles4)
        {
            moveDir = InputDir.a;
        }
        else
        {
            moveDir = InputDir.s;
        }
    }
    if (movePref == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0)
    {
        if (side == tiles3 || side == tiles4)
        {
            moveDir = InputDir.w;
        }
        else
        {
            moveDir = InputDir.a;
        }
    }
    if (movePref == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0)
    {
        if (side == tiles3 || side == tiles4)
        {
            moveDir = InputDir.s;
        }
        else
        {
            moveDir = InputDir.d;
        }
    }

    if (moveDir == InputDir.w && side[tileX, tileY - YswitchPosNeg] == 0)
    {
        tileY -= YswitchPosNeg;
        pacmanRot = Vector3.zero;
    }
    if (moveDir == InputDir.s && side[tileX, tileY + YswitchPosNeg] == 0)
    {
        tileY += YswitchPosNeg;
        pacmanRot = Vector3.down * 2;
    }
    if (moveDir == InputDir.a && side[tileX + XswitchPosNeg, tileY] == 0)
    {
        tileX += XswitchPosNeg;
        pacmanRot = Vector3.down;
    }
    if (moveDir == InputDir.d && side[tileX - XswitchPosNeg, tileY] == 0)
    {
        tileX -= XswitchPosNeg;
        pacmanRot = Vector3.up;
    }
}

public int[,] GetTileForDirection(int xdir, int ydir)
{
    if (side == tiles)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            if (tileY >= mapSize - 1)
            {
                tileY = 0;
            }
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            if (tileX >= 0)
            {
                int temp = tileY;
                tileY = tileX + 1;
                tileX = temp;
            }
            return tiles3;
        }
        if (xdir > 0 && tileX == mapSize - 1)
        {
            if (tileX >= mapSize - 1)
            {
                tileX = mapSize / 2;
                tileY = 1;
            }
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
    }

    if (side == tiles1)
    {
        if (ydir > 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (ydir < 0 && tileY == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            moveDir = InputDir.rust;
            movePref = InputDir.rust;
            if (tileY >= mapSize - 1)
            {
                tileX = mapSize / 2;
                tileY = 1;
            }
            return tiles5;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 90, 0));
            return tiles3;
        }
        if (xdir > 0 && tileX == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (tileX >= mapSize - 1)
            {
                tileX = 0;
                tileY = mapSize / 2;
            }
            return tiles4;
        }
    }

    if (side == tiles2)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (tileY <= 0)
            {
                tileY = mapSize - 1;
            }
            return tiles;
        }
        if (ydir > 0 && tileY == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            moveDir = InputDir.rust;
            movePref = InputDir.rust;
            if (tileY >= mapSize - 1)
            {
                //tileX = 10;
                tileY = 19;
            }
            return tiles5;
        }

        if (xdir > 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
        if (xdir < 0 && tileX == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
    }

    if (side == tiles3)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (tileY <= 0)
            {
                int temp = tileY;
                tileY = tileX;
                tileX = temp;
            }
            return tiles;
        }
        if (ydir > 0 && tileY == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            return tiles5;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles1;
        }
        if (xdir > 0 && tileX == mapSize - 1)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }
    }

    if (side == tiles4)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            return tiles;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            return tiles5;
        }
    }

    if (side == tiles5)
    {
        if (ydir < 0 && tileY == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            return tiles1;
        }
        if (ydir > 0 && tileY == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            return tiles2;
        }

        if (xdir < 0 && tileX == 0)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            return tiles4;
        }
        if (xdir > 0 && tileX == 20)
        {
            player.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            return tiles3;
        }
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
    //if (side == tiles || side == tiles5) {
    //    return new Vector3(x, 0, y);
    //}
    if (side == tiles1)
    {
        return new Vector3(x, y, 0);
    }
    if (side == tiles2)
    {
        return new Vector3(x, y, mapSize - pacmanSize);
    }
    if (side == tiles3)
    {
        return new Vector3(0, y, x);
    }
    if (side == tiles4)
    {
        return new Vector3(mapSize - pacmanSize, y, x);
    }
    if (side == tiles5)
    {
        return new Vector3(x, mapSize - pacmanSize, y);
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
        , 6f * Time.deltaTime);

}
} 
    */