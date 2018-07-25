using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager direct;
	public AudioSource mainBgmSource;
	public AudioClip mainBgm;

	void Start() {
		direct = this;
		DontDestroyOnLoad(this);
	}

	public void Init() {
		PlayMusic(mainBgm);
	}

	public void PlayMusic(AudioClip music) {
		mainBgmSource.clip = music;
		mainBgmSource.Play();
	}
}
