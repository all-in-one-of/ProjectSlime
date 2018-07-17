using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGear : GearBase {
	public AudioClip clip;

	protected override void FStart() { }
		
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Slime" && active) {
			Trigger();
		}
	}

	public override bool BaseTrigger(bool postive = true) {
		active = false;
		AudioManager.direct.PlayMusic(clip);
		return true;
	}

	public override bool Trigger(bool postive = true) {
		return BaseTrigger();
	}
}
