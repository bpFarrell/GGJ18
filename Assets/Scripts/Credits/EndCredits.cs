/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCredits : MonoBehaviour {
    public RectTransform credits;
    public float speed;

    private bool isDone = false;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isDone) return;
        credits.anchoredPosition += Vector2.up * (Time.deltaTime * speed);
        if(credits.anchoredPosition.y >= 0f)
        {
            CreditsOver();
            isDone = true;
        }
    }

    private void CreditsOver()
    {
        //throw new NotImplementedException();
    }
}