using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public int speed;
    private Rigidbody rb;
    public LayerMask walk;
    public LayerMask blocks;
    private float moveHorizontal;
    private float moveVertical;
    private bool left;
    private bool right;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	

	void FixedUpdate () {
        /*
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.forward * moveVertical + transform.right * moveHorizontal;
        */

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.forward * 1;

        if (Input.GetAxis("Vertical") < 0){
            transform.Rotate(new Vector3(180, 0, 0));

        }

        if (Input.GetKeyDown(KeyCode.D) && right){
            transform.Rotate(new Vector3(0, 90, 0));
            right = false;
        }

        if (Input.GetKeyDown(KeyCode.A) && left){
            transform.Rotate(new Vector3(0, -90, 0));
            left = false;
        }

        Debug.DrawRay(transform.position, transform.forward, Color.red, 0.5f);
        if (Physics.Raycast(transform.position, transform.forward, 1f, walk)){
            movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            transform.Rotate(new Vector3(-90,0,0));
        }
        
        Debug.DrawRay(transform.position, new Vector3(-2, 0, 0), Color.red, 1f);
        if (Physics.Raycast(transform.position, new Vector3(-1,0,0), 2f, blocks) == false){
            //left
            left = true;
        }
        else
        {
            left = false;
        }

        Debug.DrawRay(transform.position, new Vector3(2, 0, 0), Color.red, 1f);
        if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), 2f, blocks) == false){
            //right
            right = true;
        }
        else
        {
            right = false;
        }

        rb.AddForce(movement * speed);
    }
}
