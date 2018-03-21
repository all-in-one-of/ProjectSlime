using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBase : GearBase {
	public float breakTime = 2;
	public float resetTime = 6;

	protected bool breaking = false;
	protected float clock = 0;
	protected Vector2 originSize;

	void Start() {
		originSize = transform.localScale;
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

	public override bool BaseTrigger() {
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

	public override bool Trigger() {		
		return BaseTrigger();
	}
	
	public void Break() {
		transform.localScale = Vector2.zero;
	}

	public void ResetBreak() {
		breaking = false;
		transform.localScale = originSize;
	}
}
