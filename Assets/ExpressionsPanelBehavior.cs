using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Affdex;
using UnityEngine.UI;

public class ExpressionsPanelBehavior : MonoBehaviour
{

    private FaceComparer _faceComparer;
    public Image[] _expressionsImages;

    // Use this for initialization
    void Start()
    {
        _faceComparer = FindObjectOfType<FaceComparer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 11; i++)
        {
            //_expressionsImages[i].color = Color.Lerp(Color.clear, Color.white, _faceComparer.expressionsValues[i] / 100f); //other face
                                                                                                                               //colau face    
            float val; // value of local face expression at index i
          /*  if (_faceComparer.myFace.Expressions.TryGetValue(_faceComparer.expressionsUsed[i], out val)) //if the expression is used in the computation
                _expressionsImages[i].color = Color.Lerp(Color.clear, Color.white, val / 100f);
                */
        }
    }
}
