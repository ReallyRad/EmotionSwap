using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameControl : MonoBehaviour {

    // Use this for initialization

    public bool waiting;
    public bool skipping;

    public float currentSpeed;
    public float targetSpeed;

    public float highSpeed;

    float smoothTime =  2f;
    float yVelocity = 0.0f;


    void Start () {
        //skip to a random point in video with slowing down

        //wait for matching score to be high enough

        //match audio pitch to video skipping
    }
	
	// Update is called once per frame
	void Update () {
        //GetComponent<VideoPlayer>().frame
        float smoothhedSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref yVelocity, smoothTime);
        GetComponent<VideoPlayer>().playbackSpeed = smoothhedSpeed;
        currentSpeed = smoothhedSpeed;

        if (Input.GetKeyDown(KeyCode.P)) Skip();
    }

    public void Skip()
    {
        GetComponent<VideoPlayer>().playbackSpeed = highSpeed;
        currentSpeed = highSpeed;
    }
}
