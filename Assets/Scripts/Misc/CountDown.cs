/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
    enum CountDownState
    {
        OFF,
        DELAY,
        ENTER,
        PAUSE,
        EXIT
    }
    public List<Sprite> sprites;

    private CountDownState state = CountDownState.OFF;
    private AudioSource audioSource;
    private float transitionSeconds = .218f;
    private float pauseSeconds = .1f;
    private float delayInSeconds = 0.5f;

    public delegate void onGoDelegate();
    public static onGoDelegate OnGo;

    private Image img;
    private int index = 0;
    private float t;
    private Vector3 open = new Vector3(0,0,0);
    private Vector3 closed = new Vector3(90f, 0, 0);

    private void Start()
    {
        img = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        //SetAndCount();
    }
    public void SetAndCount()
    {
        index = 0;
        t = 0f;
        img.rectTransform.sizeDelta = new Vector2(256, 256);
        state = CountDownState.DELAY;
        img.sprite = sprites[index];
        
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case CountDownState.OFF:
                break;
            case CountDownState.DELAY:
                t += Time.deltaTime / delayInSeconds;
                if(t >= 1f)
                {
                    t = 0f;
                    state = CountDownState.ENTER;
                }
                break;
            case CountDownState.ENTER:
                t += Time.deltaTime / transitionSeconds;
                transform.eulerAngles = Vector3.Lerp(closed, open, t);
                if(t >= 1)
                {
                    if(index == 0)
                    {
                        audioSource.Play();
                    }
                    t = 0f;
                    state = CountDownState.PAUSE;
                }
                break;
            case CountDownState.PAUSE:
                t += Time.deltaTime / pauseSeconds;
                transform.eulerAngles = open;
                if(t >= 1)
                {
                    t = 0f;
                    state = CountDownState.EXIT;
                }
                break;
            case CountDownState.EXIT:
                t += Time.deltaTime / transitionSeconds;
                transform.eulerAngles = Vector3.Lerp(open, -closed, t);
                if (t >= 1)
                {
                    t = 0f;
                    ++index;
                    if (index >= sprites.Count)
                    {
                        state = CountDownState.OFF;
                        if (OnGo != null) OnGo();
                    }
                    else
                    {
                        img.sprite = sprites[index];
                        if (index == 3) img.rectTransform.sizeDelta = new Vector2(512, 256);
                        else img.rectTransform.sizeDelta = new Vector2(256, 256);
                        state = CountDownState.ENTER;
                    }
                }
                break;
            default:
                break;
        }
    }

}