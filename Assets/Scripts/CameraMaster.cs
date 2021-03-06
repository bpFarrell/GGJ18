﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaster : MonoBehaviour {
    public Camera[] playerCams;
    public GameObject[] playerSets;
    public GameObject playerSet;
    [ColorUsage(true,true,0,8,0,8)]
    public Color[] playerColors;
    public int playerCount = 1;
    public static CameraMaster instance;
    public static int currentPlayerSetup = -1;
    public bool updateCamera;
    void SetupCamera(List<int> listController) {
        if(playerSets!=null)
            for(int x = 0; x < playerSets.Length; x++) {
                Destroy(playerSets[x]);
            }
        playerCams = new Camera[playerCount];
        playerSets = new GameObject[playerCount];
        for(int x = 0; x < listController.Count; x++) {
            currentPlayerSetup = x;
            playerSets[x] = Instantiate(playerSet);
            Vector3 point = SplineLogic.Spline.instance.EvaluatePosition(0.1f);
            Transform trans = playerSets[x].transform;
            trans.rotation = SplineLogic.Spline.instance.EvaluateRotation(0.1f);
            trans.position = point + trans.right * (x*2 - 1.5f);

            playerCams[x] = playerSets[x].GetComponentInChildren<Camera>();
            ControllerManager.instance.AddPlayer(x, playerSets[x]);
        }
        currentPlayerSetup = -1;
        switch (playerCount) {
            case 2:
                playerCams[0].enabled = true;
                playerCams[0].rect = new Rect(0, 0, 1, 0.5f);
                playerCams[1].enabled = true;
                playerCams[1].rect = new Rect(0, 0.5f, 1, 1);
                break;
            case 3:

                playerCams[0].enabled = true;
                playerCams[0].rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
                playerCams[1].enabled = true;
                playerCams[1].rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
                playerCams[2].enabled = true;
                playerCams[2].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
                break;
            case 4:
                playerCams[0].enabled = true;
                playerCams[0].rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
                playerCams[1].enabled = true;
                playerCams[1].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
                playerCams[2].enabled = true;
                playerCams[2].rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
                playerCams[3].enabled = true;
                playerCams[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;
            default:
                playerCams[0].enabled = true;
                playerCams[0].rect = new Rect(0, 0, 1, 1);
                break;
        }
    }
    private void Awake() {
        instance = this;
    }
    public void Initialize (List<int> listController) {
        this.playerCount = listController.Count;
        SetupCamera(listController);
	}
    private void Update() {
        if (updateCamera) {
            updateCamera = false;
        //    SetupCamera();
        }
    }
}
