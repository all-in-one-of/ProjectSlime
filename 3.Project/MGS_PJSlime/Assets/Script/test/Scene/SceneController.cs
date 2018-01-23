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
		if (GameEngine.mainPlayer) {
			transform.position = new Vector3(GameEngine.mainPlayer.transform.position.x * shift.x + origin.x, GameEngine.mainPlayer.transform.position.y * shift.y + origin.y, transform.position.z);
		}
	}
}
