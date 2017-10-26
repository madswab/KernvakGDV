using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PACMAN_Animation : MonoBehaviour {

    [SerializeField] private Material pacmanMaterial;
    [SerializeField]
    private Texture[] sprites;

    [SerializeField] private float framesPerSecond = 1;

    private int currentSprite = 0;
    float counter = 0;

    private void Awake() {
        pacmanMaterial.SetTexture("_MainTex", sprites[currentSprite]);
        pacmanMaterial.SetTexture("_EmissionMap", sprites[currentSprite]);
    }

    private void FixedUpdate() {
        counter += Time.fixedDeltaTime;
        if(counter >= 1f/framesPerSecond) {
            counter = 0;
            
            currentSprite++;
            if(currentSprite >= sprites.Length) {
                currentSprite = 0;
            }

            pacmanMaterial.SetTexture("_MainTex", sprites[currentSprite]);
            pacmanMaterial.SetTexture("_EmissionMap", sprites[currentSprite]);
        }
    }
	
}
