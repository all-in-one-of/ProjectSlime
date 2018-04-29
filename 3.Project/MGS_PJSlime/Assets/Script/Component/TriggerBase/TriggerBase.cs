using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TriggerBase : NetworkBehaviour {
	public enum TriggerType {
		continuous,
		once,
	}

	public TriggerType triggerType;
	public float resetTime = 6;
	public bool enforceMode = false;
	public bool weightMode = false;
	public int triggerWeight = 6;

	protected List<GearBase> onceObject = new List<GearBase>();
	protected List<Collider2D> nowTouching = new List<Collider2D>();
	protected bool triggering = false;
	protected float clock = 0;
	
	protected void Update() {
		OnUpdate();
	}

	protected virtual void OnUpdate() {
		if (triggerType == TriggerType.once) {
			if (triggering && Time.timeSinceLevelLoad - clock > resetTime) {
				ResetTrigger();
			}
		}
	}

	protected void OnTriggerEnter2D(Collider2D collider) {
		TriggerEnter(collider);
	}
	
	protected void OnTriggerStay2D(Collider2D collider) {
		TriggerStay(collider);
	}

	protected void OnTriggerExit2D(Collider2D collider) {
		TriggerExit(collider);
	}

	protected virtual void TriggerEnter(Collider2D collider) {
		if (triggerType == TriggerType.once) {
			if (!triggering) {
				if (weightMode) {
					if (collider.GetComponent<PlayerController>()) {
						if (collider.GetComponent<PlayerController>().size >= triggerWeight) {

						}
					} else {
						return;
					}
				}

				triggering = true;
				clock = Time.timeSinceLevelLoad;

				foreach (GearBase gear in onceObject) {
					if (enforceMode) {
						gear.BaseTrigger();
					} else {
						gear.Trigger();
					}
				}
			}
		}
	}

	protected virtual void TriggerStay(Collider2D collider) {
		if (triggerType == TriggerType.continuous) {
			if (weightMode) {
				if (!nowTouching.Contains(collider) && collider.GetComponent<PlayerController>()) {
					nowTouching.Add(collider);
				}
			} else {
				if (!nowTouching.Contains(collider)) {
					nowTouching.Add(collider);
				}
			}
		}
	}

	protected virtual void TriggerExit(Collider2D collider) {
		if (triggerType == TriggerType.continuous) {
			nowTouching.Remove(collider);
		}
	}

	public void RegistGear(GearBase Object) {
		onceObject.Add(Object);
	}

	public void ResetTrigger() {
		triggering = false;
	}

	public bool IsTriggering() {
		if (triggerType == TriggerType.continuous) {
			if (weightMode) {
				float weight = 0;
				foreach(Collider2D entity in nowTouching) {
					weight += entity.GetComponent<PlayerController>().size;
				}

				return weight > triggerWeight;
			} else {
				return nowTouching.Count > 0;
			}
		}
		return false;
	}
}
