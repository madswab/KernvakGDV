using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, IPlayer {

    [SerializeField] private CubeSide currentWalkOnSide = CubeSide.bottom;
    public CubeSide CurrentWalkOnSide
    {
        get { return currentWalkOnSide; }
        set { currentWalkOnSide = value; }
    }

    private Vector3 lastDir = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    private void Awake() {
        lastPos = transform.position;
    }

	private void FixedUpdate () {
        lastDir = (transform.position - lastPos).normalized;
        this.ChangeWalkOnPlane(lastDir);
	}

    

    #if UNITY_EDITOR

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        this.DrawCurrentPlaneGizmos();

        try
        {
            Gizmos.color = Color.Lerp(Color.magenta, Color.red, 0.3f);
            Gizmos.DrawLine(this.GetGridPosition(), this.GetGridPosition() + this.GetWalkOnPlaneNormal() * 5f);
        }
        catch {
            
        }
    }

    #endif

}
