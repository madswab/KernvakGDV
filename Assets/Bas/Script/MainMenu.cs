using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject controlsMenu;

    private void Start(){
        controlsMenu.SetActive(false);
    }

    private void Update()
    {
        if (controlsMenu.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                controlsMenu.SetActive(false);
            }
        }
    }

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void ControlsGame(){
        controlsMenu.SetActive(true);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
