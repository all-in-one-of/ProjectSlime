using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
	public float shift = 0.1f;

	private void Update() {
		if (GameEngine.direct.player) {
			transform.position = new Vector3(GameEngine.direct.player.position.x * shift, GameEngine.direct.player.position.y * shift, transform.position.z);
		}
	}
}
