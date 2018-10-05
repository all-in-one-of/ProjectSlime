using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : GearBase {
	public Vector2 vector = new Vector2(0, 0);
	public float speed = 1;
	public float onceTime = 5;
	public bool carryMode = false;
	public bool positive = true;

	protected List<Transform> carryobj = new List<Transform>();

	public int nowPoint = 0;

	[SerializeField]
	public List<Vector2> pointList = new List<Vector2>();
		
	protected float fullDistance = 0;
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;
	
	protected override void FStart() {
		if (pointList.Count > 0) {
			transform.position = positive ? pointList[0] : pointList[pointList.Count - 1];
			nowPoint = positive ? 0 : pointList.Count - 1;

			for (int i = 1; i < pointList.Count; i++) {
				fullDistance += Vector2.Distance(pointList[i - 1], pointList[i]);
			}
		}

		if (triggerType != TriggerType.always) {
			active = false;
		}

		if (triggerType == TriggerType.button) {
			positive = false;
		}
	}
	
	void FixedUpdate() {
		if (!unTriggerable) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {
				if (pointList.Count > 0) {
					float energe = speed * Time.deltaTime;

					while (energe > 0) {
						if (HaveNextPoint()) {
							float dis = Vector2.Distance(transform.position, NextPoint());
							Vector2 vec = (NextPoint() - pointList[nowPoint]).normalized;
							Vector2 shift;

							if (dis > energe) {
								shift = vec * energe;
								transform.position = (Vector2)transform.position + vec * energe;
								energe = 0;
								CarryObj(shift);

							} else {
								shift = NextPoint() - (Vector2)transform.position;
								transform.position = pointList[nowPoint + (positive ? 1 : -1)];
								energe -= dis;
								CarryObj(shift);

								nowPoint += (positive ? 1 : -1);
								if (!HaveNextPoint()) {
									if (triggerType == TriggerType.always) {
										BaseTrigger();

									} else if (triggerType == TriggerType.once || triggerType == TriggerType.oncebutton) {
										BaseTrigger();
										if (firstTrigger) {
											unTriggerable = true;
										}

										firstTrigger = true;

									}
									break;
								}
							}
						} else {
							break;
						}
					}
				}
			}
		}
		if (!active && triggerType == TriggerType.once) {
			if (IsTriggering()) {
				firstTrigger = true;
				active = true;
			}
		}
	}

	public bool HaveNextPoint() {
		return positive ? (pointList.Count > nowPoint + 1) : 0 <= nowPoint - 1;
	}

	public Vector2 NextPoint() {
		return pointList[nowPoint + (positive ? 1 : -1)]; ;
	}
	
	public override bool BaseTrigger(bool postive = true) {
		positive = !positive;
		return true;
	}

	public override bool Trigger(bool postive = true) {
		if (triggerType == TriggerType.button) {		
			active = true;
			return BaseTrigger();

		} else if (triggerType == TriggerType.oncebutton) {
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
		float nowDistance = 0;
		if (pointList.Count > 0) {
			for (int i = 0; i < nowPoint; i++) {
				if (i == nowPoint) {
					nowDistance += Vector2.Distance(transform.position, pointList[i]);
				} else {
					nowDistance += Vector2.Distance(pointList[i - 1], pointList[i]);
				}
			}
			return fullDistance / nowDistance;
		} else {
			return 0;
		}
	}	
}
