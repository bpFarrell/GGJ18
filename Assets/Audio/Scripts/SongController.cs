using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SongController : MonoBehaviour {
    public AudioMixer Music;
    public List<AudioClip> songList = new List<AudioClip>();
    public AudioSource songPlaying;
    public float titleHigh = 180;
    public float titleLow = 3500;
    public float gameplayHigh = 10;
    public float gameplayLow = 22000;
    private MainMenuController gameplayState;

    void Start()
    {
        gameplayState = GetComponent<MainMenuController>();
        ControllerManager.instance.onControllSetupComplete += isPlaying;
        GoalLogic.OnLastFinish += creditsPlay;
        SetMusicLow(titleLow);
        SetMusicHigh(titleHigh);
        songPlaying.clip = songList[0];
        songPlaying.loop = true;
        songPlaying.Play();
    }

    private void creditsPlay()
    {
        SetMusicLow(titleLow);
        SetMusicHigh(titleHigh);
    }

    private void isPlaying()
    {
        SetMusicLow(gameplayLow);
        SetMusicHigh(gameplayHigh);
        songPlaying.clip = songList[1];
        songPlaying.loop = true;
        songPlaying.Play();
    }

    void SetMusicLow(float MusicLow)
    {
        Music.SetFloat("MusicLow", MusicLow);
    }

    void SetMusicHigh(float MusicHigh)
    {
        Music.SetFloat("MusicHigh", MusicHigh);
    }
}
