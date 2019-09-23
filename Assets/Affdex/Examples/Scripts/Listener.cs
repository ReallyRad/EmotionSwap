using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using extOSC;

public class Listener : ImageResultsListener
{
    public Text textArea;

    private Dictionary<int, Face> currentFaces;

    [Header("OSC Settings")]
    public OSCTransmitter Transmitter;

    public override void onFaceFound(float timestamp, int faceId)
    {
        Debug.Log("Found the face");
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        Debug.Log("Lost the face");
        FindObjectOfType<GameControl>().Skip();
    }
    
    public override void onImageResults(Dictionary<int, Face> faces)
    {
        currentFaces = faces;
        if (faces.Count > 0)
        {
            DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();
            if (dfv != null)
            {
                dfv.ShowFace(faces[0]);
            }

            foreach (Expressions expression in faces[0].Expressions.Keys)
            {
                float value;
                faces[0].Expressions.TryGetValue(expression, out value);

                var message = new OSCMessage("/face");

                message.AddValue(OSCValue.String(expression.ToString()));
                message.AddValue(OSCValue.Float(value));

                Transmitter.Send(message);
            }

            // Adjust font size to fit the selected platform.
            if ((Application.platform == RuntimePlatform.IPhonePlayer) ||
                (Application.platform == RuntimePlatform.Android))
            {
                textArea.fontSize = 36;
            }
            else
            {
                textArea.fontSize = 25;
            }

            textArea.text = faces[0].ToString();
            textArea.CrossFadeColor(Color.white, 0.2f, true, false);
        }
        else
        {
            textArea.CrossFadeColor(new Color(1, 0.7f, 0.7f), 0.2f, true, false);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(currentFaces != null)
            if (currentFaces.Count > 0)
                FindObjectOfType<FaceComparer>().ComputeFaceScore(currentFaces[0]);
    }
}