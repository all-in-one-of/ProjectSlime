using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
	public Vector2 shift;
	private Vector2 origin;

	private void Start() {
		origin = transform.localPosition;
	}

	private void Update() {
		if (GameEngine.direct.player) {
			transform.position = new Vector3(GameEngine.direct.player.transform.position.x * shift.x + origin.x, GameEngine.direct.player.transform.position.y * shift.y + origin.y, transform.position.z);
		}
	}
}
