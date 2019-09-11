using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimilarColor : MonoBehaviour {

    public float difference;
    public float previousDifference;
    float smoothTime = 0.3f;
    float yVelocity = 0.0f;

    public float threshold;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        difference = FindObjectOfType<FaceComparer>().averageExpressionDifference / 100f;
        float smoothedDifference = Mathf.SmoothDamp(previousDifference, difference, ref yVelocity, smoothTime);
        GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green, Color.white, (1 - smoothedDifference) );
        previousDifference = smoothedDifference;

        if (FindObjectOfType<GameControl>().waiting && smoothedDifference > threshold)
            FindObjectOfType<GameControl>().Skip();
	}
}
