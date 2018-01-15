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

	public PlayerController player;
	public List<PlayerController> players = new List<PlayerController>();
	public List<GameObject> playerUIs = new List<GameObject>();

	private void Start() {
		direct = this;
	}

	private void Update() {
		if (player) {
			camera.position = new Vector3(player.transform.position.x , player.transform.position.y , camera.position.z);
			if (!connecting) {
				Network.InitializeServer(1, 7777);
			}
		}
	}

	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log("Connected to server");
		connecting = true;
	}

	public void Focus(PlayerController focusing) {
		player = focusing;
	}
	
	void OnServerInitialized() {
		connecting = true;
	}

	public void OnVictory() {

	}
	
	public void ResetCamera() {
		PlayerController temp = null;

		foreach (PlayerController unit in players) {
			if (!unit.isDead) {
				if (temp == null) {
					temp = unit;

				} else if (temp.hp < unit.hp) {
					temp = unit;
				}
			}
		}
		player = temp;
	}

	public void OnRegist(PlayerController value) {
		players.Add(value);
		playerUIs[value.PlayerIndex].SetActive(true);
	}

	public void OnDead(PlayerController value) {
		value.Dead();
		ResetCamera();
		playerUIs[value.PlayerIndex].SetActive(false);
	}

	public void OnReborn(PlayerController value) {
		foreach (PlayerController unit in players) {
			if (unit.gameObject != value && unit.hp > 2 && !unit.isDead) {
				unit.Attack(2, true);
				value.transform.position = unit.transform.position;
				value.Attack(0, true);
				value.hp = 2;
				value.Reborn();
				ResetCamera();
				playerUIs[value.PlayerIndex].SetActive(true);
				return;
			}
		}
	}
}