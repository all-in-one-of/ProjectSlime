using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeSystem : NetworkBehaviour {
	public static PrototypeSystem direct;
	public GameObject player;
	protected int playerCount = 0;
	
	private void Start() {
		direct = this;
	}
	
	public void Pause() {
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}

	public GameObject SpawnUnit(GameObject spawnUnit, Vector2 spawnPoint ) {
		GameObject newObj = Instantiate(spawnUnit, spawnPoint, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(newObj);
		return newObj;
	}
}
