#define CAN_FALL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceObjBehave : MonoBehaviour {
    public GameObject racer;
    public float speed = 10;
    void AdjustRacerLocalPos() {
        MeshRenderer[] meshRenders = racer.GetComponentsInChildren<MeshRenderer>();
        float y = float.MinValue;
        for (int i = 0; i < meshRenders.Length; i++) {
            if (meshRenders[i].bounds.max.y > y) y = meshRenders[i].bounds.max.y;
        }
        racer.transform.localPosition = transform.up * (y * .5f);
    }
	void Start () {
    //    AdjustRacerLocalPos();
	}
    public enum State {
        idle,
        onTrack,
        falling
    }
    public State state;
	void Update () {
        
        Vector3 direction = transform.forward * Time.deltaTime * speed;
        Vector3 up = transform.up;
        Ray ray = new Ray(transform.position + (transform.up * 10) + transform.forward*racer.transform.localScale.z*2, -transform.up);
        Ray backRay = new Ray(transform.position + transform.up * 10 + transform.forward * racer.transform.localScale.z* - 1.5f, -transform.up);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * 10,Color.green);
        Debug.DrawRay(backRay.origin, backRay.direction * 10,Color.yellow);
#if CAN_FALL
        if (Physics.Raycast(ray, out hitInfo)||Physics.Raycast(backRay))
        {
            if (hitInfo.transform != null)
            {
#else
        if (Physics.Raycast(ray, out hitInfo) || Physics.Raycast(backRay, out hitInfo))
        {
#endif
            if (hitInfo.transform.gameObject.layer == 4)
                {
                    state = State.onTrack;
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.Rotate(Vector3.up * -100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                    }
                    Vector3 point = hitInfo.point;
                    transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * speed);
                    up = hitInfo.normal;
                }
            }
        }
        else {
            state = State.falling;
            direction += transform.up * -Time.deltaTime * 9.8f;
        }
        
        transform.position += direction;
        Vector3 forward = transform.forward;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward, up.normalized), Time.deltaTime * speed);
    }

    void StateMachine() {
        switch(state){
            case State.onTrack:
                break;
            case State.falling:
                break;
            case State.idle:
                break;
        }
    }
}
