using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 此類提供非同步緩存載入場景（目前僅支持緩存載入一個場景），注意︰在unity編輯器里調試時，非同步載入會阻塞主線程，打包後，非同步載入不會阻塞主線程，
/// 具體參考︰ https://forum.unity3d.com/threads/scenemanager-loadsceneasync-not-working-as-expected.369714/ 
/// </summary>
public class SceneSwitcher : MonoBehaviour {
	public static SceneSwitcher direct;

	[SerializeField]
	Slider progressSlider;
	AsyncOperation ao;

	protected string NextScene;

	void Start() {
		direct = this;
	}


	public void SwitchScene(string value) {
		NextScene = value;
		//progressSlider.minValue = 0f;
		//progressSlider.maxValue = 100f;
		//progressSlider.value = 0f;

		//doneLoadingScene = false;

		StartCoroutine(LoadNextLevelAsync());
	}
	
	/*
	bool doneLoadingScene = false;	
	void Update() {
		if (ao != null && !doneLoadingScene) {
			progressSlider.value = ao.progress * 100f;
			if (ao.isDone) {
				progressSlider.value = 100f;
				doneLoadingScene = true;
			}
		}
	}*/

	IEnumerator LoadNextLevelAsync() {
		ao = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);
		yield return ao;
	}
}