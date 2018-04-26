using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBalancer : MonoBehaviour {

	public float balanceAngle;

	private void Update() {
		if (transform.rotation.z > 0) {
			if (transform.rotation.z > 0.7f) {
				transform.Rotate(new Vector3(0, 0, balanceAngle) * Time.deltaTime);
			} else {
				transform.Rotate(new Vector3(0, 0, -balanceAngle) * Time.deltaTime);
			}
		} else {
			if (transform.rotation.z < -0.7f) {
				transform.Rotate(new Vector3(0, 0, -balanceAngle) * Time.deltaTime);
			} else {
				transform.Rotate(new Vector3(0, 0, balanceAngle) * Time.deltaTime);
			}
		}

		
	}
}
