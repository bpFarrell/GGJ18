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

    List<TrackMagnet> players       = new List<TrackMagnet>();
    public List<int> controllerIDs  = new List<int>();
    float setupCountdown = 20;
    bool beginCountdown;
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
        for (int i = 0; i < 4; i++)
        {
            if (ReInput.players.GetPlayer(i) != null && ReInput.players.GetPlayer(i).GetAnyButtonDown())
            {
                beginCountdown = true;
                if (controllerIDs.Contains(i)) controllerIDs.Remove(i);
                else {
                    controllerIDs.Add(i);
                }
            }
        }
        if (controllerIDs.Count == 0) {
            beginCountdown = false;
            setupCountdown = 20;
        }
        
    }
}
