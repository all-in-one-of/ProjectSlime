using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour {
	public ParticleSystem[] particles;

	public void Play() {
		foreach (ParticleSystem particle in particles) {
			particle.Play();
			particle.enableEmission = true;
		}
	}

	public void Stop() {
		foreach (ParticleSystem particle in particles) {
			particle.Pause();			
		}
	}

	public void Pause() {
		foreach (ParticleSystem particle in particles) {
			particle.enableEmission = false;
		}
	}
}
