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

    private Expressions[] expressionsUsed = {   Expressions.Smile,
                                                Expressions.BrowRaise,
                                                Expressions.BrowFurrow,
                                                Expressions.NoseWrinkle,
                                                Expressions.UpperLipRaise,
                                                Expressions.ChinRaise,
                                                Expressions.LipPucker,
                                                Expressions.LipPress,
                                                Expressions.MouthOpen,
                                                Expressions.Smirk};
    private float[] expressionsValues; 

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
                if (key == expressionsUsed[i].ToString())
                {
                    expressionsValues[i] = floatVal;
                    break;
                }
                else Debug.Log("key not found");
            }
        }
    }

    public void ComputeFaceScore(Face otherface)
    {
        float differenceSum = 0;

        for(int i=0; i<expressionsUsed.Length; i++)
        {
            float val;
            if (otherface.Expressions.TryGetValue(expressionsUsed[i], out val))
                differenceSum += val;
        }

        expressionSimilarity = differenceSum / expressionsUsed.Length;
    }

    #endregion
}
