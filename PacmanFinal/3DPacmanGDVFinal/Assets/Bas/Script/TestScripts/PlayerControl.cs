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
    public bool left;
    public bool right;
    public float leftDistance;
    public float rightDistance;
    public float totalDistance;
    public Vector3 movement;

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

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.forward * 1;

        if (Input.GetButtonDown("Vertical")){
            transform.Rotate(new Vector3(0, 180, 0));
        }


        if (Input.GetKeyDown(KeyCode.D)){
            right = true;
            left = false;
        }
        if (Input.GetKeyDown(KeyCode.A)){
            left = true;
            right = false;
        }


        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.5f);
        if (Physics.Raycast(transform.position, transform.forward, 0.5f, walk)){
            swapSide();
        }


        RaycastHit hitLeftBack;
        Debug.DrawRay(transform.position - (transform.forward * .5f), -transform.right*3, Color.red, 1f);
        if (Physics.Raycast(transform.position - (transform.forward*.5f), -transform.right, out hitLeftBack, 3f, blocks)){
            //left
            leftDistance = Vector3.Distance(gameObject.transform.position, hitLeftBack.point);
        }
        else{
            leftDistance = 0;
        }
        RaycastHit hitRightBack;
        Debug.DrawRay(transform.position - (transform.forward * .50f), transform.right * 3, Color.red, 1f);
        if (Physics.Raycast(transform.position - (transform.forward * .50f), transform.right, out hitRightBack, 3f, blocks)){
            //right
            rightDistance = Vector3.Distance(gameObject.transform.position, hitRightBack.point);
        }
        else{
            rightDistance = 0;
        }


        if (rightDistance == 0 && right){
            RaycastHit hitRightFront;
            Debug.DrawRay(transform.position + (transform.forward * 1f), transform.right * 3, Color.red, 1f);
            if (!Physics.Raycast(transform.position + (transform.forward * 1f), transform.right, out hitRightFront, 3f, blocks)){
                //right
                StartCoroutine(rotationDelay(right));
            }
        }
        if (leftDistance == 0 && left){
            RaycastHit hitLeftFront;
            Debug.DrawRay(transform.position + (transform.forward * 1f), -transform.right * 3, Color.red, 1f);
            if (!Physics.Raycast(transform.position + (transform.forward * 1f), -transform.right, out hitLeftFront, 3f, blocks)){
                //left
                StartCoroutine(rotationDelay(left));
            }

        }

        /*
        if (rightDistance > leftDistance || leftDistance < 1f){
            rb.AddForce(transform.right * speed * Time.deltaTime);
        }
        if (rightDistance < leftDistance || rightDistance < 1f){
            rb.AddForce(-transform.right * speed * Time.deltaTime);
        }
        */
        if (Physics.Raycast(transform.position, transform.forward, 1f, blocks)){
            rb.velocity = Vector3.zero;
        }
        else{
            rb.velocity = movement * speed * Time.deltaTime;
        }

    }

    IEnumerator rotationDelay(bool LorR){

        yield return new WaitForSecondsRealtime(0.15f);

        if (LorR == right){
            transform.Rotate(new Vector3(0, 90, 0));
            right = false;
        }

        if (LorR == left){
            transform.Rotate(new Vector3(0, -90, 0));
            left = false;
        }
    }

    public void swapSide()
    {
        movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        transform.Rotate(new Vector3(-90, 0, 0));

    }
}


/*
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
    private float leftDistance;
    private float rightDistance;

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
        

Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
movement = transform.forward* 1;

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
            leftDistance = Vector3.Distance(gameObject.transform.position, );
            left = true;
        }
        else{
            left = false;
        }

        Debug.DrawRay(transform.position, new Vector3(2, 0, 0), Color.red, 1f);
        if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), 2f, blocks) == false){
            //right
            right = true;
        }
        else{
            right = false;
        }

        rb.AddForce(movement* speed * Time.deltaTime);
    }
}

 */
