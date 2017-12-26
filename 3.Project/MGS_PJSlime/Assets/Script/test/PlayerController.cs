using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : EntityBase {	 
	public Animator anim;	
	public float moveForce;
	public float moveJumpForce; 
	public float jumpForce;
	public bool eatSkill = true;
	public bool jumping = false;
	public int PlayerIndex = 0;

	public SpriteRenderer sprite;
	public Dictionary<Collider2D, int> touching = new Dictionary<Collider2D, int>();

	protected override void FStart() {
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();

		if (Network.isServer) {
			GameEngine.direct.player = transform;
			rb.simulated = true;
			SetSize();
		}
	}
	
	void Update () {
		if (Network.isClient || Network.isServer) {
			float horizonDirection = 0;
			bool upCommand = false;
			bool downCommand = false;
			bool jumpCommand = false;
			bool eCommand = false;

			if (PlayerIndex == 0) {
				horizonDirection = (Input.GetAxis("LKeyboard") > 0 ? 1 : 0) + (Input.GetAxis("LKeyboard") < 0 ? -1 : 0);
				upCommand = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
				jumpCommand = Input.GetKeyDown(KeyCode.Space);
				eCommand = Input.GetKeyDown(KeyCode.E);

			} else if (PlayerIndex == 1) {
				horizonDirection = (Input.GetAxis("RKeyboard") > 0 ? 1 : 0) + (Input.GetAxis("RKeyboard") < 0 ? -1 : 0);
				upCommand = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow); 
				jumpCommand = Input.GetKeyDown(KeyCode.Space);
				eCommand = Input.GetKeyDown(KeyCode.E);

			} else if (PlayerIndex == 2) {
				horizonDirection = (Input.GetAxis("PS4LHorizontal") > 0 ? 1 : 0) + (Input.GetAxis("PS4LHorizontal") < 0 ? -1 : 0);
				upCommand = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
				jumpCommand = Input.GetAxis("LVPanel") < 0;
				eCommand = Input.GetAxis("LHPanel") > 0;

			} else if (PlayerIndex == 3) {
				horizonDirection = (Input.GetAxis("PS4RHorizontal") > 0 ? 1 : 0) + (Input.GetAxis("PS4RHorizontal") < 0 ? -1 : 0);
				upCommand = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);				
				jumpCommand = Input.GetKeyDown(KeyCode.Joystick1Button1);
				eCommand = Input.GetKeyDown(KeyCode.Joystick1Button2);
			}

			if (eatSkill && eCommand) {
				if (hp % 3 == 2) {
					CmdDigestive();

				} else {
					CmdEat(upCommand);
				}

			} else if (downCommand) {
				CmdCrouch();

			} else if (jumpCommand) {
				CmdJump(jumpCommand);

			} else if (horizonDirection != 0) {
				CmdMove(horizonDirection);

			}

			if (!eatSkill ) {
				Eat();
			}
		}

		if (Network.isServer) {
			if (touching.Count == 0) {
				RpcState("Jump");
			}

			if (isInvincible) {
				invincibleTimer += Time.deltaTime;
				
				if (invincibleTimer < 3f) {
					float remainder = invincibleTimer % 0.3f;
					sprite.color = remainder > 0.15f ? Color.white : Color.clear;

				} else {
					invincibleTimer = 0;
					sprite.color = Color.white;
					isInvincible = false;
				}
			}

			RpcApplyTransform(transform.position , transform.localScale);
		}		
	}

	[ClientRpc]
	public void RpcApplyTransform(Vector2 position, Vector2 localScale) {
		transform.position = position;
		//transform.localScale = localScale;
	}
		
	[ClientRpc]
	public void RpcState(string state) {
		anim.Play(state);
	}

	[Command]
	public void CmdRegist(int index) {
		PlayerIndex = index;
	}

	[Command]
	public void CmdCrouch() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Crouch") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ) {
			rb.velocity = Vector2.zero;
			RpcState("Crouch");
		}
	}

	[Command]
	public void CmdDigestive() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) {
			bool eatCheck = false;
			foreach (Transform unit in GameEngine.direct.units) {
				if (Vector2.Distance(transform.position, unit.position) <= size * 0.25f + 3) {
					eatCheck = true;
					break;
				}
			}

			if (eatCheck) {
				RpcState("Digestive");
			} else {
				RpcState("EatHorizon");
			}
			Eat();
		}
	}

	[Command]
	public void CmdEat(bool upCommand) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) {
			if (upCommand ) {
				RpcState("EatUp");
			} else {
				RpcState("EatHorizon");
			}
			Eat();
		} 
	}

	[Command]
	public void CmdJump(bool jumpCommand) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (jumpCommand && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) {
			rb.AddForce(Vector2.up * jumpForce * ((40 - size) * 0.025f), ForceMode2D.Impulse);
			jumping = true;
			RpcState("Jump");
			return;
		}
	}

	[Command]
	public void CmdMove(float moveDirection) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if ((moveDirection == 1 && touching.ContainsValue(2)) || (moveDirection == -1 && touching.ContainsValue(3))) {
			moveDirection = 0;
		}
				
		if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) {//地面發呆
			if (moveDirection == 0) {
				if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle") ) {
					RpcState("Idle");
				}
				rb.velocity = new Vector2(0, rb.velocity.y);
			} else {
				if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk") ) {
					RpcState("Walk");
				}

				transform.localScale = new Vector3(moveDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
				rb.velocity = new Vector2(moveDirection * moveForce, rb.velocity.y);
			}

		} else  {//空中移動
			if (moveDirection != 0) {
				transform.localScale = new Vector3(moveDirection * Mathf.Abs( transform.localScale.x), transform.localScale.y, 1);
				rb.velocity = new Vector2(moveDirection * moveJumpForce, rb.velocity.y);
			}				
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (Network.isServer && collision.transform.tag == "End") {
			Destroy(gameObject);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (Network.isServer) {
			touching.Remove(collision.collider);
		}
	}

	protected override void FOnCollisionStay2D(Collision2D collision) {
		Vector2 pointOfContact = collision.contacts[0].normal;

		//Left
		if (pointOfContact == new Vector2(-1, 0)) {
			TouchSide(collision, 2);

			//Right	
		} else if (pointOfContact == new Vector2(1, 0)) {
			TouchSide(collision, 3);

			//Bottom
		} else if (pointOfContact == new Vector2(0, -1)) {
			TouchSide(collision, 1);

			//Top
		} else if (pointOfContact == new Vector2(0, 1)) {
			TouchSide(collision, 0);
		}
	}
	
	private void TouchSide(Collision2D collision , int side) {
		if (side == 0 && !touching.ContainsValue(0)) {
			RpcState("Idle");
			rb.velocity = Vector2.zero;
		}

		if (!touching.ContainsKey(collision.collider)) {
			touching.Add(collision.collider, side);
		} else {
			touching[collision.collider] = side;
		}
	}

	private void Eat() {
		foreach (Transform unit in GameEngine.direct.units) {
			if (Vector2.Distance(transform.position, unit.position) <= size * 0.25f + 3) {
				Destroy(unit.gameObject);
				hp++;
				SetSize();
				return;
			}
		}
	}

	public override void Attack(int damage) {
		if (!isInvincible) {
			isInvincible = true;
			hp--;
			if (hp == 0) {
				Destroy(gameObject);
			}
			SetSize();
		}
	}

	float size = 0;

	protected void SetSize() {
		size = (int)(hp / 3);
		float tempsize = (0.5f + size * 0.25f) * (transform.localScale.x / Mathf.Abs(transform.localScale.x));
		transform.localScale = new Vector3(tempsize, Mathf.Abs(tempsize), 1);
	}
}