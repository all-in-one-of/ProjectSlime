﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBase : GearBase {
	public float breakTime = 2;
	public float resetTime = 6;
	public Collider2D c2d;

	protected bool breaking = false;
	protected float clock = 0;
	protected Vector2 originSize;

	protected override void FStart() {
		originSize = transform.localScale;
		c2d = gameObject.GetComponent<Collider2D>();
	}


	void Update () {
		if (breaking && Time.timeSinceLevelLoad - clock > resetTime + breakTime) {
			ResetBreak();
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (active || (triggerType == TriggerType.continuous && IsTriggering())) {
			if (triggerType == TriggerType.continuous && !IsTriggering()) {
				return;
			}
			Trigger();
		}
	}

	public override bool BaseTrigger(bool postive = true) {
		if (!breaking) {
			breaking = true;
			clock = Time.timeSinceLevelLoad;
			Invoke("Break", breakTime);
			if ( triggerType == TriggerType.once) {
				active = false;
			}
			return true;
		}
		return false;
	}

	public override bool Trigger(bool postive = true) {		
		return BaseTrigger();
	}
	
	public void Break() {
		transform.localScale = Vector2.zero;

		foreach (PlayerController pc in GameEngine.direct.players) {
			pc.EndCollider(c2d);
		}		
	}

	public void ResetBreak() {
		breaking = false;
		transform.localScale = originSize;
	}
}
