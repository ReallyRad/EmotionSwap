using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using Affdex; 

public class FaceComparer : MonoBehaviour {
    #region Public Vars

    public float expressionSimilarity;

    //https://developer.affectiva.com/mapping-expressions-to-emotions/

    public string Address = "/face";

    public Face myFace; 

    [Header("OSC Settings")]
    public OSCReceiver Receiver;

    private Expressions[] expressionsUsed = {   //Expressions.Smile,
                                                //Expressions.BrowRaise,
                                                //Expressions.BrowFurrow,
                                                //Expressions.NoseWrinkle,
                                                //Expressions.UpperLipRaise,
                                                //Expressions.ChinRaise,
                                                //Expressions.LipPucker,
                                                //Expressions.LipPress,
                                                //Expressions.MouthOpen
                                                Expressions.Smirk};
    private float[] expressionsValues; //expression values for the other face

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

    public void ComputeFaceScore(Face myFace) //called everytime new face information is available
    {
        float differenceSum = 0; //equivalent to all expressions being completely different
        for(int i=0; i<expressionsUsed.Length; i++)
        {
            float val;
            if (myFace.Expressions.TryGetValue(expressionsUsed[i], out val)) //if the expression is used in the computation
            {
                differenceSum += Mathf.Abs(val - expressionsValues[i]); //add the difference to the current sum.
                Debug.Log("difference for " + expressionsUsed[i] + " : " + Mathf.Abs(val - expressionsValues[i]));
            }
        }

        expressionSimilarity = differenceSum / expressionsUsed.Length; //ponderate the difference by the number of tracked expressions
    }

    #endregion
}
