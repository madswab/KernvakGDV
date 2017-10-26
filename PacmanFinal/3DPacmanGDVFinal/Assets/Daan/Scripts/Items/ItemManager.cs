using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public static ItemManager Manager   { get { return manager      ; } }
    public Material NormalGhost         { get { return normalGhost  ; } }
    public Material KillerGhost         { get { return killerGhost  ; } }

    private static ItemManager manager;
    [SerializeField]
    private Material normalGhost, killerGhost;



    [SerializeField] private Texture    spawnMapBottom, 
                                        spawnMapTop, 
                                        spawnMapLeft,
                                        spawnMapRight,
                                        spawnMapFront,
                                        spawnMapBack;


    private Dictionary<CubeSide, List<Vector2>>     
        spawnpositions 
        = new Dictionary<CubeSide, List<Vector2>>()  ;

    private Dictionary<CubeSide, List<bool>>        
        spawnedBerries 
        = new Dictionary<CubeSide, List<bool>>()     ;




    private void Start() {
        manager = this;
        GridSystem.System.AddOnDataSet((System.Action)ReadSpawns);
        /*
        MonoBehaviour.print((01100001 | 01000000));
        byte b1 = 3 << 1;
        byte b2 = 4 << 1;

        b1 & b2 == b2)
        */
        StartCoroutine(waitToReadSpawns());
    }

    private IEnumerator waitToReadSpawns() {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        ReadSpawns();
    }

    private void ReadSpawns() {
        print("Reading data");
        ReadSpawnsForSide(CubeSide.bottom   , spawnMapBottom    );
        ReadSpawnsForSide(CubeSide.top      , spawnMapTop       );
        ReadSpawnsForSide(CubeSide.left     , spawnMapLeft      );
        ReadSpawnsForSide(CubeSide.right    , spawnMapRight     );
        ReadSpawnsForSide(CubeSide.front    , spawnMapFront     );
        ReadSpawnsForSide(CubeSide.back     , spawnMapBack      );
    }

    private void ReadSpawnsForSide(CubeSide side, Texture t) {
        Color[] pixels = ((Texture2D)t).GetPixels(0, 0, GridSystem.System.Count, GridSystem.System.Count);
        for(int y=0; y<GridSystem.System.Count; y++) {

            for(int x=0; x<GridSystem.System.Count; x++) {

                if (GridSystem.System.GetWallHight(pixels[(y*GridSystem.System.Count)+x]) >= 0.5f) {

                    if (!spawnpositions.ContainsKey(side)) {

                        List<Vector2> tmp = new List<Vector2>();
                        tmp.Add(new Vector2(x, y));
                        spawnpositions.Add(side, tmp);

                    }
                    else {
                        spawnpositions[side].Add(new Vector2(x, y));
                    }
                    //MonoBehaviour.print("s: " + side.ToString() + ", " + new Vector2(x, y));
                }
            }
        }

        MonoBehaviour.print("(item)side: " + side + ", count: " + spawnpositions[side].Count);

        spawnedBerries.Add(side, new List<bool>());
        for(int i=0; i<spawnpositions[side].Count; i++) {

            spawnedBerries[side].Add(false);

        }
    }

    public Vector2 GetRandomSpawn(CubeSide side) {
        print("len: " + (spawnpositions.ContainsKey(side) ? spawnpositions[side].Count.ToString() : "0"));
        if (spawnpositions.ContainsKey(side)) {

            try {
                return spawnpositions[side][Random.Range(0, spawnpositions[side].Count - 1)];
            }
            catch { return Vector2.zero; }

        }
        else {
            return Vector2.zero;
        }
    }

    public Vector2 GetRandomBerry(CubeSide side) {
        Vector2 ret = Vector2.zero;
        if (spawnpositions.ContainsKey(side)) {

            int wantedIndex = Random.Range(0, Random.Range(0, spawnpositions[side].Count));
            if (spawnpositions.ContainsKey(side) && spawnedBerries.ContainsKey(side)) {

                for (int i = 0; i < spawnpositions[side].Count; i++) {

                    try {
                        ret = spawnpositions[side][Random.Range(0, spawnpositions[side].Count - 1)];
                        if (i == wantedIndex && spawnedBerries[side][i]) {
                            break;
                        }
                    }
                    catch { }

                }
            }
            else {
                return Vector2.zero;
            }
            return ret;
        }
        else {
            return Vector2.zero;
        }
    }


    public bool IsSpawnPosition(Vector2 coord, CubeSide side) {
        return spawnpositions[side].Contains(coord);
    }


    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        foreach(KeyValuePair<CubeSide, List<Vector2>> pair in spawnpositions) {
            if (pair.Value.Count > 0) {
                for(int i=0; i<pair.Value.Count; i++) {
                    Gizmos.color = Color.yellow;
                    //Gizmos.DrawSphere(GridSystem.System.ToGridTilePosition(GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(pair.Key, pair.Value[i])),pair.Key), 0.7f);
                    Gizmos.DrawSphere(GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(pair.Key, pair.Value[i])), 0.8f);
                }
            }
        }
    }
    #endif

}
