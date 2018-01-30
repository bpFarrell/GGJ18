using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public TrackMagnet magnetPlayer;
    public float distBack = 7;
    public float distUp = 8;
    public Transform racer;
    public float speed = 7;
    Vector3 anchor {
        get {
            return racer.position + (racer.forward * -distBack) + (racer.up * distUp);
        }
    }
    enum FollowState {
        player,
        slab
    }
    FollowState followState;
    int slabIndex;
    Vector3 newAnchor;
    public float prefDistance = 5;
    void Update () {
    if (magnetPlayer.trackingState != TrackMagnet.TrackingState.onTrack)
        {
            Vector3 modifiedAnchor = racer.position + (racer.forward * -distBack) + magnetPlayer.sheepTracking.latestSlab.transform.up * distUp;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(((racer.position + racer.forward * 5) - transform.position), Vector3.up), Time.deltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, modifiedAnchor, Time.deltaTime * speed);
        }
       else {
           transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(((racer.position + racer.forward * 5) - transform.position), Vector3.up), Time.deltaTime * speed);
           transform.position = Vector3.Lerp(transform.position, anchor, Time.deltaTime * speed);
    
       }

    }
    
}
