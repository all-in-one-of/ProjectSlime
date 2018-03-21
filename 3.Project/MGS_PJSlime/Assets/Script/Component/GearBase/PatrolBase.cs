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
	public bool DEBUG = false;

	protected List<Transform> carryobj = new List<Transform>();
	protected Vector2 max = new Vector2(0, 0);
	protected Vector2 min = new Vector2(0, 0);
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;
	float aa = 0;

	protected override void FStart() {
		max = (Vector2)transform.position;
		min = (Vector2)transform.position - vector * onceTime;

		if (!positive) {
			transform.position = min;
		}

		if (triggerType == TriggerType.continuous) {
			active = false;
		}

		if (triggerType == TriggerType.once) {
			active = false;
		}
	}
	
	void FixedUpdate() {
		if (!unTriggerable) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {
				if (positive && (transform.position.x > max.x || transform.position.y > max.y)) {

				} else if (!positive && (transform.position.x < min.x || transform.position.y < min.y)) {

				} else {
					Vector2 shift = (positive ? vector : -vector) * Time.deltaTime;
					transform.position = (Vector2)transform.position + shift;
					CarryObj(shift);
				}



				if (!accMode) {

				} else {

					/*
					Vector2 shift = transform.position;
					transform.position = Vector2.Lerp(transform.position, target ? a : b, speed);
					shift = (Vector2)transform.position - shift;
					CarryObj(shift);*/
				}



				if ((positive && (transform.position.x > max.x || transform.position.y > max.y)) || (!positive && (transform.position.x < min.x || transform.position.y < min.y))) {
					Trigger();
					if (triggerType == TriggerType.once) {
						if (firstTrigger) {
							unTriggerable = true;
						}

						firstTrigger = true;
					}
				}
				/*
				if (triggerType == TriggerType.always || (triggerType == TriggerType.continuous && IsTriggering())) {
					Trigger();
				}
				if (triggerType == TriggerType.once) {
					active = false;
				}*/
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
		return BaseTrigger();
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
}
