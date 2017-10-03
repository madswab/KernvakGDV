//oude code aangepast aan de coding conventions. niet een al te beste code.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovementCodingConvetions : MonoBehaviour
{

    public float speed;
    public float speedCurrent;
    public float speedMax;
    public bool jumpReady = true;
    public int jumpRange;
    public Vector3 direction;
    public List<GameObject> colPlayer = new List<GameObject>();
    public GameObject pauseMenu;
    public int maxScene;

    private Rigidbody2D rb;
    private bool rightPressed = false;
    private bool leftPressed = false;
    private int scaleTime = 0;
   // private Player Pl;

    void Start () {
        //Pl = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        print(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update () {
        speedCurrent = rb.velocity.magnitude;
        float moveVertical = Input.GetAxis("Horizontal");///////////
        Vector3 movement = new Vector3(moveVertical, 0.0f, 0.0f);////////////
        rb.AddForce(movement * speed);///////////
        
        if (Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        if (speedCurrent > speedMax){
            //direction = new Vector2(50, pos.y);
            //rb.AddForce(-direction);
            //float stop = SpeedCurrent - ((SpeedMax / 2) / 20);
            if (direction == new Vector3(speed, 0.0f, 0.0f)) {
                rb.velocity = new Vector3(9.9f, transform.position.y, transform.position.z);///
            }

            if (direction == new Vector3(-speed, 0.0f, 0.0f)){
                rb.velocity = new Vector3(-9.9f, transform.position.y, transform.position.z);///
            }
        } 

        if(rightPressed && speedCurrent <= speedMax){
            direction = new Vector3(speed, transform.position.y, transform.position.z);
            rb.AddForce(direction);
        }

        if (leftPressed && speedCurrent <= speedMax){
            direction = new Vector3(-speed, transform.position.y, transform.position.z); 
            rb.AddForce(direction);
        }    
    }

    void OnCollisionEnter2D(Collision2D col){
        colPlayer.Add(col.gameObject);
    }
     
    void OnCollisionExit2D(Collision2D col){
        colPlayer.Remove(col.gameObject);
    }


    public void Jump (){
        foreach (GameObject col in colPlayer){
            if(col.gameObject.tag == "Terrain"){
                jumpReady = true;
            }
            else{
                jumpReady = false;
            }
        }
        if (jumpReady){
            /*
            direction = new Vector3(transform.position.x, transform.position.y + jumpRange, transform.position.z);
            rb.AddForce(direction);
            */
            rb.velocity += Vector2.up * jumpRange;
            jumpReady = false;
        }
    }

    public void onPointerDownButForward(){
        rightPressed = true;
    }

    public void onPointerUpButForward(){
        rightPressed = false;
    }

    public void onPointerDownButBackwards(){
        leftPressed = true;
    }

    public void onPointerUpButBackwards(){
        leftPressed = false;
    }

    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
     
    public void NextLvl(){
        int NextScene = SceneManager.GetActiveScene().buildIndex + 1; 
        if (NextScene >= maxScene){
            Button temp = GameObject.Find("NextLvlBut").GetComponent<Button>();
            ColorBlock tempCol = temp.colors;
            temp.interactable = false;
            tempCol.pressedColor = tempCol.pressedColor;
            temp.colors = tempCol;            
        }
        else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
        } 
    }

    public void Pause(){
        scaleTime = scaleTime % 2;
        Time.timeScale = scaleTime;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        scaleTime += 1;
    }
}
