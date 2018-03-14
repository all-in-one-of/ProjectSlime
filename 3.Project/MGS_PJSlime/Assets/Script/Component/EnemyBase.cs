using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EntityBase {
	public bool moveAI;
	public bool jumpAI;
	public bool shootAI;

	public float moveShift;
	public float moveSpeed;
	public float jumpForce;

	public GameObject bullet;
	public float bulletSpeed;

	public  Vector2 aiGape = new Vector2(2,4);
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
		if (isDead ) {
			return;
		}
		AISeqence();
		rb.velocity = new Vector2(nowSpeed, rb.velocity.y);
	}

	//rb.AddForce(Vector2.up * GameEngine.direct.jumpYForce * ((jumpGape - size) / jumpGape), ForceMode2D.Impulse);

	protected virtual void AISeqence() {
		if (aiClock < Time.timeSinceLevelLoad) {
			if (jumpAI) {
				if (Random.Range(0, 2) == 0) {
					rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				}
			}

			if (moveAI) {
				if (transform.position.x < movePos - moveShift) {
					nowSpeed = moveSpeed;

				} else if (transform.position.x > movePos + moveShift) {
					nowSpeed = -moveSpeed;

				} else {
					nowSpeed = Random.Range(0, 2) == 0 ? moveSpeed : -moveSpeed;
				}
			}

			if (shootAI) {
				if (Random.Range(0, 2) == 0) {
					if (Random.Range(0, 2) == 0) {
						GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x - 1, transform.position.y));
						newBullet.GetComponent<ProjectileBase>().FireProjectile(-bulletSpeed);
					} else {
						GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x + 1, transform.position.y));
						newBullet.GetComponent<ProjectileBase>().FireProjectile(bulletSpeed);
					}
				}
			}

			aiClock = Random.Range(aiGape.x, aiGape.y) + Time.timeSinceLevelLoad;
		}
	}
}
