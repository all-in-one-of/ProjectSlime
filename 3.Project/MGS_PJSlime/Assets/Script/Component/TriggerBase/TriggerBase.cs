using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TriggerBase : NetworkBehaviour {
	public List<GearBase> triggerObject = new List<GearBase>();
	public List<Collider2D> touching = new List<Collider2D>();
	public float resetTime = 6;
	public bool enforceMode = false;
	public bool activeMode = false;
	public bool weightMode = false;
	public int triggerWeight = 6;

	protected bool triggering = false;
	protected float clock = 0;
	

	void Update() {
		if (triggering && Time.timeSinceLevelLoad - clock > resetTime ) {
			ResetTrigger();
		}
	}

	protected void OnTriggerEnter2D(Collider2D collider) {
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

			foreach (GearBase gear in triggerObject) {
				if (enforceMode) {
					gear.BaseTrigger();
				} else {
					gear.Trigger();
				}				
			}
		}
	}

	protected void OnTriggerStay2D(Collider2D collider) {
		if (activeMode) {
			if (weightMode) {
				if (!touching.Contains(collider) && collider.GetComponent<PlayerController>()) {
					touching.Add(collider);
				}
			} else {
				if (!touching.Contains(collider)) {
					touching.Add(collider);
				}
			}
		}
	}

	protected void OnTriggerExit2D(Collider2D collider) {
		if (activeMode) {
			touching.Remove(collider);
		}
	}

	public void ResetTrigger() {
		triggering = false;
	}

	public bool IsTriggering() {
		if (activeMode) {
			if (weightMode) {
				float weight = 0;
				foreach(Collider2D entity in touching) {
					weight += entity.GetComponent<PlayerController>().size;
				}

				return weight > triggerWeight;
			} else {
				return touching.Count > 0;
			}
		}
		return false;
	}
}
