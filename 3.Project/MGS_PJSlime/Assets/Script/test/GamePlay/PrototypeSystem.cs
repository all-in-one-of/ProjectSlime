using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrototypeSystem : NetworkBehaviour {
	public GameObject player;
	public GameObject[] obj;
	public int a = 0;
	public int clock;

	void Update () {
		if (isServer && GameEngine.direct.connecting) {
			if (transform.childCount < 5 && clock >= 30) {
				
				GameObject newObj = Network.Instantiate(obj[Random.Range(0, obj.Length)], new Vector3(transform.position.x + Random.Range(-15, 15), transform.position.y, 0), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(newObj);
				newObj.transform.SetParent(transform);
				newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);
				clock = 0;
			}
			clock++;
		}
	}

	public void SPlayer2() {
		if (a < 4) {
			GameObject newObj = Network.Instantiate(player, new Vector3(transform.position.x + Random.Range(-15, 15), transform.position.y, 0), Quaternion.identity, 0) as GameObject;
			NetworkServer.Spawn(newObj);
			newObj.GetComponent<PlayerController>().CmdRegist(a);
			a++;
		}		
	}
}
