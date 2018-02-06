using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	private static string checkPoint = "";

	public Transform units;
	public bool connecting;

	public static PlayerController mainPlayer;
	public List<PlayerController> players = new List<PlayerController>();
	public List<GameObject> playerUIs = new List<GameObject>();

	public float walkXSpeed = 8;
	public float walkXAcc = 10;
	public float walkXDec = 10;

	public float jumpXSpeed = 8;
	public float jumpXAcc = 10;
	public float jumpXDec = 10;

	public float jumpYForce = 8;
	public float jumpYDec = 10;

	public float iceXAcc = 10;
	public float iceXDec = 10;

	private void Start() {
		direct = this;
	}

	private void Update() {
		if (!connecting) {
			Network.InitializeServer(1, 7777);
		}
	}

	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log("Connected to server");
		connecting = true;
	}

	public void Focus(PlayerController focusing) {
		mainPlayer = focusing;
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
		mainPlayer = temp;
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

	public void KillBorder(Vector2 cameraPos) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i] != mainPlayer ) {
				if (Mathf.Abs(players[i].transform.position.x - cameraPos.x) > 30) {
					OnDead(players[i]);
				}
				if (Mathf.Abs(players[i].transform.position.y - cameraPos.y) > 17) {
					OnDead(players[i]);
				}
			}
		}
	}

	public static void RegistCheckPoint(string obj) {
		checkPoint = obj;
	}

	public static void ResetCheckPoint() {
		checkPoint = null;
	}

	public static string GetCheckPoint() {
		return checkPoint;
	}
}