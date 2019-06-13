using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager direct;
	public EventSystem uiTrigger;
	public Text timer;
	public Text counter;

	public Transform bullpenObject;
	public Transform scoreObject;
	public Transform gardenObject;

	public List<Material> recordPlayer = new List<Material>();
	public List<Text> recordShower = new List<Text>();
	public List<Text> recordShower2 = new List<Text>();
	public List<Image> recordShower3 = new List<Image>();

	[SerializeField]
	public List<BufferEffect> bufferEffects = new List<BufferEffect>();

	void Start() {
		direct = this;
		DontDestroyOnLoad(this);
	}

	public void Join() {
		GameEngine.direct.SpawnPlayer();
		uiTrigger.SetSelectedGameObject(gameObject);
	}

	public void Pause() {
		GameEngine.Pause();
		uiTrigger.SetSelectedGameObject(gameObject);
	}

	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		uiTrigger.SetSelectedGameObject(gameObject);
	}
		
	//score
	public void OnScore() {
		Debug.Log("s");
		scoreObject.gameObject.SetActive(true);
		//bullpenObject.gameObject.SetActive(false);
	}
	public void EndScore() {
		scoreObject.gameObject.SetActive(false);
		OnGarden();
	}

	//garden
	public void OnGarden() {
		gardenObject.gameObject.SetActive(true);
	}
	public void EndGarden(int buffValue) {
		if (ScoreSystem.GetScore() >= bufferEffects[buffValue].cost) {
			ScoreSystem.ModifyScore(-bufferEffects[buffValue].cost);
			EndGarden();
		}
	}

	public void EndGarden() {
		gardenObject.gameObject.SetActive(false);
		GameEngine.direct.StageInit(GameEngine.Status.Garden);
	}
}
