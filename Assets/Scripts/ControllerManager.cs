using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerManager : MonoBehaviour {
    private static ControllerManager _instance;
    public static ControllerManager instance {
        get {
            return _instance;
        }
    }
    void OnEnable()
    {
        _instance = this;
        Debug.Log(instance);
    }

    List<TrackMagnet> players = new List<TrackMagnet>();
    public List<int> controllerIDs = new List<int>();
    public float setupCountdown = 5;
    public bool beginCountdown;
    public bool gameBegan;
    public delegate void ControllerSetupComplete();
    public ControllerSetupComplete onControllSetupComplete;

    public void AddPlayer(int id, GameObject playerSet) {
        TrackMagnet trackMagnet = playerSet.GetComponentInChildren<TrackMagnet>();
        trackMagnet.playerID = id;
        trackMagnet.controllerID = controllerIDs[id];
        trackMagnet.init = true;
        players.Add(trackMagnet);
    }
    public void Update()
    {
        if (!ReInput.isReady) return;
        if (gameBegan) return;

        for (int i = 0; i < 4; i++)
        {
            if ((ReInput.players.GetPlayer(i) != null && ReInput.players.GetPlayer(i).GetAnyButtonDown())|| 
                (controllerIDs.Count == 0 && Input.GetButtonDown("Fire1")))
            {
                beginCountdown = true;
                if (controllerIDs.Contains(i)) controllerIDs.Remove(i);
                else {
                    controllerIDs.Add(i);
                }
                Debug.Log(controllerIDs.Count +": players join, controller == " + i);
            }
        }
        if (beginCountdown) {
            setupCountdown -= Time.deltaTime;
            if (setupCountdown <= 0.1f) {
                CameraMaster.instance.Initialize(controllerIDs);
                CountDown.SetAndCount();
                if (onControllSetupComplete != null) onControllSetupComplete();
                gameBegan = true;
            }
        }
        if (controllerIDs.Count == 0) {
            beginCountdown = false;
            setupCountdown = 5;
        }
        Debug.Log(setupCountdown);
    }
}
