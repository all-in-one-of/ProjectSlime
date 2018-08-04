using Spine.Unity;
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
	private string state = "";



	protected override void FStart() {
		base.FStart();
		if (Network.isServer) {
			movePos = transform.position.x;

			skam = GetComponent<SkeletonAnimation>();
			if (skam) {
				skam.state.SetAnimation(0, "walk", true);
			}			
		}
	}

	protected override void FFixedUpdate() {
		if (isDead ) {
			return;
		}
		AISeqence();
		if (agressiveAI || moveAI) {
			rb.velocity = new Vector2(facing * moveSpeed, rb.velocity.y);
		}
	}
	
	protected virtual void AISeqence() {
		if (aiClock < Time.timeSinceLevelLoad) {
			//轉向判斷
			if (agressiveAI) {
				if (transform.position.x < movePos - moveShift) {
					Face(true);

				} else if (transform.position.x > movePos + moveShift) {
					Face(false);

				} else {
					if (GameEngine.mainPlayer) {
						Face(GameEngine.mainPlayer.transform.position.x > transform.position.x);
					}
				}

			} else if (moveAI) {
				if (transform.position.x < movePos - moveShift) {
					Face(true);

				} else if (transform.position.x > movePos + moveShift) {
					Face(false);

				} else {
					Face(Random.Range(0, 2) == 0);
				}
			}

			//沒事才能做事
			if (IsActionAble()) {
				if (jumpAI ) {
					if (Random.Range(0, 2) == 0) {
						rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

						if (skam) {
							state = "jump";
							skam.state.SetAnimation(0, "jump", false).End += delegate {
								skam.state.SetAnimation(0, "walk", true);
								state = "";
							};
						}
					}
				}

				if (shootAI ) {
					if (Random.Range(0, 2) == 0) {
						if (Random.Range(0, 2) == 0) {
							GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x - 1, transform.position.y));
							newBullet.transform.SetParent(GameEngine.nowStage.unitSet);
							newBullet.GetComponent<ProjectileBase>().FireProjectile(new Vector2(-1, 0));
						} else {
							GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullet, new Vector2(transform.position.x + 1, transform.position.y));
							newBullet.transform.SetParent(GameEngine.nowStage.unitSet);
							newBullet.GetComponent<ProjectileBase>().FireProjectile(new Vector2(1, 0));
						}
						
						if (skam) {
							state = "attack";
							Spine.TrackEntry entry = skam.state.SetAnimation(0, "attack", false);
							skam.state.AddAnimation(0, "walk", true, 0f);
							entry.End += delegate {
								state = "";
							};
						} 
					}
				}
			}
			
			aiClock = Random.Range(aiGape.x, aiGape.y) + Time.timeSinceLevelLoad;
		}
	}

	public bool IsActionAble() {
		return state == "";
	}

	public void SetAction() {

	}

	public void Face(bool right) {
		facing = right ? 1 : -1;
		transform.Rotate(0 , right ? 0 : 180, 0);
	}
}
