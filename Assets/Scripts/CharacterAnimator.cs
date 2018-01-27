using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {
    public GameObject sheep;
    public GameObject horns;
    Material sheepMat;
    Material hornsMat;
    const float hornOffset = 0.44f;
    float animTime;
    public float animSpeed;
    public float jumping;
    public float speed;
	// Use this for initialization
	void Awake () {
        sheepMat = sheep.GetComponent<MeshRenderer>().material;
        hornsMat = horns.GetComponent<MeshRenderer>().material;
    }
	float GetYPos(float animTime,float jumping) {

        return Mathf.Abs(Mathf.Sin(hornOffset+animTime)) * jumping;
    }
	void Update () {
        jumping = speed;
        animSpeed = speed * 5 + 2;


        animTime += Time.deltaTime*animSpeed;
        sheepMat.SetFloat("_Offset", animTime);
        sheepMat.SetFloat("_Jumping", jumping);
        Vector3 tempVec = horns.transform.localPosition;
        tempVec.y = GetYPos(animTime, jumping);
        horns.transform.localPosition = tempVec;
	}
}
