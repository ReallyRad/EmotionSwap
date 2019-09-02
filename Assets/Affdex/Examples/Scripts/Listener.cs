using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using extOSC;

public class Listener : ImageResultsListener
{
    public Text textArea;

    [Header("OSC Settings")]
    public OSCTransmitter Transmitter;

    public override void onFaceFound(float timestamp, int faceId)
    {
        Debug.Log("Found the face");
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        Debug.Log("Lost the face");
    }
    
    public override void onImageResults(Dictionary<int, Face> faces)
    {
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

                var message = new OSCMessage(expression.ToString());
                message.AddValue(OSCValue.Float(value));
                

                Transmitter.Send(message);
            }

            float anger;
            faces[0].Emotions.TryGetValue(Emotions.Anger, out anger);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(0, anger);

            float fear;
            faces[0].Emotions.TryGetValue(Emotions.Fear, out fear);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(1, fear);

            float disgust;
            faces[0].Emotions.TryGetValue(Emotions.Disgust, out disgust);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(2, disgust);

            float joy;
            faces[0].Emotions.TryGetValue(Emotions.Joy, out joy);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(3, joy);

            float surprise;
            faces[0].Emotions.TryGetValue(Emotions.Surprise, out surprise);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(4, surprise);

            float sadness;
            faces[0].Emotions.TryGetValue(Emotions.Sadness, out sadness);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(5, sadness*5);


            // Adjust font size to fit the selected platform.
            if ((Application.platform == RuntimePlatform.IPhonePlayer) ||
                (Application.platform == RuntimePlatform.Android))
            {
                textArea.fontSize = 36;
            }
            else
            {
                textArea.fontSize = 12;
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
	void Update () {
	
	}
}