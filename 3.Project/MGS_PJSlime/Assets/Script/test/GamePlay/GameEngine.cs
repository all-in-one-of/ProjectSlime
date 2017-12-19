using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public Transform units;
	public Transform camera;
	public Transform player;
	public Transform unit;
	public bool connecting;
	NetworkClient myClient;

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
	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log("Connected to server");
		connecting = true;
	}

	void OnServerInitialized() {
		connecting = true;
	}
}