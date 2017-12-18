using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public Transform units;
	public Transform camera;
	public Transform player;
	public Transform unit;
	public bool connecting;

	private void Start() {
		direct = this;
	}

	private void Update() {
		if (player) {
			camera.position = new Vector3(player.position.x , player.position.y + 5 , camera.position.z);
			if (!connecting) {
				Network.InitializeServer(1, 7777);
			}
		}
	}

	void OnServerInitialized() {
		connecting = true;
	}
}