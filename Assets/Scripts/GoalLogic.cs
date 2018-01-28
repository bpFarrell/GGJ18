using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour {
    Vector3 startPos;
    public float spinSpeed = 1;
    public float bounceSpeed = 1;
    public static int[] finishOrder;
    public static GoalLogic instance;
    public float scoreDistance = 10;
    // Use this for initialization
    private void Awake() {
        instance = this;
    }
	void Start () {
        finishOrder = new int[]{ -1, -1, -1, -1 };
        float lastPoint = SplineLogic.Spline.instance.NodesCount-0.1f;
        Vector3 pos = SplineLogic.Spline.instance.EvaluatePosition(lastPoint);
        pos += SplineLogic.Spline.instance.EvaluateRotation(lastPoint) * new Vector3(0, 2, 15);
        transform.rotation = SplineLogic.Spline.instance.EvaluateRotation(lastPoint);
        transform.position = pos;
        startPos = pos;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time*bounceSpeed);
        transform.Rotate(Time.deltaTime * spinSpeed,0 , 0);
	}
    public void CheckDistance(Vector3 pos,TrackMagnet magnet) {
        if (Vector3.Distance(pos, transform.position) < scoreDistance) {
            for(int x = 0; x < finishOrder.Length; x++) {
                if (finishOrder[x] == magnet.playerID) return;
                if (finishOrder[x] == -1) {
                    finishOrder[x] = magnet.playerID;
                    break;
                }
            }
            magnet.trackingState = TrackMagnet.TrackingState.finished;
            Debug.Log("HIITTTTT!!!!");
        }
    }
}
