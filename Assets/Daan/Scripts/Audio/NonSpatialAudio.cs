using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class NonSpatialAudio : MonoBehaviour {

    public static AudioListener listener;

	private void Start() {
        listener = GetComponent<AudioListener>();
    }

}
