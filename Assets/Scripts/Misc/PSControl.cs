using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSControl : MonoBehaviour {

    public RaceObjBehave raceObjBehave;
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        ParticleSystem.EmissionModule emiss = ps.emission;
        emiss.rateOverTime = raceObjBehave.speed * .5f;
    }
}
