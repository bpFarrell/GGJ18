using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMagnet : MonoBehaviour
{
    public enum State
    {
        idle,
        onTrack,
        jump,
        falling
    }
    public State state;
    public float speedMAX       = 30;
    public float speedNORM      = 20;
    public float speedMIN       = -2;
    public float currentSpeed   = 0;
    public float accelerator    = 10;
    public float deccelerator   = 15;
    public float jumpHeight     = 15;
    public bool isGrounded;
    void Update()
    {
        Vector3 direction   = transform.forward * Time.deltaTime * currentSpeed;
        Vector3 up          = transform.up;
        Ray ray             = new Ray(transform.position + (transform.up * 10), -transform.up);
        Ray backRay         = new Ray(transform.position + transform.up * 10 + transform.forward * -1.5f, -transform.up);
        Ray groundRay       = new Ray(transform.position + transform.up, transform.up * -2);

        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        Debug.DrawRay(backRay.origin, backRay.direction * 10, Color.yellow);
        Debug.DrawRay(groundRay.origin, groundRay.direction * 2, Color.cyan);

        if (Physics.Raycast(groundRay, 2f))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (state == State.onTrack)
                {
                    state = State.jump;
                    transform.position += transform.up * jumpHeight;
                }
            }
            isGrounded = true;
        }
        else isGrounded = false;
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
                    state = State.onTrack;

                    if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -.9f)
                    {
                        transform.Rotate(Vector3.up * -100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > .9f)
                    {
                        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") < -.9f)
                    {
                        if (currentSpeed > speedMIN)
                        {
                            currentSpeed -= Time.deltaTime * deccelerator;
                        }
                    }
                    if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > .9f)
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
                    transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime);
                    up = hitInfo.normal;
                }

            }
        }
        else
        {
            state = State.falling;
            direction += transform.up * -Time.deltaTime * 9.8f;
        }

        transform.position += direction;
        Vector3 forward = transform.forward;
        transform.rotation = Quaternion.LookRotation(forward, up.normalized);
    }

}
