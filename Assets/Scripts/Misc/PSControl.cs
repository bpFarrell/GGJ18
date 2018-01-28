using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSControl : MonoBehaviour {

    public float rateToSpeed = .5f;
    public TrackMagnet raceObjBehave;
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        ParticleSystem.EmissionModule emiss = ps.emission;
        emiss.rateOverTime = raceObjBehave.currentSpeed * rateToSpeed;
    }
}
