using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsRandomizer : MonoBehaviour {
    private CharacterAnimator characterAnimator;
    public List<AudioClip> stepList = new List<AudioClip>();
    public AudioSource stepAudio;
    public float delayToPlay;

    void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
    }
    
    void Start ()
    {
        // StartCoroutine(stepDelay());
	}
	
    void Update ()
    {
        if (characterAnimator.GetYPos(characterAnimator.animTime, characterAnimator.jumping) <= 0.1f*characterAnimator.speed)
        {
            stepAudio.clip = stepList[Random.Range(0, 5)];
            stepAudio.Play();
        }
    }

    IEnumerator stepDelay()
    {
        yield return new WaitForSeconds(delayToPlay);
        delayToPlay = characterAnimator.speed;


        PlaySteps();
    }

	void PlaySteps ()
    {

        StartCoroutine(stepDelay());
    }
}
