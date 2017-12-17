using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public Transform units;
	public Transform camera;
	public Transform player;

	private void Start() {
		direct = this;
	}

	private void Update() {
		if (player) {
			camera.position = new Vector3(player.position.x , player.position.y + 5 , camera.position.z);
		}
	}

	void OnSerializeNetworkView() {
		Debug.Log("22");
	}
}