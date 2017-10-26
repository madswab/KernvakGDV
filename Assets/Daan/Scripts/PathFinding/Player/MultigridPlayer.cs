using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultigridPlayer : MonoBehaviour, ICubeWalker {

    public static MultigridPlayer player;
    private static int score = 0;

    public CubeSide CurrentWalkOnSide { get { return walkOnSide; } set { walkOnSide = value; } }
    [SerializeField] private CubeSide walkOnSide = CubeSide.bottom;

    [SerializeField] private GameObject particles;  public GameObject Particles { get { return particles; } }

    private bool isAlive = true;

    [SerializeField] private UnityEngine.UI.Text  scoreText;
    [SerializeField] private GameObject gameOverImage;

    private Vector3 lastPos;

    [SerializeField] private AudioClip deathClip, scoreAddedClip, changedTile;

    private float chompCooldown = 0;


	private void Awake() {
        player = this;
        lastPos = transform.position;
        gameOverImage.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        this.ChangeWalkOnPlane((transform.position - lastPos).normalized);
        chompCooldown -= Time.fixedDeltaTime;
        if(changedTile != null && isAlive && chompCooldown <= 0) { 
            if(GridSystem.System.GetGridCoords(transform.position, true) != GridSystem.System.GetGridCoords(lastPos, true)) {
                AudioSource.PlayClipAtPoint(changedTile, NonSpatialAudio.listener.transform.position);//Camera.main.transform.position);
                chompCooldown = changedTile.length; //0.5f;
            }
        }
        lastPos = transform.position;
        if(scoreText != null) { 
            scoreText.text = "score: " + score;
        }
    }

    public void EndGame() {
        if (isAlive) {
            isAlive = false;
            gameOverImage.gameObject.SetActive(true);
            MonoBehaviour.print("Score: " + score);
            if(deathClip != null) {
                AudioSource.PlayClipAtPoint(deathClip, NonSpatialAudio.listener.transform.position); //Camera.main.transform.position);
            }
            StartCoroutine(waitForEndGame());
        }
    }

    public void AddScore() {
        if (isAlive) {
            score += 5;
            if(scoreAddedClip != null) {
                AudioSource.PlayClipAtPoint(scoreAddedClip, NonSpatialAudio.listener.transform.position); //Camera.main.transform.position);
            }
        }
    }

    private IEnumerator waitForEndGame() {
        do {
            if (Input.GetKey(KeyCode.Escape)) {
                print("Should change");
                score = 0;
                SceneManager.LoadScene(0);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        while (true);
    }

}
