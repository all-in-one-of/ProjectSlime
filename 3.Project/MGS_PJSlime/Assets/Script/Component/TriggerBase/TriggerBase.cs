using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TriggerBase : NetworkBehaviour {
	public List<GearBase> triggerObject = new List<GearBase>();
	public float resetTime = 6;
	public bool enforceTrigger = false;

	protected bool triggering = false;
	protected float clock = 0;
	

	void Update() {
		if (triggering && Time.timeSinceLevelLoad - clock > resetTime ) {
			ResetTrigger();
		}
	}

	protected void OnTriggerEnter2D(Collider2D collider) {
		if (!triggering) {
			triggering = true;
			clock = Time.timeSinceLevelLoad;

			foreach (GearBase gear in triggerObject) {
				if (enforceTrigger) {
					gear.BaseTrigger();
				} else {
					gear.Trigger();
				}				
			}
		}
	}

	public void ResetTrigger() {
		triggering = false;
	}
}
