using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EntityBase {
	public bool moveAI;
	public bool agressiveAI;
	public bool jumpAI;
	public bool shootAI;
	
	public float moveShift;
	public float moveSpeed;
	public float jumpForce;

	public GameObject bullet;

	public  Vector2 aiGape = new Vector2(2,4);
	private float movePos;
	private float aiClock;

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
		if (agressiveAI || moveAI) {
			rb.velocity = new Vector2(face * moveSpeed, rb.velocity.y);
		}
	}
	
	protected virtual void AISeqence() {
		if (aiClock < Time.timeSinceLevelLoad) {
			if (agressiveAI) {
				if (GameEngine.mainPlayer) {
					if (transform.position.x < movePos - moveShift) {
						face = 1;

					} else if (transform.position.x > movePos + moveShift) {
						face = -1;

					} else {
						if (GameEngine.mainPlayer.transform.position.x > transform.position.x) {
							face = 1;
						} else {
							face = -1;
						}
					}
				}

			} else if (moveAI) {
				if (transform.position.x < movePos - moveShift) {
					face = 1;

				} else if (transform.position.x > movePos + moveShift) {
					face = -1;

				} else {
					face = Random.Range(0, 2) == 0 ? 1 : -1;
				}
			}


			if (jumpAI) {
				if (Random.Range(0, 2) == 0) {
					rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				}
			}

			

			if (shootAI) {
				if (Random.Range(0, 2) == 0) {
					if (Random.Range(0, 2) == 0) {
						GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x - 1, transform.position.y));
						newBullet.transform.SetParent(GameEngine.nowStage.unitSet);
						newBullet.GetComponent<ProjectileBase>().FireProjectile(new Vector2(-1,0));
					} else {
						GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x + 1, transform.position.y));
						newBullet.transform.SetParent(GameEngine.nowStage.unitSet);
						newBullet.GetComponent<ProjectileBase>().FireProjectile(new Vector2(1, 0));
					}
				}
			}

			aiClock = Random.Range(aiGape.x, aiGape.y) + Time.timeSinceLevelLoad;
		}
	}
}
