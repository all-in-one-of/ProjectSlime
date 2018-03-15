using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class GearBase : NetworkBehaviour {
	public abstract bool BaseTrigger();
	public abstract bool Trigger();
}
