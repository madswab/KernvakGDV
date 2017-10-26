using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GridSystem {

    public System.Action onDataSet;

    private Dictionary<CubeSide, bool[]> sideData = new Dictionary<CubeSide, bool[]>();


    private void SetData(CubeSide side, Texture2D txt)
    {
        Color[] pixels = (txt.GetPixels(0, 0, count, count));
        bool[] data = new bool[count * count];

        for (int y = 0; y < count; y++) {
            for (int x = 0; x < count; x++) {

                if (GetWallHight(pixels[x + ((y) * (Count))]) <= 0.5f) {
                    data[(y * count) + x] = true;
                }
                else {
                    data[(y * count) + x] = false;
                }

            }
        }

        if (sideData.ContainsKey(side)) {
            sideData[side] = data;
        }
        else {
            sideData.Add(side, data);
        }

        if(onDataSet != null) { onDataSet(); }

    }


    public bool CanMoveTo(CubeSide side, Vector2 coord) {
        bool canMove = false;
        if (sideData.ContainsKey(side)) { 
            try {
                canMove = (sideData[side][((int)coord.y * count) + (int)coord.x]);
            }
            catch { canMove = false; }
        }
        return canMove;
    }

    public Vector3 CoordToPos(CubeSide side, Vector2 coord) {
        Vector3 ret = Vector3.zero;
        Vector3 c = Vector3.zero;

        switch (side) {
            case CubeSide.bottom:
                c = new Vector3(coord.x, 0, coord.y);
                break;
            case CubeSide.left:
                c = new Vector3(0, coord.y, coord.x);
                break;
            case CubeSide.front:
                c = new Vector3(coord.x, coord.y, (Count-1) * GridUnitSize);
                break;
            case CubeSide.top:
                c = new Vector3(coord.x, (Count-1)*GridUnitSize, coord.y);
                break;
            case CubeSide.right:
                c = new Vector3((Count-1) * GridUnitSize, coord.y, coord.x);
                break;
            case CubeSide.back:
                c = new Vector3(coord.x, coord.y, 0);
                break;
        }

        ret = FromGridCoords(c);

        return ret;
    }

    public Vector2 PosToCoord(CubeSide side, Vector3 pos) {
        Vector2 ret = Vector2.zero;
        Vector3 p = GetGridCoords(pos, true);

        switch (side) {
            case CubeSide.bottom:
                ret = new Vector2(p.x, p.z);
                break;
            case CubeSide.left:
                ret = new Vector2(p.z, p.y);
                break;
            case CubeSide.front:
                ret = new Vector2(p.x, p.y);
                break;
            case CubeSide.top:
                ret = new Vector2(p.x, p.z);
                break;
            case CubeSide.right:
                ret = new Vector2(p.z, p.y);
                break;
            case CubeSide.back:
                ret = new Vector2(p.x, p.y);
                break;
        }

        return ret;

    }


    public bool[,] To2DArray(CubeSide side) {
        bool[,] tdArr = new bool[count, count];
        
        for(int y=0; y<count; y++) {
            for(int x=0; x<count; x++) {
                tdArr[x,y] = sideData[side][(y * count) + x];
            }
        }

        return tdArr;
    }

    public int[,] To2DIntArray(CubeSide side) {
        int[,] tdArr = new int[count, count];

        for (int y = 0; y < count; y++) {
            for (int x = 0; x < count; x++) {
                tdArr[x, y] = sideData[side][(y * count) + x] ? 0: 1;
            }
        }

        return tdArr;
    }


    public void AddOnDataSet(System.Action a) {
        onDataSet += a;
    }

}
