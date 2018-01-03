using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public Transform camera;
	public Transform units;
	public bool connecting;
	NetworkClient myClient;

	public Transform player;
	public List<PlayerController> players = new List<PlayerController>();

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

	public void Focus(Transform focusing) {
		player = focusing;
	}

	void OnServerInitialized() {
		connecting = true;
	}

	public void OnVictory() {

	}

	public void OnDead(PlayerController value) {
		bool dead = true;

		foreach (PlayerController unit in players) {
			if (unit.gameObject != value) {
				if (unit.hp > 2) {
					unit.Attack(2, true);
					value.transform.position = unit.transform.position;
					value.Attack(0, true);
					value.hp = 2;
					return;
				}
			}
		}

		if (dead) {
			players.Remove(value);
			Destroy(value.gameObject);
		}

		PlayerController temp = null;

		foreach (PlayerController unit in players) {
			if (temp == null) {
				temp = unit;

			} else if (temp.hp < unit.hp) {
				temp = unit;
			}
		}

		player = temp.transform;
	}
}