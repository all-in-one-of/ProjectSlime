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

	public TriggerType triggerType;
	public List<TriggerBase> triggers = new List<TriggerBase>();
	public bool active = true;

	public abstract bool BaseTrigger();
	public abstract bool Trigger();

	public bool IsTriggering() {
		foreach (TriggerBase trigger in triggers) {
			if (trigger.IsTriggering()) {
				return true;
			}
		}
		return false;
	}
}
