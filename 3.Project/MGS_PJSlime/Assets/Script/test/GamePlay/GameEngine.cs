using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public static int stageCountDown = 240;
	private static string checkPoint = "";
	
	public bool connecting;

	//test
	public bool preTester = false;
	public GameObject testStage;

	public static PlayerController mainPlayer;
	public static StageData nowStage;

	public List<PlayerController> players = new List<PlayerController>();
	public List<GameObject> playerUIs = new List<GameObject>();


	public List<GameObject> stageList = new List<GameObject>();

	public GameObject cameraManager;
	public GameObject audioManager;
	public GameObject scoreManager;
	public GameObject uiManager;

	public float walkXSpeed = 8;	
	public float walkXAcc = 10;		
	public float walkXDec = 10;		
	
	public int jumpGape = 48;

	public float jumpXSpeed = 8;
	public float jumpXAcc = 10;
	public float jumpXDec = 10;

	public float jumpYForce = 8;
	public float jumpDuraion = 0.5f;
	public float jumpYDec = 10;

	public float waterXSpeed = 8;
	public float waterYSpeed = 6;
	public float waterXAcc = 10;
	public float waterXDec = 10;
	public float waterYForce = 2;	
	public float waterColdDown = 0.25f;
	public float waterYDec = 2.5f;

	public float iceXAcc = 10;
	public float iceXDec = 10;
	
	private void Start() {
		direct = this;
		DontDestroyOnLoad(this);

		//Initiate Manger
		GameObject cm = Instantiate(cameraManager);
		Instantiate(audioManager);
		Instantiate(uiManager);

		//Init - SYS
		new ScoreSystem();
		cm.GetComponent<CameraManager>().Init();

		//Init - Stage
		if (preTester && testStage) {
			LoadStage(testStage);
		} else {

		}
		
		//Init - UI
	}

	public void LoadStage(GameObject value) {
		GameObject newStage = Instantiate(value);
		nowStage = newStage.GetComponent<StageData>();
	}

	private void Update() {
		if (!connecting) {
			Network.InitializeServer(1, 7777);
		} else {
			UIManager.direct.timer.text = ((int)(stageCountDown - Time.timeSinceLevelLoad)).ToString();
			if (stageCountDown - Time.timeSinceLevelLoad < 0) {
				SkyTalker.direct.ResetScene();
			}
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
		if (temp && mainPlayer != temp) {
			mainPlayer = temp;
			ScoreSystem.AddRecord(temp.playerID, 9, 1);
		}
	}

	public void OnRegist(PlayerController value) {
		players.Add(value);
		playerUIs[value.playerID].SetActive(true);
	}

	public void OnDead(PlayerController value) {
		ResetCamera();
		playerUIs[value.playerID].SetActive(false);
	}

	public void OnReborn(PlayerController value) {
		int hpRecord = 0;

		if (mainPlayer.hp > 2 && !mainPlayer.isDead) {
			mainPlayer.Attack(2, true);
			value.transform.position = mainPlayer.transform.position;
			value.Attack(0, true);
			value.Reborn();
			ResetCamera();
			playerUIs[value.playerID].SetActive(true);
			return;
		}

		foreach (PlayerController unit in players) {
			if (unit.gameObject != value && unit.hp > 2 && !unit.isDead && unit.hp > hpRecord) {
				unit.Attack(2, true);
				ScoreSystem.AddRecord(unit.playerID, 8, 1);
				value.transform.position = unit.transform.position;
				value.Attack(0, true);
				value.Reborn();
				ResetCamera();
				playerUIs[value.playerID].SetActive(true);
				return;
			}
		}
	}

	public void KillBorder(Vector2 cameraPos) {
		float cameraScale = CameraManager.direct.mainCamera.orthographicSize * 0.0625f;

		for (int i = 0; i < players.Count; i++) {
			if (players[i] != mainPlayer ) {
				if (Mathf.Abs(players[i].transform.position.x - cameraPos.x) > 28.8f * cameraScale) {
					players[i].OnDead();
				}
				if (Mathf.Abs(players[i].transform.position.y - cameraPos.y) > 16.2f * cameraScale) {
					players[i].OnDead();
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

	public EntityBase GetUnitInRange(float range , Vector2 pos) {

		foreach (Transform unit in nowStage.unitSet) {
			if (Mathf.Abs(transform.position.x - unit.transform.position.x) < range) {
				if (Mathf.Abs(transform.position.y - unit.transform.position.y) < range * 0.1f) {
					EntityBase enemy = unit.GetComponent<EntityBase>();
					if (enemy && !enemy.isDead) {
						return enemy;
					}
				}
			}

			if (Vector2.Distance(pos, unit.position) <= range) {
				EntityBase enemy = unit.GetComponent<EntityBase>();
				if (enemy && !enemy.isDead) {
					return enemy;
				}
			}
		}

		/*
		foreach (Transform unit in units) {
			if (Vector2.Distance(pos, unit.position) <= range) {
				EntityBase enemy = unit.GetComponent<EntityBase>();
				if (enemy && !enemy.isDead) {
					return enemy;
				}
			}
		}*/
		return null;
	}
}