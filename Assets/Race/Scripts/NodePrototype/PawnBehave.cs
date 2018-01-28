using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBehave : MonoBehaviour {

    NodeManager.Node destinationNode;
    private Vector3 _destination;
    public Vector3 destination {
        set {
            state = State.enroute;
            _destination = value;
        }
    }
    public void SetDestination(NodeManager.Node node) {
        destinationNode = node;
        destination = node.position;
    }
    public enum State{
        idle,
        enroute,
        done
    }
    public State state;
    public void Update()
    {
        if (state == State.enroute) {
            transform.LookAt(_destination);
            transform.position += transform.forward * Time.deltaTime * 10;
            //transform.position = Vector3.Lerp(transform.position, _destination, Time.deltaTime);
            if ((transform.position - _destination).magnitude < 0.1f) {
                state = State.done;
                destinationNode.nodeTrans.GetComponent<MeshRenderer>().material.color = Color.red;
                transform.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
