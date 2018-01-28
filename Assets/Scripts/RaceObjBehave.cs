//#define CAN_FALL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Changed to a track magnet and lerp system")]
public class RaceObjBehave : MonoBehaviour {
    CharacterAnimator charAnimator;
    public GameObject racer;
    Material trackMaterial;
    public float speed = 10;
    public float accelorator = 2;
    void AdjustRacerLocalPos() {
        MeshRenderer[] meshRenders = racer.GetComponentsInChildren<MeshRenderer>();
        float y = float.MinValue;
        for (int i = 0; i < meshRenders.Length; i++) {
            if (meshRenders[i].bounds.max.y > y) y = meshRenders[i].bounds.max.y;
        }
        racer.transform.localPosition = transform.up * (y * .5f);
    }
	void Start () {
        charAnimator = GetComponentInChildren<CharacterAnimator>();
        trackMaterial = Resources.Load("Unlit_Spawn") as Material;
    //    AdjustRacerLocalPos();
	}
    public enum State {
        idle,
        onTrack,
        jump,
        falling
    }
    public State state;
    public float xbox;
    public float debugFakeHeight;
    public bool grounded;
	void Update () {
        //trackMaterial.SetFloat("_Location", transform.position.magnitude + 5);
        charAnimator.speed = speed*.25f;
        Vector3 direction = transform.forward * Time.deltaTime * speed;
        Vector3 up = transform.up;
        Ray ray = new Ray(transform.position + (transform.up * 10) + transform.forward*racer.transform.localScale.z*2, -transform.up);
        Ray backRay = new Ray(transform.position + transform.up * 10 + transform.forward * racer.transform.localScale.z* - 1.5f, -transform.up);
        
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * 10,Color.green);
        Debug.DrawRay(backRay.origin, backRay.direction * 10,Color.yellow);
        Ray groundRay = new Ray(transform.position+transform.up, transform.up * -2);
        Debug.DrawRay(groundRay.origin, groundRay.direction * 2, Color.cyan);

        RaycastHit groundHit;
        if (Physics.Raycast(groundRay, 2f))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (state != State.jump)
                {
                    direction += transform.up * 3;
                }
            }
            grounded = true;
        }
        else grounded = false;
#if CAN_FALL
        if (Physics.Raycast(ray, out hitInfo)||Physics.Raycast(backRay))
        {
            
#else
        if (Physics.Raycast(ray, out hitInfo) || Physics.Raycast(backRay, out hitInfo))
        {
#endif
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.gameObject.layer == 4)
                {
                    Vector3 point = hitInfo.point;
                    debugFakeHeight = (point-transform.position).magnitude;
                    state = State.onTrack;
                    
                    if (Input.GetKey(KeyCode.A)|| Input.GetAxis("Horizontal") < -.9f)
                    {
                        transform.Rotate(Vector3.up * -100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > .9f)
                    {
                        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") < -.9f)
                    {
                        if (speed > -3)
                        {
                            speed -= Time.deltaTime;
                        }
                    }
                    if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > .9f)
                    {
                        if (speed < 30)
                        {
                            speed += Time.deltaTime * accelorator;
                        }
                    }
                    else {
                        if (speed > 1) speed -= Time.deltaTime;
                    }
                 //   Vector3 point = hitInfo.point;
                    transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * speed);
                    up = hitInfo.normal;
                }

            }
        }
        else {
            state = State.falling;
            direction += transform.up * -Time.deltaTime * 9.8f;
            charAnimator.speed = 0;
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
