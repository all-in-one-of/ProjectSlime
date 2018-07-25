using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager direct;
	public EventSystem uiTrigger;
	public Text timer;
	public Text counter;

	public Transform bullpenObject;
	public Transform gardenObject;

	public List<Material> recordPlayer = new List<Material>();
	public List<Text> recordShower = new List<Text>();
	public List<Text> recordShower2 = new List<Text>();
	public List<Image> recordShower3 = new List<Image>();

	void Start() {
		direct = this;
		DontDestroyOnLoad(this);
	}

	public void Join() {
		if (PrototypeSystem.direct) {
			PrototypeSystem.direct.SpawnPlayer();
			uiTrigger.SetSelectedGameObject(gameObject);
		}		
	}

	public void Pause() {
		if (PrototypeSystem.direct) {
			PrototypeSystem.direct.Pause();
			uiTrigger.SetSelectedGameObject(gameObject);
		}			
	}

	public void Restart() {
		SkyTalker.direct.ResetScene();
		uiTrigger.SetSelectedGameObject(gameObject);
	}

	public void OnStage() {
		bullpenObject.gameObject.SetActive(true);
	}

	public void EndScore() {
		gardenObject.gameObject.SetActive(false);
		GameEngine.direct.Init(GameEngine.Status.Garden);
	}

	public void OnScore() {
		gardenObject.gameObject.SetActive(true);
		bullpenObject.gameObject.SetActive(false);
	}
}
