using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
	public Vector2 shift;
	private RectTransform image;
	private Vector2 origin;

	private void Start() {
		image = GetComponent<RectTransform>();
		origin = image.anchoredPosition;
	}

	private void Update() {
		if (GameEngine.mainPlayer) {
			image.anchoredPosition = new Vector3(CameraManager.obj.transform.position.x * -shift.x + origin.x, CameraManager.obj.transform.position.y * -shift.y + origin.y, transform.position.z);
		}
	}
}
