using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrototypeSystem : NetworkBehaviour {
	public GameObject[] obj;
	public int clock;

	void Update () {
		if (isServer) {
			if (transform.childCount < 5 && clock >= 30) {
				
				GameObject newObj = Network.Instantiate(obj[Random.Range(0, obj.Length)], new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, 0), Quaternion.identity, 0) as GameObject;
				newObj.transform.SetParent(transform);
				clock = 0;
			}

			clock++;
		}
	}
}
