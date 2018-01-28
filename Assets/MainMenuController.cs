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

public class MainMenuController : MonoBehaviour
{
    public delegate void ReadyDelegate(bool state);
    public delegate void StartDelegate(int[] players);
    public class PlayerReadyUp
    {
        public bool isReady = false;
        public ReadyDelegate OnReadyChanged;
        public void Input()
        {
            isReady = !isReady;
            if (OnReadyChanged != null) OnReadyChanged(isReady);
        }
        public void Input(bool b)
        {
            isReady = b;
            if (OnReadyChanged != null) OnReadyChanged(isReady);
        }
        public PlayerReadyUp(Image image)
        {
            OnReadyChanged += ((b) => {
                if (b)
                    image.color = Color.green;
                else
                    image.color = Color.red;
            });
            Input(false);
        }
        public void Update()
        {
            // check for input and call Input if detected;
        }
    }
    public List<PlayerReadyUp> players = new List<PlayerReadyUp>();
    public Image[] iconReferences;
    public StartDelegate OnStart;

    private void Start()
    {
        for (int i = 0; i < iconReferences.Length; i++)
        {
            players.Add(new PlayerReadyUp(iconReferences[i]));
        }
    }
    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Update();
        }
        if(false)
        {
            //if (OnStart != null) OnStart();
        }
    }
}