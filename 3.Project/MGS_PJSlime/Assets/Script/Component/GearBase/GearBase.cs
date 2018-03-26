using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class GearBase : NetworkBehaviour {
	public enum TriggerType {
		always,
		once,
		continuous,
		button,
		oncebutton,
	}

	public abstract bool BaseTrigger();
	public abstract bool Trigger();
	
	public TriggerType triggerType;
	public List<TriggerBase> triggers = new List<TriggerBase>();	
	protected bool active = true;

	void Start() {
		foreach (TriggerBase trigger in triggers) {
			if (trigger) {
				trigger.RegistGear(this);
			} else {
				Debug.LogWarning("#GearBase:triggers註冊異常");
			}	
		}
		FStart();
	}

	protected virtual void FStart() {

	}

	public bool IsTriggering() {
		foreach (TriggerBase trigger in triggers) {
			if (trigger) {
				if (trigger.IsTriggering()) {
					return true;
				}
			}			
		}
		return false;
	}
}
