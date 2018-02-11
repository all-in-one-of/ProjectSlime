using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EntityBase {
	public bool moveAI;
	public float moveShift;
	public float moveSpeed;

	private float movePos;
	private float aiClock;
	private float nowSpeed;

	protected override void FStart() {
		base.FStart();
		if (Network.isServer) {
			movePos = transform.position.x;
		}
	}

	protected override void FFixedUpdate() {
		if (isDead || !moveAI) {
			return;
		}
		AISeqence();

		rb.velocity = new Vector2(nowSpeed, rb.velocity.y);
	}

	protected virtual void AISeqence() {
		if (aiClock < Time.timeSinceLevelLoad) {
			if (transform.position.x < movePos - moveShift) {
				nowSpeed = moveSpeed;

			} else if (transform.position.x > movePos + moveShift) {
				nowSpeed = -moveSpeed;

			} else {
				nowSpeed = Random.Range(0, 2) == 0 ? moveSpeed : -moveSpeed;
			}
			aiClock = Random.Range(2, 5) + Time.timeSinceLevelLoad;
		}
	}
}
