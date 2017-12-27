using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : NetworkBehaviour {
	public Vector2 shift = new Vector2(0, 0);

	protected Vector2 a = new Vector2(0, 0);
	protected Vector2 b = new Vector2(0, 0);

	protected bool target = true;

	void Start() {
		a = (Vector2)transform.position + shift;
		b = (Vector2)transform.position - shift;
	}
	
	void Update () {
		transform.position = Vector2.Lerp(transform.position, target ? a : b, 0.05f);
		if (Vector2.Distance(transform.position, target ? a : b) < 0.1f) {
			target = !target;
		}
	}
}
