using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBase : GearBase {
	public float aroundSpeed = 10;
	public bool accMode = true;
	public float startAngle = 0;
	public bool positive = true;
	
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;

	protected float angled;

	protected override void FStart() {
		angled = startAngle;
		angled += ((positive ? aroundSpeed : -aroundSpeed) * Time.deltaTime) % 360;//累加已经转过的角度

		if (triggerType != TriggerType.always) {
			active = false;
		}
	}

	void FixedUpdate() {
		if (!unTriggerable) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {
				transform.Rotate(new Vector3(0, 0, -aroundSpeed * Time.deltaTime));
				Vector2 shift = transform.position;
			}
		}
		if (!active && triggerType == TriggerType.once) {
			if (IsTriggering()) {
				active = true;
			}
		}
	}

	public override bool BaseTrigger() {
		return true;
	}

	public override bool Trigger() {
		if (triggerType == TriggerType.button) {
			if (firstTrigger) {
				positive = !positive;
			}

			unTriggerable = false;
			active = true;
			positive = !positive;
			return BaseTrigger();

		} else if (triggerType == TriggerType.oncebutton) {
			active = true;
			return BaseTrigger();
		}
		return false;
	}
}
