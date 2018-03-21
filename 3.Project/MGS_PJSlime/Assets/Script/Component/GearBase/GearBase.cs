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
		continuous
	}

	public abstract bool BaseTrigger();
	public abstract bool Trigger();

	public TriggerType triggerType;
	public List<TriggerBase> triggers = new List<TriggerBase>();	
	protected bool active = true;

	void Start() {
		foreach (TriggerBase trigger in triggers) {
			trigger.RegistGear(this);
		}
		FStart();
	}

	protected virtual void FStart() {

	}

	public bool IsTriggering() {
		foreach (TriggerBase trigger in triggers) {
			if (trigger.IsTriggering()) {
				return true;
			}
		}
		return false;
	}
}
