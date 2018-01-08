using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : NetworkBehaviour {
	public bool floatAble = true;
	public bool breakAble2 = false;
	public Vector2 shift = new Vector2(0, 0);
	public float speed = 0.05f;
	public float resetTimer;

	protected Vector2 a = new Vector2(0, 0);
	protected Vector2 b = new Vector2(0, 0);

	protected bool target = true;
	protected float clock = 0;
	protected bool breaking = false;
	protected Vector2 size;

	void Start() {
		a = (Vector2)transform.position + shift;
		b = (Vector2)transform.position - shift;
		size = transform.localScale;
	}
	
	void Update () {

		if (floatAble) {
			transform.position = Vector2.Lerp(transform.position, target ? a : b, speed);
			if (Vector2.Distance(transform.position, target ? a : b) < 0.1f) {
				target = !target;
			}
		} else if(breakAble2) {
			if (breaking && Time.timeSinceLevelLoad - clock > resetTimer) {
				UnBreak();
			}
		}		
	}

	public void Break() {
		breaking = true;
		clock = Time.timeSinceLevelLoad;
		transform.localScale = Vector2.zero;
	}

	public void UnBreak() {
		breaking = false;
		transform.localScale = size;
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (breakAble2 && !breaking) {
			Break();
		}
	}
}
