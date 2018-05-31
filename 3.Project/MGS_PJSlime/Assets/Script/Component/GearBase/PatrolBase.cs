using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : GearBase {
	public Vector2 vector = new Vector2(0, 0);
	public float onceTime = 5;
	public bool accMode = true;
	public bool carryMode = false;
	public bool positive = true;

	protected List<Transform> carryobj = new List<Transform>();
	public Vector2 pa = new Vector2(0, 0);
	public Vector2 pb = new Vector2(0, 0);
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;

	protected float distance;

	protected override void FStart() {
		pa = (Vector2)transform.position;
		pb = (Vector2)transform.position + vector * onceTime;

		distance = Vector2.Distance(pa, pb);
		
		if (!positive) {
			transform.position = pb;
		}

		if (triggerType != TriggerType.always ) {
			active = false;
		}
	}
	
	void FixedUpdate() {
		if (!unTriggerable) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {

				if ((positive && Vector2.Distance(pa, transform.position) <= distance) || (!positive && Vector2.Distance(pb, transform.position) <= distance)) {
					Vector2 shift = (positive ? vector : -vector) * Time.deltaTime;
					transform.position = (Vector2)transform.position + shift;
					CarryObj(shift);
				}

				
				if ((positive && Vector2.Distance(pa, transform.position) > distance) || (!positive && Vector2.Distance(pb, transform.position) > distance)) {
					BaseTrigger();
					if (triggerType == TriggerType.once || triggerType == TriggerType.oncebutton || triggerType == TriggerType.button) {
						if (firstTrigger) {
							unTriggerable = true;
						}

						firstTrigger = true;
					}
				}
			}
		}
		if (!active && triggerType == TriggerType.once) {
			if (IsTriggering()) {
				active = true;
			}
		}
	}

	public override bool BaseTrigger() {
		positive = !positive;		
		return true;
	}

	public override bool Trigger() {
		if (triggerType == TriggerType.button) {
			if (firstTrigger) {
				positive = !positive;
			}
			
			unTriggerable = false;
			active = true;
			return BaseTrigger();

		} else if(triggerType == TriggerType.oncebutton) {
			active = true;
			return BaseTrigger();
		}
		return false;
	}

	protected void CarryObj(Vector2 shift) {
		if (carryobj.Count > 0) {
			foreach (Transform obj in carryobj) {
				obj.position = (Vector2)obj.position + shift;
			}
		}
	}	
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (carryMode && !carryobj.Contains(collision.transform)) {
			carryobj.Add(collision.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (carryMode && carryobj.Contains(collision.transform)) {
			carryobj.Remove(collision.transform);
		}
	}

	public float GetCompleteRate() {
		return Vector2.Distance(pa, transform.position) / distance;
	}
}
