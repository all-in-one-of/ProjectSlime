using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineBase : GearBase {
	protected SkeletonAnimation sk;

	protected float coldDown = 6;
	protected bool coldDowning = false;
	protected bool acting = false;
	protected float clock = 0;

	[SpineAnimation]
	public string idleAnim;

	[SpineAnimation]
	public string actAnim;

	protected override void FStart() {
		sk = GetComponent<SkeletonAnimation>();
		sk.state.SetAnimation(0, idleAnim, false);
	}

	void Update() {
		if (coldDowning && Time.timeSinceLevelLoad - clock > coldDown) {
			ResetAct();
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
		if (!coldDowning) {
			if (!acting) {
				acting = true;
				sk.state.SetAnimation(0, actAnim, false);
			} else {
				acting = false;
				sk.state.SetAnimation(0, idleAnim, false);
			}

			if (triggerType == TriggerType.once) {
				active = false;
			}
			coldDowning = true;
			clock = Time.timeSinceLevelLoad;
			return true;
		} 
		return false;
	}

	public override bool Trigger(bool postive = true) {
		return BaseTrigger();
	}
	
	public void ResetAct() {
		coldDowning = false;
	}
}
