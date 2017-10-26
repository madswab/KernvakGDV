using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour, ICubeWalker {

    [SerializeField] private CubeSide walkOnSide = CubeSide.bottom;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private bool calculatePath = false;

	public CubeSide CurrentWalkOnSide
    {
        get { return walkOnSide;    }
        set { walkOnSide = value;   }
    }

	void Start () {
        StartCoroutine(testenum());
        this.ChangeWalkOnPlane(Vector3.zero);
	}
	

	void Update () {
        if (calculatePath)
        {
            calculatePath = false;
            Vector2[] path = AISystem.GetPath(GridSystem.System.PosToCoord(walkOnSide, transform.position), GridSystem.System.PosToCoord(walkOnSide, targetPosition), walkOnSide);
            transform.position = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[0]));
        }
	}


    private IEnumerator testenum()
    {
        yield return new WaitForSeconds(1);
        //print("Own position: " + GridSystem.System.CanMoveTo(walkOnSide, GridSystem.System.GetGridCoords(transform.position, true)));

        //print("time1: " + Time.time.ToString());
        //MonoBehaviour.print("start: "+GridSystem.System.PosToCoord(walkOnSide, transform.position));
        //MonoBehaviour.print("end :"+ GridSystem.System.PosToCoord(walkOnSide, targetPosition));
        Vector2[] path = AISystem.GetPath(GridSystem.System.PosToCoord(walkOnSide, transform.position), GridSystem.System.PosToCoord(walkOnSide, targetPosition), walkOnSide);
        //print("time2: " + Time.time.ToString());

        //transform.position = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[0]));

        //print("Path lenght: " + path.Length);
        StartCoroutine(walkTo(path));
    }


    private IEnumerator walkTo(Vector2[] path)
    {
        int i = 0;
        do
        {
            //print("Own position: " + GridSystem.System.CanMoveTo(walkOnSide, GridSystem.System.GetGridCoords(transform.position, true)));
            yield return new WaitForSeconds(0.5f);
            //MonoBehaviour.print(i + " / " + path.Length);
            transform.position = GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, path[i]));
            i++;
            //MonoBehaviour.print(GridSystem.System.PosToCoord(walkOnSide, transform.position));
        }
        while (i < path.Length);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(this.GetGridPosition(), Vector3.one);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GridSystem.System.ToGridPosition(GridSystem.System.FromGridCoords(targetPosition)), 0.5f);
            Gizmos.color = Color.white;
            //Gizmos.DrawCube(GridSystem.System.ToGridPosition(GridSystem.System.CoordToPos(walkOnSide, GridSystem.System.PosToCoord(walkOnSide, transform.position) + new Vector2(0, 1))), Vector3.one);
        }
        catch { }
    }
#endif

}
