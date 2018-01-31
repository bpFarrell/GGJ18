using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepFallSound : MonoBehaviour {

    private TrackMagnet fallChecker;
    public AudioSource fallSound;
    public List<AudioClip> fallSoundList = new List<AudioClip>();

    // Use this for initialization
    void Start () {
        fallSound.clip = fallSoundList[Random.Range(0,1)];
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > fallChecker.fallTime + 2)
        {
            fallSound.Play();
        }
        if (fallSound.isPlaying)
        {
            fallSound.clip = fallSoundList[Random.Range(0, 1)];
        }
    }
}
