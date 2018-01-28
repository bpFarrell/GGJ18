using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void AddPlayer(int id, GameObject playerSet) {
        TrackMagnet trackMagnet = playerSet.GetComponentInChildren<TrackMagnet>();
        trackMagnet.playerID = id;
        trackMagnet.AssignController(id);
        trackMagnet.init = true;
        players.Add(trackMagnet);
    }
}
