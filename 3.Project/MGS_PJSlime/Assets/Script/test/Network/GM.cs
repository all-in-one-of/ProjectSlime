using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour {
	public List<PlayerController> allPlayer = new List<PlayerController>();

	public void Login(PlayerController player) {
		allPlayer.Add(player);
		//player.RpcSetPlayer();
	}
}