using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager direct;
	public AudioSource mainBgmSource;
	public AudioClip mainBgm;

	private void Start() {
		direct = this;
		PlayMusic(mainBgm);
	}

	public void PlayMusic(AudioClip music) {
		mainBgmSource.clip = music;
		mainBgmSource.Play();
	}
}
