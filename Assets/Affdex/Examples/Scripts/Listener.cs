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

            float anger;
            faces[0].Emotions.TryGetValue(Emotions.Anger, out anger);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(0, anger);
            FindObjectOfType<ControlVolumeEmotions>().anger.volume = anger / 100;
            FindObjectOfType<ControlVolumeEmotions>().angerRenderer.material.color = Color.Lerp(Color.black, Color.white, anger / 100);

            float fear;
            faces[0].Emotions.TryGetValue(Emotions.Fear, out fear);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(1, fear);
            FindObjectOfType<ControlVolumeEmotions>().fear.volume = fear/100;
            FindObjectOfType<ControlVolumeEmotions>().fearRenderer.material.color = Color.Lerp(Color.black, Color.white, fear / 100);

            float disgust;
            faces[0].Emotions.TryGetValue(Emotions.Disgust, out disgust);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(2, disgust);
            FindObjectOfType<ControlVolumeEmotions>().disgust.volume = disgust / 100;
            FindObjectOfType<ControlVolumeEmotions>().disgustRenderer.material.color = Color.Lerp(Color.black, Color.white, disgust / 100);


            float joy;
            faces[0].Emotions.TryGetValue(Emotions.Joy, out joy);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(3, joy);
            FindObjectOfType<ControlVolumeEmotions>().happiness.volume = joy / 100;
            FindObjectOfType<ControlVolumeEmotions>().happinessRenderer.material.color = Color.Lerp(Color.black, Color.white, joy / 100);

            float surprise;
            faces[0].Emotions.TryGetValue(Emotions.Surprise, out surprise);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(4, surprise);
            FindObjectOfType<ControlVolumeEmotions>().surprise.volume = surprise / 100;
            FindObjectOfType<ControlVolumeEmotions>().surpriserenderer.material.color = Color.Lerp(Color.black, Color.white, surprise / 100);

            float sadness;
            faces[0].Emotions.TryGetValue(Emotions.Sadness, out sadness);
            FindObjectOfType<AudioFeedback>().SetEmotionLevel(5, sadness*5);
            FindObjectOfType<ControlVolumeEmotions>().sadness.volume = sadness / 100;
            FindObjectOfType<ControlVolumeEmotions>().sadnessRenderer.material.color = Color.Lerp(Color.black, Color.white, sadness / 100);


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
	void LateUpdate () {
        if (currentFaces.Count > 0)
            FindObjectOfType<FaceComparer>().ComputeFaceScore(currentFaces[0]);
    }
}