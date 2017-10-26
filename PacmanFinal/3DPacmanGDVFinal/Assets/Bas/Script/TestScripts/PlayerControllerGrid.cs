using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGrid : MonoBehaviour {

    public enum InputDir { rust, w, a, s, d };

    public int tileX;
    public int tileY;
    public TileMap_ map;
    public InputDir moveDir = InputDir.rust;
    private InputDir movePref = InputDir.rust;

    void Start () {
        tileX = 10;//(map.mapSize / 2);
        tileY = 0;//(map.mapSize / 2);
	}


    void Update(){
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

        if (transform.position == new Vector3(tileX, 0, tileY)){
            UpdateWalkPos(1, 1);
        }
        else if (transform.position == new Vector3(tileX, tileY, 0)){
            UpdateWalkPos(1, 1);
            print("can update walk1");

        }


        #region
        /*
                if (Input.GetKeyDown(KeyCode.W)){
                    if (TileMap.tiles[tileX, tileY - 1] == 0){
                            tileY -= 1;                
                    }
                }
                if (Input.GetKeyDown(KeyCode.S)){
                    if (TileMap.tiles[tileX, tileY + 1] == 0){
                            tileY += 1;               
                    }
                }
                if (Input.GetKeyDown(KeyCode.A)){
                    if (TileMap.tiles[tileX + 1, tileY] == 0){
                            tileX += 1;           
                    }
                }
                if (Input.GetKeyDown(KeyCode.D)){
                    if (TileMap.tiles[tileX - 1, tileY] == 0){
                            tileX -= 1;
                    }
                }
        */
        #endregion

        map.MoveTo(tileX, tileY);
    }
    private void UpdateWalkPos(int x, int y)
    {
        print("can update walk");
        if (movePref == InputDir.w && map.tiles[tileX, tileY - 1] == 0){
            moveDir = InputDir.w;
        }
        if (movePref == InputDir.s && map.tiles[tileX, tileY + 1] == 0){
            moveDir = InputDir.s;
        }
        if (movePref == InputDir.a && map.tiles[tileX + 1, tileY] == 0){
            moveDir = InputDir.a;
        }
        if (movePref == InputDir.d && map.tiles[tileX - 1, tileY] == 0){
            moveDir = InputDir.d;
        }

        if (moveDir == InputDir.w && map.tiles[tileX, tileY - 1] == 0){
            if (map.tiles[tileX, tileY - 1] == map.tiles[tileX, - 1])
            {
                map.GetTileForDirection(0, -1);
                print("d");
            }
            tileY -= y;

        }
        if (moveDir == InputDir.s && map.tiles[tileX, tileY + 1] == 0){
            tileY += y;
        }
        if (moveDir == InputDir.a && map.tiles[tileX + 1, tileY] == 0){
            tileX += 1;
        }
        if (moveDir == InputDir.d && map.tiles[tileX - 1, tileY] == 0){
            tileX -= 1;
        }

    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGrid : MonoBehaviour {

    enum InputDir {rust,w,a,s,d};

    public int tileX;
    public int tileY;
    public TileMap map;

    private InputDir moveDir = InputDir.rust;

    void Start () {
        tileX = (map.mapSize / 2);
        tileY = (map.mapSize / 2);
	}


    void Update () {

        if (Input.GetKeyDown(KeyCode.W)){
            moveDir = InputDir.w;
        }
        if (Input.GetKeyDown(KeyCode.S)){
            moveDir = InputDir.s;
        }
        if (Input.GetKeyDown(KeyCode.A)){
            moveDir = InputDir.a;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            moveDir = InputDir.d;
        }


        switch (moveDir)
        {
            case InputDir.w:
                if (map.tiles[tileX, tileY - 1] == 0){
                    tileY -= 1;
                }
                break;
            case InputDir.a:
                if (map.tiles[tileX + 1, tileY] == 0){
                    tileX += 1;
                }
                break;
            case InputDir.s:
                if (map.tiles[tileX, tileY + 1] == 0){
                    tileY += 1;
                }
                break;
            case InputDir.d:
                if (map.tiles[tileX - 1, tileY] == 0){
                    tileX -= 1;
                }
                break;
            default:
                break;
        }

        #region
        
                if (Input.GetKeyDown(KeyCode.W)){
                    if (TileMap.tiles[tileX, tileY - 1] == 0){
                            tileY -= 1;                
                    }
                }
                if (Input.GetKeyDown(KeyCode.S)){
                    if (TileMap.tiles[tileX, tileY + 1] == 0){
                            tileY += 1;               
                    }
                }
                if (Input.GetKeyDown(KeyCode.A)){
                    if (TileMap.tiles[tileX + 1, tileY] == 0){
                            tileX += 1;           
                    }
                }
                if (Input.GetKeyDown(KeyCode.D)){
                    if (TileMap.tiles[tileX - 1, tileY] == 0){
                            tileX -= 1;
                    }
                }
        
#endregion

map.MoveTo(tileX, tileY);
    }
}



    */