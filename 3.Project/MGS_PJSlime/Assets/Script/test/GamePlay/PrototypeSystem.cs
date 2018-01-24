using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PrototypeSystem : NetworkBehaviour {
	public NetworkManager networkManager;
	public GameObject spawnPoint;

	public GameObject player;
	public GameObject[] obj;
	public Material[] mt;
	public int a = 0;
	public int clock;

	public int hostIntSize;
	public int intSize;
	public int jumpGape;

	void Update () {
		if (isServer && GameEngine.direct.connecting) {
			if (transform.childCount < 20 && clock >= 30) {
				
				GameObject newObj = Network.Instantiate(obj[Random.Range(0, obj.Length)], new Vector3(Random.Range(0, 100), transform.position.y, 0), Quaternion.identity, 0) as GameObject;
				NetworkServer.Spawn(newObj);
				newObj.transform.SetParent(transform);
				newObj.GetComponent<NetworkIdentity>().RebuildObservers(true);
				clock = 0;
			}
			clock++;
		}
	}

	public void SPlayer2(EventSystem set) {
		if (a < 4) {
			GameObject newObj = Network.Instantiate(player, new Vector3(spawnPoint.transform.position.x + Random.Range(-5, 5), spawnPoint.transform.position.y , 0), Quaternion.identity, 0) as GameObject;
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

	public void ResetScene(EventSystem set) {
		networkManager.StopHost();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		set.SetSelectedGameObject(gameObject);
	}
}
