using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EntityBase : NetworkBehaviour {
	public bool isInvincible = false;
	public bool isUndead = false;
	public bool eatAble = true;
	public bool isDead = false;
	public float invincibleTimer;


	public int bonus = 0;
	public int attack = 1;
	public int hp = 3;
	public int face = 1;

	protected Animator an;
	protected Rigidbody2D rb;
	protected BoxCollider2D bc;

	void Start() {
		FStart();
	}
	
	void FixedUpdate() {
		if (Network.isServer) {
			FFixedUpdate();
		}
	}

	protected virtual void FStart() {
		an = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();

		if (Network.isServer) {
			rb.simulated = true;
		}
	}

	protected virtual void FFixedUpdate() {
		
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (Network.isServer) { FOnCollisionEnter2D(collision); }
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (Network.isServer) { FOnCollisionStay2D(collision); }
	}

	protected virtual void FOnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Dead" && !isUndead) {
			OnDead();
		}
	}

	protected virtual void FOnCollisionStay2D(Collision2D collision) {
		EntityBase other = collision.gameObject.GetComponent<EntityBase>();
		if (other && attack > 0) {
			other.Attack(attack);
		}
	}

	public virtual void Attack(int damage, bool firstOrder = false) {
		if (!isInvincible || firstOrder) {
			hp--;
			isInvincible = true;
			if (hp == 0) {
				OnDead();
			}
		}
	}

	public virtual void OnDead() {
		isDead = true;
		bc.enabled = false;
		rb.simulated = false;
		if (an) {
			an.Play("Eaten");
		}
		Invoke("temp", 0.6f);
	}

	private void temp() {
		Destroy(gameObject);
	}
}
