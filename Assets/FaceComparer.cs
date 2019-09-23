using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using Affdex; 

public class FaceComparer : MonoBehaviour {
    #region Public Vars

    public float averageExpressionDifference;

    //https://developer.affectiva.com/mapping-expressions-to-emotions/

    public string Address = "/face";

    public Face myFace; 

    [Header("OSC Settings")]
    public OSCReceiver Receiver;

    public Expressions[] expressionsUsed = {   Expressions.Smile,
                                                Expressions.BrowRaise,
                                                Expressions.BrowFurrow,
                                                Expressions.NoseWrinkle,
                                                Expressions.UpperLipRaise,
                                                Expressions.ChinRaise,
                                                Expressions.LipPress,
                                                Expressions.MouthOpen,
                                                Expressions.Smirk,
                                                Expressions.InnerBrowRaise,
                                                Expressions.LipCornerDepressor,
                                                };
    public float[] expressionsValues; //expression values for the other face

    public float threshold;

    [SerializeField] private float detectedExpressions;
    [SerializeField] private float matchingExpressions;
    #endregion

    #region Unity Methods

    protected virtual void Start()
    {
       
        Receiver.Bind(Address, ReceivedMessage);
        expressionsValues = new float[expressionsUsed.Length];
    }

    #endregion

    #region Private Methods

    private void ReceivedMessage(OSCMessage message)
    {
        string key;
        float floatVal;

        if (message.ToString(out key) && message.ToFloat(out floatVal))
        {
            for (int i = 0; i < expressionsUsed.Length; i++)
            {
                if (key == expressionsUsed[i].ToString()) expressionsValues[i] = floatVal;
            }
        }
    }

    public void ComputeFaceScore(Face myFace) //called everytime new face information is available. parameter is local face
    {
        float differenceSum = 100; //equivalent to all expressions being completely different
        detectedExpressions = 0;
        matchingExpressions = 0;
        

        for(int i=0; i<expressionsUsed.Length; i++)
        {
            float val; // value of local face expression at index i
            if (myFace.Expressions.TryGetValue(expressionsUsed[i], out val)) //if the expression is used in the computation
            {
                FindObjectOfType<ExpressionsPanelBehavior>()._expressionsImages[i].color = Color.clear;


                if (val > threshold && expressionsValues[i] > threshold) //if expression ON on both faces
                {
                    detectedExpressions++;
                    matchingExpressions++;
                    FindObjectOfType<ExpressionsPanelBehavior>()._expressionsImages[i].color = Color.white;

                }
                else if (val > threshold) { //if expression on local face only
                    detectedExpressions++;
                    FindObjectOfType<ExpressionsPanelBehavior>()._expressionsImages[i].color = Color.white;

                }
                else if (expressionsValues[i] > threshold) //if expression on other face only
                {
                    detectedExpressions++;  
                    /*
                    LeanTween.value(0, 1, 0.5f).setOnUpdate((float tweenValue) =>
                    {
                        FindObjectOfType<ExpressionsPanelBehavior>()._expressionsImages[i].color = Color.Lerp(Color.clear, Color.white, tweenValue);
                    });*/

                }
            }

        }

        if (detectedExpressions > 0.01f)
            averageExpressionDifference = matchingExpressions / detectedExpressions * 100; //ponderate the difference by the number of tracked expressions
        else
            averageExpressionDifference = 100;
    }

    #endregion
}
