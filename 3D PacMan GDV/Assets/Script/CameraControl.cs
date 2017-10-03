using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform sceneCamera;
    public GameObject Player;
    public float CameraRotationRaduis;
    public float CameraRotationSpeed;

    private float height = 2;
    private float rotationOfCamera;

    void Start () {
		
	}


    void Update () {

        if (Input.GetKey(KeyCode.Mouse1))
        {

            rotationOfCamera += CameraRotationSpeed * Time.deltaTime;

            sceneCamera.position = new Vector3(0, 0, 0);
            sceneCamera.rotation = Quaternion.Euler(0, rotationOfCamera, 0);
            sceneCamera.Translate(0, height + 0.5f, -CameraRotationRaduis);
            sceneCamera.LookAt(new Vector3(Player.transform.position.x, height, Player.transform.position.z));
        }

	}
}
