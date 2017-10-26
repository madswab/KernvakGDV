using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAI : MonoBehaviour, ICubeWalker
{

    public CubeSide CurrentWalkOnSide
    {
        get { return walkOnSide; }
        set { walkOnSide = value; }
    }

    [SerializeField]
    private CubeSide walkOnSide = CubeSide.bottom;


    [SerializeField]
    protected bool hasBerry = false;


    [SerializeField]
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField]
    private bool calculatePath = false;

    [SerializeField]
    private float movementSpeed = 1.3f;
    private float maxMovement = 0;

    private Vector2[] path = new Vector2[0];
    private int pathIndex = 0;
    private bool initiated = false;

    [SerializeField]
    private new MeshRenderer renderer;

    private bool isAlive = true, canRespawn = false;

    [SerializeField]
    private List<MeshRenderer> renderers = new List<MeshRenderer>();


	private void Awake () {
        maxMovement = movementSpeed / 1f * (float)Time.fixedDeltaTime;
        StartCoroutine(fleeFromPACMAN());
	}
	
    private void FixedUpdate() {
        if (path.Length > 0) {
            transform.position = Vector3.MoveTowards(transform.position, GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[Mathf.Clamp(pathIndex, 0, path.Length - 1)])), maxMovement);
            if (ReachedPosition()) {        //Vector3.Distance(transform.position, GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[Mathf.Clamp(pathIndex, 0, path.Length - 1)]))) <= 0.001f)
                pathIndex++;
            }
        }

        if(Vector3.Distance(transform.position, MultigridPlayer.player.transform.position) <= GridSystem.System.GridUnitSize / 2f && isAlive) {
            if (hasBerry) {
                //player died
                MultigridPlayer.player.EndGame();
            }
            else {
                //AI died
                MultigridPlayer.player.AddScore();
                StartCoroutine(Die());
            }
        }
        else if(Vector3.Distance(transform.position, MultigridPlayer.player.transform.position) >= (GridSystem.System.GridUnitSize / 2f)*4 && !isAlive && canRespawn) {
            isAlive = true;
            canRespawn = false;
            foreach(MeshRenderer r in renderers) {
                r.enabled = true;
            }
        }

    }

    private IEnumerator Die() {
        isAlive = false;
        canRespawn = false;

        foreach (MeshRenderer r in renderers) {
            r.enabled = false;
        }

        GameObject p = Instantiate(MultigridPlayer.player.Particles);
        p.transform.position = transform.position;

        yield return new WaitForSeconds(AISystem.RespawnTime);

        Destroy(p);
        canRespawn = true;
    }

    private IEnumerator fleeFromPACMAN() {
        for (int i = 0; i < 3; i++) {
            yield return new WaitForFixedUpdate();
        }

        do {
            //int i = 0;
            //yield return new WaitForSeconds(1);

            float counter = 3;
            do {
                yield return new WaitForFixedUpdate();
                counter -= Time.fixedDeltaTime;
                //print(ReachedPosition());
                if (!initiated) {
                    goto skipWaiting;
                }
            }
            while (
                        //counter > 0 &&
                        //!ReachedPosition()
                        !ReachedTarget()
                        && counter > (!hasBerry ? -10 : -2)
                );

            if(ReachedTarget() && ItemManager.Manager.IsSpawnPosition(GridSystem.System.PosToCoord(walkOnSide, transform.position), walkOnSide) ) {
                StartCoroutine(GetBerry());
            }

            skipWaiting:
            initiated = true;

            Vector2 target = Vector2.zero;

            Vector2 coord = GridSystem.System.PosToCoord(walkOnSide, transform.position);

            if (MultigridPlayer.player.CurrentWalkOnSide != CurrentWalkOnSide) {
                int targetTile = GetWalkableTile((int)coord.x + (int)(coord.y * GridSystem.System.Count));
                targetPosition = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, new Vector2(targetTile % GridSystem.System.Count, Mathf.Floor(targetTile / GridSystem.System.Count))));
            }
            else {
                //walk to berry
                if (!hasBerry) {
                    //Vector2 
                        target = ItemManager.Manager.GetRandomBerry(walkOnSide);
                    if (target != Vector2.zero) {
                        targetPosition = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, target));
                    }
                }
                else {
                    targetPosition = MultigridPlayer.player.transform.position;
                }
            }

            //print("side: " + walkOnSide + ", target:" + GridSystem.System.PosToCoord(walkOnSide, targetPosition));

            pathIndex = 0;
            path = AISystem.GetPath(GridSystem.System.PosToCoord(walkOnSide, transform.position), GridSystem.System.PosToCoord(walkOnSide, targetPosition)
                //target
                , walkOnSide);

            //print("side: " + walkOnSide + ", path len: " + path.Length);
            //print("side: " + walkOnSide + ", pos: " + GridSystem.System.PosToCoord(walkOnSide, transform.position) + ", targ: " + GridSystem.System.PosToCoord(walkOnSide, targetPosition));

            /*
            Vector2[] path = AISystem.GetPath(GridSystem.System.PosToCoord(walkOnSide, transform.position), GridSystem.System.PosToCoord(walkOnSide, targetPosition), walkOnSide);
            

            float counter = 0, counter2 = 0; ;
            if (path.Length > 0)
            {   
                do
                {
                    yield return new WaitForFixedUpdate();
                    transform.position = Vector3.MoveTowards(transform.position, GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[Mathf.Clamp(i,0,path.Length-1)])), maxMovement);
                    //transform.position = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[Mathf.Clamp(i, 0, path.Length - 1)]));
                    //i++;
                    counter += Time.fixedDeltaTime;
                    counter2 += Time.fixedDeltaTime;
                    if(counter2 >= (1f / movementSpeed * Time.fixedDeltaTime)) {
                        counter2 = counter2 % (1f / movementSpeed * Time.fixedDeltaTime);
                        i++;
                    }
                }
                while (counter < 1);
                
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
            */
        }
        while (true);
    }


    private int GetWalkableTile(int sourceTile) {
        bool found = false;
        int Tile = sourceTile;
        List<int> alreadyCheckedTiles = new List<int>();
        int tries = 100;

        findFromTile:
        if (true) {
            tries--;
            if(tries != 100 && GridSystem.System.CanMoveTo(walkOnSide, new Vector2(Tile%//&
                GridSystem.System.Count, Mathf.Floor(Tile / GridSystem.System.Count)))) {

                found = true;
            }
            else {
                alreadyCheckedTiles.Add(Tile);
                int x = Random.Range(-1, 1);
                int y = Random.Range(-1, 1);
                if(x != 0 &&  Tile+x < Mathf.Pow(GridSystem.System.Count,2) && Tile+x >= 0) {

                    if (!alreadyCheckedTiles.Contains(Tile + x)) {
                        Tile = Tile + x;
                    }

                }
                else if(y != 0 && Tile+(y*GridSystem.System.Count) < Mathf.Pow(GridSystem.System.Count, 2) && Tile+(y*GridSystem.System.Count) >= 0) {

                    if(!alreadyCheckedTiles.Contains(Tile + (y * GridSystem.System.Count))) {
                        Tile = Tile + (y * GridSystem.System.Count);
                    }

                }

            }
        }          
        if (!found && tries > 0) {
            goto findFromTile;

        }
        if (found) {
            return Tile;
        }
        else {
            return sourceTile;
        }
    }

    private bool ReachedPosition() {
        if (path.Length > 0) {
            return (Vector3.Distance(transform.position, GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[Mathf.Clamp(pathIndex, 0, path.Length - 1)]))) <= 0.001f);
        }
        else { return false; }
    }

    private bool ReachedTarget() {
        if (path.Length > 0) {
            return (Vector3.Distance(transform.position, GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[path.Length - 1]))) <= 0.001f);
        }
        else { return false; }
    }

    private IEnumerator GetBerry() {
        hasBerry = true;
        renderer.sharedMaterial = ItemManager.Manager.KillerGhost;
        yield return new WaitForSeconds(5);
        renderer.sharedMaterial = ItemManager.Manager.NormalGhost;
        hasBerry = false;
    }

}
