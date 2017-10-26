using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ICubeWalkerMethods {

    public static void DrawCurrentPlaneGizmos(this IPlayer player) {
        if (GridSystem.System != null) {
            try {
                GridSystem.System.PlayerGridTile = GridSystem.System.ToGridTilePosition(player.transform.position, player.CurrentWalkOnSide);
            }
            catch {

            }
        }
    }

    public static void ChangeWalkOnPlane(this ICubeWalker walker, Vector3 movementDirection) {
        Vector3 tilePosition = GridSystem.System.GetGridCoords(walker.transform.position + movementDirection.normalized * GridSystem.System.GridUnitSize, false);
        int count = GridSystem.System.Count-1;
        
        if(tilePosition.x > count) {
            walker.CurrentWalkOnSide = CubeSide.right;
        }
        else if(tilePosition.x < 0) {
            walker.CurrentWalkOnSide = CubeSide.left;
        }
        else if(tilePosition.y > count) {
            walker.CurrentWalkOnSide = CubeSide.top;
        }
        else if(tilePosition.y < 0) {
            walker.CurrentWalkOnSide = CubeSide.bottom;
        }
        else if(tilePosition.z > count) {
            walker.CurrentWalkOnSide = CubeSide.front;
        }
        else if(tilePosition.z < 0) {
            walker.CurrentWalkOnSide = CubeSide.back;
        }

    }

    public static Vector3 GetWalkOnPlaneNormal(this ICubeWalker walker) {
        return GridSystem.System.GetPlaneNormal(walker.CurrentWalkOnSide);
    }

    public static Vector3 GetGridTilePosition(this Vector3 pos, CubeSide side) {
        try {
            return GridSystem.System.ToGridTilePosition(pos, side);
        }
        catch {
            return pos;
        }
    }

    public static Vector3 GetGridTilePosition(this ICubeWalker walker) {
        try {
            return GridSystem.System.ToGridTilePosition(walker.transform.position, walker.CurrentWalkOnSide);
        }
        catch {
            return walker.transform.position;
        }
    }

    public static Vector3 GetGridPosition(this Vector3 pos, CubeSide side) {
        try {
            return GridSystem.System.ToGridPosition(GridSystem.System.ToGridTilePosition(pos, side));
        }
        catch {
            return pos;
        }
    }

    public static Vector3 GetGridPosition(this ICubeWalker walker)
    {
        try {
            return GridSystem.System.ToGridPosition(GridSystem.System.ToGridTilePosition(walker.transform.position, walker.CurrentWalkOnSide));
        }
        catch {
            return walker.GetGridPosition();
        }
    }

}
