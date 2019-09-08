using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFeedback : MonoBehaviour {

    public AudioSource[] sources;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEmotionLevel(int emotionID, float val)
    {
        //sources[emotionID].volume = val / 100f;
    }
}
