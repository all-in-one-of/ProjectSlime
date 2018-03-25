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
	public Vector2 pa = new Vector2(0, 0);
	public Vector2 pb = new Vector2(0, 0);
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;
	float aa = 0;

	protected float distance;

	protected override void FStart() {
		pa = (Vector2)transform.position;
		pb = (Vector2)transform.position + vector * onceTime;

		distance = Vector2.Distance(pa, pb);

		/*
		if (min.x > max.x) {
			float temp = max.x;
			max.x = min.x;
			min.x = temp;
		}
		if (min.y > max.y) {
			float temp = max.y;
			max.y = min.y;
			min.y = temp;
		}*/


		if (!positive) {
			transform.position = pb;
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

				if ((positive && Vector2.Distance(pa, transform.position) <= distance) || (!positive && Vector2.Distance(pb, transform.position) <= distance)) {
					Vector2 shift = (positive ? vector : -vector) * Time.deltaTime;
					transform.position = (Vector2)transform.position + shift;
					CarryObj(shift);
				}

				
				if ((positive && Vector2.Distance(pa, transform.position) > distance) || (!positive && Vector2.Distance(pb, transform.position) > distance)) {	
					Trigger();
					if (triggerType == TriggerType.once) {
						if (firstTrigger) {
							unTriggerable = true;
						}

						firstTrigger = true;
					}
				}


				/*
				if (positive && (transform.position.x > max.x || transform.position.y > max.y)) {

				} else if (!positive && (transform.position.x < min.x || transform.position.y < min.y)) {

				} else {
					Vector2 shift = (positive ? vector : -vector) * Time.deltaTime;
					transform.position = (Vector2)transform.position + shift;
					CarryObj(shift);
				}
								

				if ((positive && (transform.position.x > max.x || transform.position.y > max.y)) || (!positive && (transform.position.x < min.x || transform.position.y < min.y))) {
					Trigger();
					if (triggerType == TriggerType.once) {
						if (firstTrigger) {
							unTriggerable = true;
						}

						firstTrigger = true;
					}
				}*/

				if (!accMode) {

				} else {

					/*
					Vector2 shift = transform.position;
					transform.position = Vector2.Lerp(transform.position, target ? a : b, speed);
					shift = (Vector2)transform.position - shift;
					CarryObj(shift);*/
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
