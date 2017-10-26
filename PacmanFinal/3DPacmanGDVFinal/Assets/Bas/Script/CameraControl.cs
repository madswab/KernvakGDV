using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float yaw;
    public float pitch;
    public GameObject player;
    private Vector3 positionOffset;
    private int sizeMap;
    /*
    public float speed;
    public float cameraRotationRaduis;
    public float cameraHeight;
    public float cameraLookHeight;
    private float rotationOfCamera;
    
    public Transform sceneCamera;
    public GameObject Player;
    public float CameraRotationRaduis;
    public float CameraRotationSpeed;

    private float height = 2;
    private float rotationOfCamera;
    */

    public void Start () {
        sizeMap             = GameObject.Find("map(Grid)").GetComponent<TileMap_>().mapSize;
    }


    public void Update(){
        pitch               = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y"), 15, 60);
        yaw                += Input.GetAxis("Mouse X");

        positionOffset      = -Vector3.forward * 7.5f;
        positionOffset      = Quaternion.Euler(0, yaw, 0) * Quaternion.Euler(pitch, 0, 0) * positionOffset;
        //positionOffset    = new Quaternion(player.transform.rotation.y, player.transform.rotation.x, player.transform.rotation.z, player.transform.rotation.w) * positionOffset;
        positionOffset      = player.transform.rotation * positionOffset;

        Vector3 bounderies  = player.transform.position + positionOffset;
        bounderies.x        = Mathf.Clamp(bounderies.x, 1.5f, sizeMap - 1.5f);
        bounderies.y        = Mathf.Clamp(bounderies.y, 1.5f, sizeMap - 1.5f);
        bounderies.z        = Mathf.Clamp(bounderies.z, 1.5f, sizeMap - 1.5f);

        Camera.main.transform.position = Vector3.MoveTowards(transform.position ,bounderies, 15f * Time.deltaTime);  //bounderies; //player.transform.position + positionOffset;
        Camera.main.transform.LookAt(player.transform.position, player.transform.up);

    
        #region

        /*
        cameraLookHeight = Mathf.Clamp(cameraLookHeight - Input.GetAxis("Mouse Y"), -10, 60);

        rotationOfCamera += Input.GetAxis("Mouse X");

        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Camera.main.transform.rotation = Quaternion.Euler(cameraLookHeight, rotationOfCamera, 0) * player.transform.rotation;
        Camera.main.transform.Translate(0, cameraHeight, -cameraRotationRaduis);

        Camera.main.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        */
        /*
                if (Input.GetKey(KeyCode.Mouse1))
                {

                    rotationOfCamera += CameraRotationSpeed * Time.deltaTime;

                    sceneCamera.position = new Vector3(0, 0, 0);
                    sceneCamera.rotation = Quaternion.Euler(0, rotationOfCamera, 0);
                    sceneCamera.Translate(0, height + 0.5f, -CameraRotationRaduis);
                    sceneCamera.LookAt(new Vector3(Player.transform.position.x, height, Player.transform.position.z));
                }

            }*/
        #endregion

    }

}
