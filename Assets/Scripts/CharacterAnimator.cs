using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {
    public GameObject sheep;
    public GameObject bow;
    public GameObject eyes;
    public GameObject horns;
    Material sheepMat;
    Material hornsMat;
    const float hornOffset = 0.44f;
    public float animTime;
    public float animSpeed;
    public float jumping;
    public float speed;
    private float hornScale=2;
    public bool isSpoofed;
    public int spoofPlace;
    void Awake () {
        sheepMat = sheep.GetComponent<MeshRenderer>().material;
        sheepMat = new Material(sheepMat);
        sheep.GetComponent<MeshRenderer>().material = sheepMat;
        bow.GetComponent<MeshRenderer>().material = sheepMat;
        eyes.GetComponent<MeshRenderer>().material = sheepMat;
        hornsMat = horns.GetComponent<MeshRenderer>().material;
        hornsMat = new Material(hornsMat);
        horns.GetComponent<MeshRenderer>().material = hornsMat;
        if (!isSpoofed) {
            sheepMat.SetColor("_Color", CameraMaster.instance.playerColors[CameraMaster.currentPlayerSetup]);
            hornsMat.SetColor("_Color", CameraMaster.instance.playerColors[CameraMaster.currentPlayerSetup]);
        }else {
            if (spoofPlace >= CameraMaster.instance.playerCount) {
                Destroy(gameObject);
            }
            sheepMat.SetColor("_Color", CameraMaster.instance.playerColors[GoalLogic.finishOrder[spoofPlace]]);
            hornsMat.SetColor("_Color", CameraMaster.instance.playerColors[GoalLogic.finishOrder[spoofPlace]]);
        }
    }
	public float GetYPos(float animTime,float jumping) {

        return Mathf.Abs(Mathf.Sin(hornOffset+animTime)) * jumping;
    }
	void Update () {
        if (isSpoofed) {
            transform.position += transform.forward * 10 * Time.deltaTime;

        }
        jumping = speed;
        animSpeed = speed * 5 + 2;


        animTime += Time.deltaTime*animSpeed;
        sheepMat.SetFloat("_Offset", animTime);
        sheepMat.SetFloat("_Jumping", jumping);
        hornsMat.SetFloat("_T", speed * hornScale);
        Vector3 tempVec = horns.transform.localPosition;
        tempVec.y = GetYPos(animTime, jumping);
        horns.transform.localPosition = tempVec;
	}
}
