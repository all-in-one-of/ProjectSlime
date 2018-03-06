using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : EntityBase {
	public float nowSpeed;
	private float lifeClock;
	private bool active;

	protected override void FStart() {
		base.FStart();
		if (Network.isServer) {

		}
	}

	public void FireProjectile(float speed) {
		lifeClock = Time.timeSinceLevelLoad;
		nowSpeed = speed;
		active = true;
	}

	protected override void FFixedUpdate() {
		if (isDead ) {
			return;
		}

		if (!active) {
			return;
		}

		LifeTimeSeqence();

		rb.velocity = new Vector2(nowSpeed, rb.velocity.y);
	}

	protected override void FOnCollisionStay2D(Collision2D collision) {
		EntityBase other = collision.gameObject.GetComponent<EntityBase>();
		if (other && attack > 0) {
			if (collision.transform.tag == "Slime") {
				other.Attack(attack);
			}			
			Destroy(gameObject);
		}
	}

	protected virtual void LifeTimeSeqence() {
		if (10 < Time.timeSinceLevelLoad - lifeClock) {
			Destroy(gameObject);
		}
	}
}
