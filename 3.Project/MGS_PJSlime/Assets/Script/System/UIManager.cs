﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager direct;
	public EventSystem uiTrigger;
	public Text timer;
	public Text counter;
	public Transform recordObject;
	public List<Material> recordPlayer = new List<Material>();
	public List<Text> recordShower = new List<Text>();
	public List<Text> recordShower2 = new List<Text>();
	public List<Image> recordShower3 = new List<Image>();

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
