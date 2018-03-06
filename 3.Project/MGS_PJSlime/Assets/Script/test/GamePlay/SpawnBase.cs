using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnBase : NetworkBehaviour {
	public GameObject[] spawnObject;
	public Vector2 offset;

	public int clock = 200;
	
	void Update() {
		if (isServer && GameEngine.direct.connecting) {
			if (GameEngine.direct.units.childCount < 40 && clock >= 120) {

				GameObject newObj = Network.Instantiate(spawnObject[Random.Range(0, spawnObject.Length)], new Vector3(transform.position.x + Random.Range(-offset.x, offset.x), transform.transform.position.y, +Random.Range(-offset.y, offset.y)), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(newObj);
				newObj.transform.SetParent(GameEngine.direct.units);
				newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);
				clock = 0;
			}
			clock++;
		}
	}	
}
