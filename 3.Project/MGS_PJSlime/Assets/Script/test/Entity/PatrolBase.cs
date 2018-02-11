using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : NetworkBehaviour {
	public bool carryAble = false;
	public bool floatAble = true;
	public bool breakAble2 = false;
	public Vector2 shift = new Vector2(0, 0);
	public float speed = 0.05f;
	public float crackTimer;
	public float resetTimer;

	protected Vector2 a = new Vector2(0, 0);
	protected Vector2 b = new Vector2(0, 0);

	protected bool target = true;
	protected float clock = 0;
	protected bool breaking = false;
	protected Vector2 size;
	public Transform carryobj = null;

	void Start() {
		a = (Vector2)transform.position + shift;
		b = (Vector2)transform.position - shift;
		size = transform.localScale;
	}
	
	void Update () {
		if (floatAble) {
			Vector2 offset = transform.position;
			transform.position = Vector2.Lerp(transform.position, target ? a : b, speed);

			offset = (Vector2)transform.position - offset;
			if (carryobj) {
				carryobj.position = (Vector2)carryobj.position + offset;
			}

			if (Vector2.Distance(transform.position, target ? a : b) < 0.1f) {
				target = !target;
			}
		} else if(breakAble2) {
			if (breaking && Time.timeSinceLevelLoad - clock > resetTimer + crackTimer) {
				UnBreak();
			}
		}		
	}

	public void Break() {
		breaking = true;
		clock = Time.timeSinceLevelLoad;
		Invoke("RealBreal", crackTimer);
	}

	public void RealBreal() {
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
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (carryAble) {
			carryobj = collision.transform;
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (carryAble) {
			carryobj = null;
		}
	}
	/*
	void ConnectTo(Rigidbody2D character) {
		SliderJoint2D joint = GetComponent<SliderJoint2D>();
		joint.connectedBody = character;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (carryAble && collision.collider.tag == "Slime") {
			ConnectTo(collision.collider.GetComponent<Rigidbody2D>());
		}
	}*/
}
