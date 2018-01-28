#define CAN_FALL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TrackMagnet : MonoBehaviour
{
    public int playerID;
    public bool init;
    public enum StartupState {
        idle,
        ready
    }
    public StartupState startupState;
    public enum TrackingState
    {
        idle,
        onTrack,
        jump,
        falling
    }
    public TrackingState trackingState;
    public float speedMAX       = 30;
    public float speedNORM      = 20;
    public float speedMIN       = -2;
    public float currentSpeed   = 0;
    public float accelerator    = 10;
    public float deccelerator   = 15;
    public float jumpHeight     = 15;
    public bool isGrounded;
    public CharacterAnimator animator;
    public SheepTracking sheepTracking;

    public Vector3 fallPos;
    public Vector3 fallDir;
    public float fallTime;

    public float axisHorizontal;
    public float axisVertical;
    public bool jump;

    Player playerController;
    void Controller() {
        axisHorizontal  = ReInput.players.GetPlayer(playerID).GetAxis("AxisHorizontal");//Input.GetAxis("Horizontal");
        axisVertical    = ReInput.players.GetPlayer(playerID).GetAxis("AxisVertical"); Input.GetAxis("Vertical");
        jump            = ReInput.players.GetPlayer(playerID).GetButtonDown("Action2");//Input.GetButtonDown("Fire1");
    }
    public void AssignController(int id)
    {
        playerController = ReInput.players.GetPlayer(id);
    }
    void Update()
    {
        if(init) Controller();

        // Quick hack to wait for 3..2..1.. ready
        if (jump)
        {
            if (startupState == StartupState.idle) startupState = StartupState.ready;
        }
        if (startupState == StartupState.idle) return;
        animator.speed = currentSpeed * .1f;
        Vector3 direction   = transform.forward * Time.deltaTime * currentSpeed;
        Vector3 up          = transform.up;
        Ray ray             = new Ray(transform.position + (transform.up * 10), -transform.up);
        Ray backRay         = new Ray(transform.position + transform.up * 10 + transform.forward * -1.5f, -transform.up);
        Ray frontRay        = new Ray(transform.position + transform.up, transform.forward);
        Ray groundRay       = new Ray(transform.position + transform.up, transform.up * -2);
        Ray obstacleRay     = new Ray(transform.position + transform.up, transform.forward * 2);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        Debug.DrawRay(backRay.origin, backRay.direction * 10, Color.yellow);
        Debug.DrawRay(groundRay.origin, groundRay.direction * 2, Color.cyan);
        Debug.DrawRay(obstacleRay.origin, obstacleRay.direction * 2, Color.red);

        RaycastHit obstacleHitinfo;
        if (Physics.Raycast(obstacleRay, out obstacleHitinfo,2)) {
            if (obstacleHitinfo.transform.gameObject.layer == 8) {
                Debug.Log("Hit obstacle : "+ obstacleHitinfo.transform.name);

                if(currentSpeed > 2) currentSpeed = 2;
            }
        }
        if (Physics.Raycast(groundRay, 2f))
        {
            if (jump)
            {
                if (startupState == StartupState.idle) startupState = StartupState.ready;
                if (trackingState == TrackingState.onTrack)
                {
                    trackingState = TrackingState.jump;
                    direction += transform.up * jumpHeight;
                }
            }
            isGrounded = true;
        }
        else isGrounded = false;

        RaycastHit hitInfo;
#if CAN_FALL
        if (Physics.Raycast(ray, out hitInfo) || Physics.Raycast(frontRay, out hitInfo,2) ||Physics.Raycast(backRay))
        {

#else
        if (Physics.Raycast(ray, out hitInfo) || Physics.Raycast(frontRay, out hitInfo,2) || Physics.Raycast(backRay, out hitInfo))
        {
#endif
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.gameObject.layer == 4)
                {
                    Vector3 point = hitInfo.point;
                    trackingState = TrackingState.onTrack;

                    if (Input.GetKey(KeyCode.A) || axisHorizontal < -.9f)
                    {
                        transform.Rotate(Vector3.up * -100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.D) || axisHorizontal > .9f)
                    {
                        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.S) || axisVertical < -.9f)
                    {
                        if (currentSpeed > speedMIN)
                        {
                            currentSpeed -= Time.deltaTime * deccelerator;
                        }
                    }
                    if (Input.GetKey(KeyCode.W) || axisVertical > .9f)
                    {
                        if (currentSpeed < speedMAX)
                        {
                            currentSpeed += Time.deltaTime * accelerator;
                        }
                    }
                    else
                    {
                        if (currentSpeed < speedNORM) currentSpeed += Time.deltaTime*accelerator;
                    }
                    //   Vector3 point = hitInfo.point;
                    transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime*currentSpeed);
                    up = hitInfo.normal;
                }

            }
        }
        else
        {
            if (trackingState != TrackingState.falling) {
                fallPos = sheepTracking.latestSlab.transform.position;
                fallDir = sheepTracking.latestSlab.transform.forward;
                fallTime = Time.time;

            }
            trackingState = TrackingState.falling;
            direction += transform.up * -Time.deltaTime * 9.8f;
            animator.speed = -1f;

            if (Time.time > fallTime + 3) {
                transform.position = fallPos;
                transform.forward = fallDir;
                fallTime = float.MaxValue;
                currentSpeed = 0;
            }
            
        }

        transform.position += direction;
        Vector3 forward = transform.forward;
        transform.rotation = Quaternion.LookRotation(forward, up.normalized);
    }

}
