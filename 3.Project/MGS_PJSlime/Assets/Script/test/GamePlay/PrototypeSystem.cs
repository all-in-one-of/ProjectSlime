using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeSystem : NetworkBehaviour {
	public static PrototypeSystem direct;
	public Transform sp;
	public GameObject player;
	public GameObject[] obj;
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

	void Update () {
		if (isServer && GameEngine.direct.connecting) {
			if (GameEngine.direct.units.childCount < 40 && clock >= 120) {
				
				GameObject newObj = Network.Instantiate(obj[Random.Range(0, obj.Length)], new Vector3(sp.position.x + Random.Range(-50, 50),sp.transform.position.y, 0), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(newObj);
				newObj.transform.SetParent(GameEngine.direct.units);
				newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);
				clock = 0;
			}
			clock++;
		}
	}

	void OnServerInitialized() {
		SpawnPlayer();
	}

	public void SpawnPlayer(EventSystem set) {
		SpawnPlayer();
		set.SetSelectedGameObject(gameObject);
	}

	public void SpawnPlayer() {
		if (a < 4) {
			Vector2 spawnPoint = GameEngine.GetCheckPoint() != "" ? GameObject.Find(GameEngine.GetCheckPoint()).transform.position : transform.position;

			GameObject newObj = Network.Instantiate(player, new Vector3(spawnPoint.x + Random.Range(-5, 5), spawnPoint.y, 0), Quaternion.identity, 0) as GameObject;
			NetworkServer.Spawn(newObj);
			newObj.GetComponentInChildren<SpriteRenderer>().material = mt[a];

			if (a == 0) {
				newObj.GetComponent<PlayerController>().CmdRegist(a, hostIntSize, jumpGape);
				GameEngine.direct.Focus(newObj.GetComponent<PlayerController>());
			} else {
				newObj.GetComponent<PlayerController>().CmdRegist(a, intSize, jumpGape);
			}

			a++;

		}
	}
}
