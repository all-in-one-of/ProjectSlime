using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeSystem : NetworkBehaviour {
	public GameObject player;
	public GameObject[] obj;
	public Material[] mt;
	public int a = 0;
	public int clock;

	public int hostIntSize;
	public int intSize;
	public int jumpGape;
	public bool order = false;
		

	void Update () {
		if (isServer && GameEngine.direct.connecting) {
			if (GameEngine.direct.units.childCount < 20 && clock >= 30) {
				
				GameObject newObj = Network.Instantiate(obj[Random.Range(0, obj.Length)], new Vector3(Random.Range(0, 100), transform.position.y, 0), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(newObj);
				newObj.transform.SetParent(GameEngine.direct.units);
				newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);
				clock = 0;
			}
			clock++;
		}
	}

	public void SPlayer2(EventSystem set) {
		if (a < 4) {
			Vector2 spawnPoint = GameEngine.GetCheckPoint() != "" ? GameObject.Find(GameEngine.GetCheckPoint()).transform.position : transform.position;

			GameObject newObj = Network.Instantiate(player, new Vector3(spawnPoint.x + Random.Range(-5, 5), spawnPoint.y , 0), Quaternion.identity, 0) as GameObject;
			NetworkServer.Spawn(newObj);
			newObj.GetComponentInChildren<SpriteRenderer>().material = mt[a];

			if (a == 0) {
				newObj.GetComponent<PlayerController>().CmdRegist(a, hostIntSize , jumpGape);
				GameEngine.direct.Focus(newObj.GetComponent<PlayerController>());
			} else {
				newObj.GetComponent<PlayerController>().CmdRegist(a, intSize , jumpGape);
			}

			a++;
			set.SetSelectedGameObject(gameObject);
		}
	}
}
