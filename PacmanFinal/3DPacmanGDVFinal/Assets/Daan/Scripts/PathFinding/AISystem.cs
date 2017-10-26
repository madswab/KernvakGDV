using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class AISystem {

    public static float RespawnTime = 7f;
    
	public static Vector2[] GetPath(Vector2 currentCoords, Vector2 wantedCoords, CubeSide side) {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<Vector2> path = new List<Vector2>();
        Vector2 currentCoord = currentCoords;

        int count = 0;

        bool foundPath = false;

        Vector2 lastDir = Vector2.zero;

        do {
            count++;

            Vector2 direction = (wantedCoords - currentCoord).normalized;

            //MonoBehaviour.print("direction: " + direction);
            //MonoBehaviour.print("CurrentCoord: " + currentCoord);


            //if (direction.x == direction.y)
            //{
                //if can go to x?
                //if can go to y?
            //}

            if(true) {      //else 
                bool canXplus = false;
                bool canYplus = false;

                bool canXmin = false;
                bool canYmin = false;

                
                if(direction.x == 0 && direction.y == 0) {
                    foundPath = true;
                    break;
                }
                if (direction.x == 0) {
                    direction.x = (lastDir.x == 0?0.01f:(lastDir.x.Normalize()*0.01f));
                }
                if (direction.y == 0) {
                    direction.y = (lastDir.y == 0 ? 0.01f : (lastDir.y.Normalize() * 0.01f));
                }

                if (direction.x != 0) {
                    //MonoBehaviour.print("CanMoveTo "+GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(direction.x.Normalize(), 0)));
                    canXplus = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(direction.x.Normalize(), 0));
                    canXmin = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(-direction.x.Normalize(), 0));
                }
                if (direction.y != 0) {
                    //MonoBehaviour.print("CanMoveTo "+GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(0, direction.y.Normalize())));
                    canYplus = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(0, direction.y.Normalize()));
                    canYmin = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(0, -direction.y.Normalize()));
                }

                //MonoBehaviour.print("Can: y+ " + canYplus + ", y- " + canYmin + ", x+ " + canXplus + ", x- " + canXmin);


                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {

                    if (canXplus && lastDir != new Vector2(-direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(direction.x.Normalize(), 0);
                        lastDir = new Vector2(direction.x.Normalize(), 0);
                    }
                    else if (canYplus && lastDir != new Vector2(0,-direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, direction.y.Normalize());
                        lastDir = new Vector2(0, direction.y.Normalize());
                    }
                    else if(canYmin && lastDir != new Vector2(0, direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, -direction.y.Normalize());
                        lastDir = new Vector2(0, -direction.y.Normalize());
                    }
                    else if (canXmin && lastDir != new Vector2(direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(-direction.x.Normalize(), 0);
                        lastDir = new Vector2(-direction.x.Normalize(), 0);
                    }

                }
                else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {

                    if (canYplus && lastDir != new Vector2(0,-direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, direction.y.Normalize());
                        lastDir = new Vector2(0, direction.y.Normalize());
                    }
                    else if (canXplus && lastDir != new Vector2(-direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(direction.x.Normalize(), 0);
                        lastDir = new Vector2(direction.x.Normalize(), 0);
                    }
                    else if (canXmin && lastDir != new Vector2(direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(-direction.x.Normalize(), 0);
                        lastDir = new Vector2(-direction.x.Normalize(), 0);
                    }
                    else if (canYmin && lastDir != new Vector2(0, direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, -direction.y.Normalize());
                        lastDir = new Vector2(0, -direction.y.Normalize());
                    }

                }
                else if(Mathf.Abs(direction.y) == Mathf.Abs(direction.x)) {
                    if (canXplus && canYplus) {
                        bool canXplus2 = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(direction.x.Normalize()*2, 0));
                        bool canYplus2 = GridSystem.System.CanMoveTo(side, currentCoord + new Vector2(-direction.x.Normalize()*2, 0));
                        if (canXplus2 && lastDir != new Vector2(-direction.x.Normalize(), 0)) {
                            currentCoord = currentCoord + new Vector2(direction.x.Normalize(), 0);
                            lastDir = new Vector2(direction.x.Normalize(), 0);
                        }
                        else if (canYplus2 && lastDir != new Vector2(0, -direction.y.Normalize())) {
                            currentCoord = currentCoord + new Vector2(0, direction.y.Normalize());
                            lastDir = new Vector2(0, direction.y.Normalize());
                        }
                        else if (canYplus && lastDir != new Vector2(0, -direction.y.Normalize())) {
                            currentCoord = currentCoord + new Vector2(0, direction.y.Normalize());
                            lastDir = new Vector2(0, direction.y.Normalize());
                        }
                        else if (canXplus && lastDir != new Vector2(-direction.x.Normalize(), 0)) {
                            currentCoord = currentCoord + new Vector2(direction.x.Normalize(), 0);
                            lastDir = new Vector2(direction.x.Normalize(), 0);
                        }
                    }
                    else if (canYplus && lastDir != new Vector2(0, -direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, direction.y.Normalize());
                        lastDir = new Vector2(0, direction.y.Normalize());
                    }
                    else if (canXplus && lastDir != new Vector2(-direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(direction.x.Normalize(), 0);
                        lastDir = new Vector2(direction.x.Normalize(), 0);
                    }
                    else if (canXmin && lastDir != new Vector2(direction.x.Normalize(), 0)) {
                        currentCoord = currentCoord + new Vector2(-direction.x.Normalize(), 0);
                        lastDir = new Vector2(-direction.x.Normalize(), 0);
                    }
                    else if (canYmin && lastDir != new Vector2(0, direction.y.Normalize())) {
                        currentCoord = currentCoord + new Vector2(0, -direction.y.Normalize());
                        lastDir = new Vector2(0, -direction.y.Normalize());
                    }
                }


                //MonoBehaviour.print("canX" + canXplus + ", canY: " + canYplus);
                if(!canXplus && !canYplus && !canYmin && !canXmin) {
                    foundPath = true;
                }
                else {
                    path.Add(currentCoord);
                }

            }
            //foundPath = true;
            //path.Add(currentCoord);

            if (count >= GridSystem.System.Count * GridSystem.System.Count) {
                foundPath = true;
            }
            
        }
        while (!foundPath);

        //path.Add(currentCoord);

        stopwatch.Stop();
        MonoBehaviour.print("Millis: "+stopwatch.ElapsedMilliseconds);

        return path.ToArray();
    }
    


    /*
    public static Vector2[] GetPath(Vector2 currentCoords, Vector2 wantedCoords, CubeSide side)
    {

    }
    */


    private static int Normalize(this float f) {
        return (f > 0 ? 1 : (f < 0 ? -1 : 0));
    }

}
