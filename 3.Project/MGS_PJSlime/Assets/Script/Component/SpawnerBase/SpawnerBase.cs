using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnerBase : NetworkBehaviour {
	public List<GameObject> spawnObject = new List<GameObject>();
	public Vector2 spawnOffset;
	public Vector2 acGape = new Vector2(2, 4);

	private float acClock;
	void Update() {
		if (isServer && GameEngine.direct.connecting) {
			AISeqence();
		}
	}
	

	protected virtual void AISeqence() {
		if (acClock < Time.timeSinceLevelLoad) {
			GameObject newObj = Network.Instantiate(spawnObject[Random.Range(0, spawnObject.Count)], new Vector3(transform.position.x + Random.Range(-spawnOffset.x, spawnOffset.x), transform.transform.position.y, +Random.Range(-spawnOffset.y, spawnOffset.y)), Quaternion.identity, 0) as GameObject;
			NetworkServer.Spawn(newObj);
			newObj.transform.SetParent(GameEngine.direct.units);
			newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);

			acClock = Random.Range(acGape.x, acGape.y) + Time.timeSinceLevelLoad;
		}
	}
}
