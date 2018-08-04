using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EntityBase : NetworkBehaviour {
	protected SkeletonAnimation skam;
	protected string state = "";

	public BufferEffect buffer = new BufferEffect();

	public bool isInvincible = false;
	public bool isUndead = false;
	public bool eatAble = true;
	public bool eatBuffer = false;
	public bool isDead = false;
	public float invincibleTimer;


	public int bonus = 0;
	public int attack = 1;
	public int hp = 3;
	public int facing = 1;

	protected Animator an;
	protected Rigidbody2D rb;
	protected BoxCollider2D bc;

	
	void Start() {
		if (Network.isServer) {
			an = GetComponentInChildren<Animator>();
			rb = GetComponent<Rigidbody2D>();
			bc = GetComponent<BoxCollider2D>();
			skam = GetComponent<SkeletonAnimation>();
			rb.simulated = true;
			FStart();
		}		
	}
	
	void FixedUpdate() {
		if (Network.isServer) {
			FFixedUpdate();
		}
	}

	protected virtual void FStart() { }

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
	

	public void Face(bool right) {
		facing = right ? 1 : -1;
		transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
	}
}
