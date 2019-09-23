using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameControl : MonoBehaviour {

    // Use this for initialization

    public bool waiting;

    public float currentSpeed;
    public float targetSpeed;

    public float highSpeed;

    public float smoothTime = 2f;
    float yVelocity = 0.0f;

    public float loopThisFadeFactor = 10f ;

	// Update is called once per frame
	void Update () {
        //GetComponent<VideoPlayer>().frame
        float smoothhedSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref yVelocity, smoothTime);
        GetComponent<VideoPlayer>().playbackSpeed = smoothhedSpeed;
        currentSpeed = smoothhedSpeed;

        if (Input.GetKeyDown(KeyCode.P)) Skip();
        if (currentSpeed < 0.2) waiting = true;
        GetComponent<AudioReverbFilter>().decayTime = 1/smoothhedSpeed;
    }

    public void Skip()
    {
        //GetComponent<VideoPlayer>().playbackSpeed = highSpeed;
        currentSpeed = highSpeed;
        waiting = false;
    }
}
