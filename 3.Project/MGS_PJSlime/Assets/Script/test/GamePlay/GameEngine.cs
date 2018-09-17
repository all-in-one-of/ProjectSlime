using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public static int stageCountDown = 240;
	private static string checkPoint = "";

	public enum Status {
		Bullpen,
		Stage,
		Garden,

		Loading
	}

	public Status status = Status.Loading;
	public bool connecting;

	//test
	public bool preTester = false;
	public GameObject testStage;

	public static PlayerController mainPlayer;

	public static GameObject nowStage;
	public static StageData nowStageData;

	public Material[] playerMaterial;

	public List<PlayerController> players = new List<PlayerController>();
	public List<GameObject> playerUIs = new List<GameObject>();
	private static Buffer[] playerBuffer = {
		new Buffer(),
		new Buffer(),
		new Buffer(),
		new Buffer()};

	public List<GameObject> stageList = new List<GameObject>();

	public GameObject cameraManager;
	public GameObject audioManager;
	public GameObject uiManager;
	public GameObject gardenStage;

	public int broSize = 4;		//大哥生命
	public int baseSize = 2;		//小弟生命

	public int bornSize = 1;		//復活生命
	public int bornReqSize = 2;   //復活需求
	public int bornCost = 1;		//復活消耗
	
	public float walkXSpeed = 8;	
	public float walkXAcc = 10;		
	public float walkXDec = 10;		
	
	public int jumpMaxCount = 2;
	public float jumpReduce = 1;

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

	private int stageIndex = 0;
	private bool gameStart = false;


	void Start() {
		direct = this;
		DontDestroyOnLoad(this);

		//Initiate Manger
		GameObject temp;
		temp = Instantiate(cameraManager);
		CameraManager.direct = temp.GetComponent<CameraManager>();

		temp = Instantiate(audioManager);
		AudioManager.direct = temp.GetComponent<AudioManager>();

		temp = Instantiate(uiManager);
		UIManager.direct = temp.GetComponent<UIManager>();

		//Init - SYS
		new ScoreSystem();
		Init();
	}



	public void Init( Status nextStatus = Status.Stage) {
		//Init - Value
		mainPlayer = null;
		players = new List<PlayerController>();
		playerUIs = new List<GameObject>();

		//Init - System
		CameraManager.direct.Init();
		AudioManager.direct.Init();

		//Init - Stage
		if (preTester && testStage) {
			LoadStage(testStage);
		} else {
			LoadStage(stageList[stageIndex]);
		}

		UIManager.direct.OnStage();
		/*
		if (nextStatus == Status.Stage) {
			if (preTester && testStage) {
				LoadStage(testStage);
			} else {
				LoadStage(stageList[stageIndex]);
			}

			UIManager.direct.OnStage();
		} else {
			LoadStage(gardenStage);
		}*/
		
		//Init - Finish
		status = nextStatus;
		connecting = false;
		gameStart = true;
	}
	
	//讀取場景
	public void LoadStage(GameObject value) {
		nowStage = Instantiate(value);
		nowStageData = nowStage.GetComponent<StageData>();
	}
	
	private void Update() {
		if (status == Status.Stage) {
			if (!connecting) {
				Network.InitializeServer(1, 7777, true);
				//PrototypeSystem.direct.Init();
			} else {
				UIManager.direct.timer.text = ((int)(stageCountDown - Time.timeSinceLevelLoad)).ToString();
				if (stageCountDown - Time.timeSinceLevelLoad < 0) {
					SkyTalker.direct.ResetScene();
				}
			}
		} else if (status == Status.Garden) {
			if (!connecting) {
				Network.InitializeServer(1, 7777, true);
				//PrototypeSystem.direct.Init();
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
		if (gameStart) {
			gameStart = false;

			stageIndex = stageIndex + 1 >= stageList.Count ? 0 : stageIndex + 1;

			ScoreSystem.CaculateRecord();
			if (nowStage) {
				Destroy(nowStage);
			}

			foreach (PlayerController pl in players) {
				Destroy(pl.gameObject);
			}

			foreach (GameObject pl in playerUIs) {
				pl.SetActive(false);
			}
		}	
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
		if (mainPlayer.hp >= bornReqSize && !mainPlayer.isDead) {
			mainPlayer.Attack(bornCost, true);
			value.transform.position = mainPlayer.transform.position;
			value.Attack(0, true);  //immune
			value.Reborn();
			ResetCamera();
			playerUIs[value.playerID].SetActive(true);
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
				
		foreach (Transform unit in nowStageData.unitSet) {
			if (Vector2.Distance(pos, unit.position) <= range) {
				EntityBase enemy = unit.GetComponent<EntityBase>();
				if (enemy && !enemy.isDead) {
					return enemy;
				}
			}
		}
		return null;
	}

	public static Buffer GetBuffer(int index) {
		return playerBuffer[index];
	}

	public static void AddBufferEffect(int index , BufferEffect buffValue) {
		playerBuffer[index].AddEffect(new BufferEffect(buffValue));
	}

	public static void AddBufferEffect(BufferEffect buffValue) {
		foreach (Buffer pcBuffer in playerBuffer) {
			pcBuffer.AddEffect(new BufferEffect(buffValue));
		}
	}
}