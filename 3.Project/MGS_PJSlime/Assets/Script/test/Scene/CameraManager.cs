using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public float maxSpeed;
	public float minSpeed;

	private void Update() {
		if (GameEngine.mainPlayer) {
			float speed = Mathf.Sqrt(Vector2.Distance(transform.position, GameEngine.mainPlayer.transform.position));

			if (speed > maxSpeed) {
				speed = maxSpeed;
			} else if(speed < minSpeed) {
				speed = minSpeed;
			}

			transform.position = Vector3.Lerp(transform.position , new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y, transform.position.z) , Time.deltaTime * speed);
			GameEngine.direct.KillBorder(transform.position);
		}
	}
}
