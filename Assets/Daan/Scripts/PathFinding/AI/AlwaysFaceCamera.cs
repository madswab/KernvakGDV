using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour {

    private new Camera camera;
    [SerializeField] private StandardAI walker;

	private void Awake() {
        camera = Camera.main;
    }

	private void FixedUpdate () {
        transform.position = walker.transform.position;
        transform.rotation = Quaternion.LookRotation( Vector3.ProjectOnPlane( (transform.position-camera.transform.position).normalized, GridSystem.System.GetPlaneNormal(walker.CurrentWalkOnSide)), GridSystem.System.GetPlaneNormal(walker.CurrentWalkOnSide));
	}

}
