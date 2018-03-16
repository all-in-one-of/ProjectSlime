using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PatrolBase : GearBase {
	public Vector2 vector = new Vector2(0, 0);
	public float onceTime = 5;
	public bool accMode = true;
	public bool carryMode = false;

	protected List<Transform> carryobj = new List<Transform>();
	protected Vector2 max = new Vector2(0, 0);
	protected Vector2 min = new Vector2(0, 0);
	public bool positive = true;

	float aa = 0;

	void Start() {
		max = (Vector2)transform.position + vector * onceTime * 0.5f;
		min = (Vector2)transform.position - vector * onceTime * 0.5f;

		if (!active) {
			transform.position = positive ? max : min;
		}
	}
	
	void FixedUpdate() {
		if (positive && (transform.position.x > max.x || transform.position.y > max.y)) {

		} else if (!positive && (transform.position.x < min.x || transform.position.y < min.y)) {

		} else {
			Vector2 shift = (positive ? vector : -vector) * Time.deltaTime;
			transform.position = (Vector2)transform.position + shift;
			CarryObj(shift);
		}

		

		if (!accMode) {

		} else {

			/*
			Vector2 shift = transform.position;
			transform.position = Vector2.Lerp(transform.position, target ? a : b, speed);
			shift = (Vector2)transform.position - shift;
			CarryObj(shift);*/
		}
		
		if (transform.position.x > max.x || transform.position.y > max.y || transform.position.x < min.x || transform.position.y < min.y) {
			if (active || IsTriggering()) {
				Trigger();
			}
		}
	}

	public override bool BaseTrigger() {
		positive = !positive;
		return true;
	}

	public override bool Trigger() {
		return BaseTrigger();
	}

	protected void CarryObj(Vector2 shift) {
		if (carryobj.Count > 0) {
			foreach (Transform obj in carryobj) {
				obj.position = (Vector2)obj.position + shift;
			}
		}
	}	
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (carryMode && !carryobj.Contains(collision.transform)) {
			carryobj.Add(collision.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (carryMode && carryobj.Contains(collision.transform)) {
			carryobj.Remove(collision.transform);
		}
	}
}
