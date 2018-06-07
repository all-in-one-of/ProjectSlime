using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static CameraManager direct;
	public static Transform nowCamera;

	public AnimationCurve speedCurve;
	public AnimationCurve focusCurve;

	public Camera mainCamera;
	public Transform targetHint;
	public SpriteRenderer hintSprite;
	public float focusPSpeed = 2;
	public float focusNSpeed = 2;
	public float preFocusSpeed;

	public float yOffset = 5;

	public AnimationCurve bumpCurve;
	public float bumpTime = 1;
	public float bumpTimer = 0;


	private void Start() {
		direct = this;
	}

	public void Init() {
		nowCamera = mainCamera.transform;

		foreach (Transform ui in nowCamera.transform) {
			GameEngine.direct.playerUIs.Add(ui.gameObject);
		}		
	}

	private void Update() {
		if (!GameEngine.mainPlayer) {
			return;
		}

		//float cSpeed = GameEngine.mainPlayer.GetSpeed();

		float mainSpeed = speedCurve.Evaluate(Vector2.Distance(nowCamera.position, (Vector2)GameEngine.mainPlayer.transform.position + new Vector2(0, yOffset)));
		float hintSpeed = -Vector2.Distance(targetHint.position, GameEngine.mainPlayer.transform.position) * 5 + 50;
		
		
		if (hintSpeed < 5) {
			hintSpeed = 5;
		}
		
		nowCamera.position = Vector3.Lerp(nowCamera.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y + yOffset, nowCamera.transform.position.z), Time.deltaTime * mainSpeed);
		targetHint.position = Vector3.Lerp(targetHint.position, new Vector3(GameEngine.mainPlayer.transform.position.x, GameEngine.mainPlayer.transform.position.y, targetHint.transform.position.z), Time.deltaTime * hintSpeed);

		/*
		float focusSpeed = focusCurve.Evaluate(mainSpeed);

		if (focusSpeed > preFocusSpeed) {
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 16 + focusCurve.Evaluate(mainSpeed), Time.deltaTime * focusPSpeed);
		} else {
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 16 + focusCurve.Evaluate(mainSpeed), Time.deltaTime * focusNSpeed);
		}
		preFocusSpeed = focusSpeed;*/


		if (bumpTimer > 0) {
			float targetBump = bumpCurve.Evaluate(bumpTime - bumpTimer);
			
			if (16 + targetBump > mainCamera.orthographicSize) {
				mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 16 + targetBump,  focusPSpeed);
			} else {
				mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 16 + targetBump * 0.5f, focusPSpeed);
			}
			bumpTimer = bumpTimer - Time.deltaTime;
		} else {

			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 16 ,  focusNSpeed);
		}

		hintSprite.size = new Vector2(GameEngine.mainPlayer.transform.localScale.x * 2.5f, GameEngine.mainPlayer.transform.localScale.x * 2.5f);
		GameEngine.direct.KillBorder(nowCamera.transform.position);
	}

	public void Bump() {
		bumpTimer = bumpTime;
	}
}
