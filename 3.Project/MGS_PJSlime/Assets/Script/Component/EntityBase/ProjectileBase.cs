using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : EntityBase {
	public float nowSpeed;
	public float lifeTime = 10;
	public bool isCruise = false;
	public float cruiseRate = 0.5f;

	private float constSpeed;
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

	public void FireProjectile(float speed , float life ) {
		lifeClock = Time.timeSinceLevelLoad;
		lifeTime = life;
		constSpeed = Mathf.Abs(speed);
		nowSpeed = speed;
		active = true;
		rb.velocity = new Vector2(nowSpeed, rb.velocity.y);
	}

	protected override void FFixedUpdate() {
		if (isDead ) {
			return;
		}

		if (!active) {
			return;
		}

		LifeTimeSeqence();

		if (!isCruise) {
			rb.velocity = new Vector2(nowSpeed, rb.velocity.y);
		} else {
			rb.velocity = ((rb.velocity.normalized * (1 - cruiseRate) + ((Vector2)(GameEngine.mainPlayer.transform.position - transform.position)).normalized) * cruiseRate) * constSpeed;
		}		
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
