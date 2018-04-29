using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static Transform nowCamera;

	public GameObject mainCamera;
	public Transform targetHint;
	public SpriteRenderer hintSprite;

	public float maxSpeed;
	public float minSpeed;

	private void Start() {
		GameObject temp = GameObject.Instantiate(mainCamera);
		temp.transform.SetParent(transform);
		nowCamera = temp.transform;
	}

	private void Update() {
		if (!GameEngine.mainPlayer) {
			return;
		}

		float mainSpeed = Mathf.Sqrt(Vector2.Distance(nowCamera.position, GameEngine.mainPlayer.transform.position));
		float hintSpeed = -Vector2.Distance(targetHint.position, GameEngine.mainPlayer.transform.position) * 5 + 50;

		if (mainSpeed > maxSpeed) {
			mainSpeed = maxSpeed;
		} else if (mainSpeed < minSpeed) {
			mainSpeed = minSpeed;
		}

		if (hintSpeed < 5) {
			hintSpeed = 5;
		}

		nowCamera.position = Vector3.Lerp(nowCamera.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y + 5, mainCamera.transform.position.z), Time.deltaTime * mainSpeed);
		targetHint.position = Vector3.Lerp(targetHint.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y, targetHint.transform.position.z), Time.deltaTime * hintSpeed);
		hintSprite.size = new Vector2(GameEngine.mainPlayer.transform.localScale.x * 2.5f, GameEngine.mainPlayer.transform.localScale.x * 2.5f);

		GameEngine.direct.KillBorder(mainCamera.transform.position);
	}
}
