using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeSystem : NetworkBehaviour {
	public static PrototypeSystem direct;
	public GameObject player;
	public Material[] mt;
	public int a = 0;
	public int clock;

	public int hostIntSize;
	public int intSize;
	public int jumpGape;
	public bool order = false;

	private void Start() {
		direct = this;
	}
	
	void OnServerInitialized() {
		SpawnPlayer();
		CameraManager.nowCamera.transform.position = new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y, CameraManager.nowCamera.transform.position.z);
	}
	
	public void SpawnPlayer() {
		if (a < 4) {
			Vector2 spawnPoint = GameEngine.GetCheckPoint() != "" ? GameObject.Find(GameEngine.GetCheckPoint()).transform.position : transform.position;

			GameObject newObj = Network.Instantiate(player, new Vector3(spawnPoint.x + Random.Range(-5, 5), spawnPoint.y, 0), Quaternion.identity, 0) as GameObject;
			NetworkServer.Spawn(newObj);
			newObj.GetComponentInChildren<SpriteRenderer>().material = mt[a];

			if (a == 0) {
				newObj.GetComponent<PlayerController>().CmdRegist(a, hostIntSize);
				GameEngine.direct.Focus(newObj.GetComponent<PlayerController>());
				GameEngine.direct.StartStage();	
			} else {
				newObj.GetComponent<PlayerController>().CmdRegist(a, intSize);
			}

			a++;
		}
	}

	public void Pause() {
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}

	public GameObject SpawnUnit(GameObject spawnUnit, Vector2 spawnPoint ) {
		GameObject newObj = Network.Instantiate(spawnUnit, spawnPoint, Quaternion.identity, 0) as GameObject;
		NetworkServer.Spawn(newObj);
		return newObj;
	}
}
