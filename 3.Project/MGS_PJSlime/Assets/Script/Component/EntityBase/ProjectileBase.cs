using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : EntityBase {
	public float nowSpeed;
	public float lifeTime = 10;
	private float lifeClock;
	private bool active;

	protected void OnTriggerEnter2D(Collider2D collider) {
		if (Network.isServer) {
			FOnTriggerEnter2D(collider);
		}
	}

	protected override void FStart() {
		base.FStart();
		if (Network.isServer) {

		}
	}

	public void FireProjectile(float speed , float life) {
		lifeClock = Time.timeSinceLevelLoad;
		lifeTime = life;
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

	protected void FOnTriggerEnter2D(Collider2D collision) {
		EntityBase other = collision.gameObject.GetComponent<EntityBase>();
		if (other && attack > 0) {
			if (collision.transform.tag == "Slime") {
				other.Attack(attack);
				Destroy(gameObject);
			}
		}
	}

	protected override void FOnCollisionEnter2D(Collision2D collision) { }

	protected override void FOnCollisionStay2D(Collision2D collision) { }

	protected virtual void LifeTimeSeqence() {
		if (lifeTime < Time.timeSinceLevelLoad - lifeClock) {
			Destroy(gameObject);
		}
	}
}
