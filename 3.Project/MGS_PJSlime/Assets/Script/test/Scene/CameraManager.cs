using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static CameraManager direct;
	public static Transform nowCamera;

	public AnimationCurve speedCurve;
	public Transform mainCamera;
	public Transform targetHint;
	public SpriteRenderer hintSprite;

	public float yOffset = 5;

	private void Start() {
		direct = this;
	}

	public void Init() {
		nowCamera = mainCamera;

		foreach (Transform ui in mainCamera.transform) {
			GameEngine.direct.playerUIs.Add(ui.gameObject);
		}		
	}

	private void Update() {
		if (!GameEngine.mainPlayer) {
			return;
		}

		float mainSpeed = speedCurve.Evaluate(Vector2.Distance(nowCamera.position, (Vector2)GameEngine.mainPlayer.transform.position + new Vector2(0, yOffset)));
		float hintSpeed = -Vector2.Distance(targetHint.position, GameEngine.mainPlayer.transform.position) * 5 + 50;
		
		
		if (hintSpeed < 5) {
			hintSpeed = 5;
		}

		nowCamera.position = Vector3.Lerp(nowCamera.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y + yOffset, nowCamera.transform.position.z), Time.deltaTime * mainSpeed);
		targetHint.position = Vector3.Lerp(targetHint.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y, targetHint.transform.position.z), Time.deltaTime * hintSpeed);
		hintSprite.size = new Vector2(GameEngine.mainPlayer.transform.localScale.x * 2.5f, GameEngine.mainPlayer.transform.localScale.x * 2.5f);

		GameEngine.direct.KillBorder(nowCamera.transform.position);
	}
}
