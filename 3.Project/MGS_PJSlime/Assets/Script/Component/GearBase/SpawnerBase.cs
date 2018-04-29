using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnerBase : GearBase {
	public List<GameObject> spawnObject = new List<GameObject>();
	public Vector2 spawnOffset;
	public Vector2 acGape = new Vector2(2, 4);
	public int limit = 5;

	protected GameObject[] spawnedObject;
	protected float acClock;


	protected override void FStart() {
		spawnedObject = new GameObject[limit];

		if (triggerType != TriggerType.always) {
			active = false;
		}
	}

	void Update() {
		if (isServer && GameEngine.direct.connecting ) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {
				if (triggerType == TriggerType.continuous && !IsTriggering()) {
					return;
				}
				AISeqence();
			}
		}
	}

	public override bool BaseTrigger() {
		SpawnObject();
		return true;
	}

	public override bool Trigger() {
		if (limit > 0) {
			for (int i = 0; i < spawnedObject.Length; i++) {
				if (spawnedObject[i] == null) {
					return BaseTrigger();
				}
			}
			return false;
		}
		return BaseTrigger();
	}

	protected virtual void AISeqence() {
		if (acClock < Time.timeSinceLevelLoad) {
			if (Trigger()) {
				acClock = Random.Range(acGape.x, acGape.y) + Time.timeSinceLevelLoad;
			}
		}
	}

	protected void SpawnObject() {		
		for (int i = 0; i < spawnedObject.Length; i++) {
			if (spawnedObject[i] == null) {
				spawnedObject[i] = Network.Instantiate(spawnObject[Random.Range(0, spawnObject.Count)], new Vector3(transform.position.x + Random.Range(-spawnOffset.x, spawnOffset.x), transform.transform.position.y, +Random.Range(-spawnOffset.y, spawnOffset.y)), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(spawnedObject[i]);
				spawnedObject[i].transform.SetParent(GameEngine.direct.units);
				spawnedObject[i].GetComponent<NetworkIdentity>().RebuildObservers(true);
				if (triggerType == TriggerType.once) {
					active = false;
				}
				break;
			}
		}
	}
}
