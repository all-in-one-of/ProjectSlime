using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager direct;
	public EventSystem uiTrigger;
	public Text timer;

	public void Start() {
		direct = this;
	}

	public void Join() {
		PrototypeSystem.direct.SpawnPlayer();
		uiTrigger.SetSelectedGameObject(gameObject);
	}

	public void Pause() {
		PrototypeSystem.direct.Pause();
		uiTrigger.SetSelectedGameObject(gameObject);
	}

	public void Restart() {
		SkyTalker.direct.ResetScene();
		uiTrigger.SetSelectedGameObject(gameObject);
	}
}
