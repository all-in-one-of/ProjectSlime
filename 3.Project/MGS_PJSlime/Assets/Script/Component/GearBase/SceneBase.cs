﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase : GearBase {
	public string name;

	protected override void FStart() {
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Slime" && active) {
			Trigger();
		}
	}

	public override bool BaseTrigger(bool postive = true) {
		Application.LoadLevel(name);
		return true;
	}

	public override bool Trigger(bool postive = true) {
		return BaseTrigger();
	}
}