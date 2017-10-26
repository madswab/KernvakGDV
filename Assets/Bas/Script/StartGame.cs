using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public GameObject mainCam;
    public GameObject particlesDestroy;

    void Start () {
        mainCam.SetActive(false);
        //StartCoroutine(Countdown());
    }


    void Update () {
        StartCoroutine(Countdown());

    }

    private IEnumerator Countdown() {
        yield return new WaitForSecondsRealtime(0.1f); 

        Time.timeScale                          = 0;

        gameObject.GetComponent<Animator>().Play("StartAni");

        yield return new WaitForSecondsRealtime(7.5f);

        mainCam.SetActive(true);
        Time.timeScale                          = 1;
        gameObject.SetActive(false);

        if (particlesDestroy != null){              //wilde misschien nog het begin van de game wat pimpen met fireworks maar het ziet er niet zo super uit.
             particlesDestroy.SetActive(false);
        }

    }
}
