using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimilarColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {        
        GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green, Color.white, 1 - FindObjectOfType<FaceComparer>().averageExpressionDifference/100f);
	}
}
