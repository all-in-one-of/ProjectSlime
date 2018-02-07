using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : EntityBase {	
	private static float BasicSize = 0.5f;

	public enum State {
		Normal,
		Jump,
	};


	public State state = State.Normal;
	public Animator anim;
	
	public int jumpGape;

	public bool isDead = false;
	public bool eatSkill = true;
	public int PlayerIndex = 0;
	public Vector2 velocitA;
	public Vector2 deVelocity;

	public SpriteRenderer sprite;
	public Dictionary<Collider2D, int> touching;
	
	protected float size = 0;
	

	protected override void FStart() {
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();

		if (Network.isServer) {
			rb.simulated = true;
			SetSize();
			touching = new Dictionary<Collider2D, int>();
		}
	}
	
	void Update () {
		if ((Network.isClient || Network.isServer)) {

			float horizonDirection = 0;
			bool downCommand = false;
			bool jumpCommand = false;
			bool eCommand = false;
			

			if (PlayerIndex == 0) {
				if (isDead) {
					if (Input.GetKeyDown(KeyCode.E)) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("LKeyboard") > 0 ? 1 : 0) + (Input.GetAxisRaw("LKeyboard") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
				jumpCommand = Input.GetKeyDown(KeyCode.Space);
				eCommand = Input.GetKeyDown(KeyCode.E);

			} else if (PlayerIndex == 1) {
				if (isDead) {
					if (Input.GetKeyDown(KeyCode.Period)) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("RKeyboard") > 0 ? 1 : 0) + (Input.GetAxisRaw("RKeyboard") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow); 
				jumpCommand = Input.GetKeyDown(KeyCode.Comma);
				eCommand = Input.GetKeyDown(KeyCode.Period);

			} else if (PlayerIndex == 2) {
				if (isDead) {
					if (Input.GetAxisRaw("LHPanel") > 0) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("PS4LHorizontal") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4LHorizontal") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
				jumpCommand = Input.GetAxisRaw("LVPanel") < 0;
				eCommand = Input.GetAxisRaw("LHPanel") > 0;

			} else if (PlayerIndex == 3) {
				if (isDead) {
					if (Input.GetKeyDown(KeyCode.Mouse1)) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("PS4RHorizontal") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4RHorizontal") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);				
				jumpCommand = Input.GetKeyDown(KeyCode.Mouse0);
				eCommand = Input.GetKeyDown(KeyCode.Mouse1);
			}

			if (eatSkill && eCommand) {
				CmdDigestive();

			} else if (downCommand) {
				CmdCrouch();

			} else if (jumpCommand) {
				CmdJump(jumpCommand);

			} else if (horizonDirection != 0) {
				CmdMove(horizonDirection);

			} else {
				CmdIdle();
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

	void FixedUpdate() {
		if (Network.isServer && !isDead) {
			if (touching.ContainsValue(2) && velocitA.x > 1) {
				velocitA.x = 0;

			} else if(touching.ContainsValue(3) && velocitA.x < 1) {
				velocitA.x = 0;
			}

			velocitA.y = rb.velocity.y - GameEngine.direct.jumpYDec * Time.deltaTime ;
			//velocitA.y = !touching.ContainsValue(2) ? rb.velocity.y - GameEngine.direct.jumpYDec * Time.deltaTime : 0;
			rb.velocity = velocitA;
		}
	}

	[ClientRpc]
	public void RpcApplyTransform(Vector2 position, Vector2 localScale) {
		transform.position = position;
	}
		
	[ClientRpc]
	public void RpcState(string state) {
		anim.Play(state);
	}

	[Command]
	public void CmdRegist(int PlayerIndex, int hp, int jumpGape) {
		this.PlayerIndex = PlayerIndex;
		this.hp = hp;
		this.jumpGape = jumpGape;
		GameEngine.direct.OnRegist(this);
		SetSize();
	}

	[Command]
	public void CmdCrouch() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Crouch") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") ) {
			//rb.velocity = Vector2.zero;
			RpcState("Crouch");
		}
	}

	[Command]
	public void CmdDigestive() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (state != State.Jump) {
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
	public void CmdEat() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (state != State.Jump) {
			RpcState("EatHorizon");
			Eat();
		} 
	}

	[Command]
	public void CmdJump(bool jumpCommand) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (jumpCommand && state != State.Jump) {
			rb.AddForce(Vector2.up * GameEngine.direct.jumpYForce * ((jumpGape - size) / jumpGape), ForceMode2D.Impulse);
			state = State.Jump;
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

		if (state != State.Jump) {//地面發呆
			if (moveDirection != 0) {				
				if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk")) {
					RpcState("Walk");
				}

				transform.localScale = new Vector3(moveDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);

				if (!IsSlideing()) {
					velocitA.x = Accelerator(velocitA.x, moveDirection * GameEngine.direct.walkXAcc, moveDirection * GameEngine.direct.walkXSpeed);
				} else {
					velocitA.x = Accelerator(velocitA.x, moveDirection * GameEngine.direct.iceXAcc, moveDirection * GameEngine.direct.walkXSpeed);
				}				
				return;
			}
			CmdIdle();
		} else  {//空中移動
			if (moveDirection != 0) {
				transform.localScale = new Vector3(moveDirection * Mathf.Abs( transform.localScale.x), transform.localScale.y, 1);
				velocitA.x = Accelerator(velocitA.x, moveDirection * GameEngine.direct.jumpXAcc, moveDirection * GameEngine.direct.jumpXSpeed);
			}				
		}
	}

	[Command]
	public void CmdIdle() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (state != State.Jump) {//地面發呆
			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
				RpcState("Idle");
			}

			if (rb.velocity.x != 0) {
				if (!IsSlideing()) {
					velocitA.x = Decelerator(velocitA.x, GameEngine.direct.walkXDec, 0);
				} else {
					velocitA.x = Decelerator(velocitA.x, GameEngine.direct.iceXDec, 0);
				}
			}
		} else {//空中移動
			velocitA.x = Decelerator(velocitA.x, GameEngine.direct.iceXDec, 0);
		}
	}

	public override void Attack(int damage, bool firstOrder = false) {
		if (!isInvincible || firstOrder) {
			isInvincible = true;
			hp = hp - damage;
			if (hp == 0) {
				GameEngine.direct.OnDead(this);
			} else {
				SetSize();
			}
		}
	}

	public override void OnDead() {
		rb.simulated = false;
		transform.localScale = Vector3.zero;
		isDead = true;
		velocitA = Vector2.zero;
		base.OnDead();
	}

	public void Reborn() {
		rb.simulated = true;
		SetSize();
		isDead = false;
	}
	
	protected void OnCollisionExit2D(Collision2D collision) {
		if (Network.isServer) {
			if (isDead) {
				return;
			}

			touching.Remove(collision.collider);
		}
	}

	protected override void FOnCollisionEnter2D(Collision2D collision) {
		if (isDead) {
			return;
		}

		if (collision.transform.tag == "End") {
			GameEngine.direct.OnVictory();

		} else if (collision.transform.tag == "Dead" || collision.transform.tag == "Scene") {
			GameEngine.direct.OnDead(this);
		}
	}

	protected override void FOnCollisionStay2D(Collision2D collision) {
		if (isDead) {
			return;
		}

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

	private void TouchSide(Collision2D collision, int side) {
		if (side == 0 && !touching.ContainsValue(0)) {
			RpcState("Idle");
			//rb.velocity = Vector2.zero;
			state = State.Normal;
		}

		if (!touching.ContainsKey(collision.collider)) {
			touching.Add(collision.collider, side);
		} else {
			touching[collision.collider] = side;
		}
	}

	protected void Eat() {
		foreach (Transform unit in GameEngine.direct.units) {
			if (Vector2.Distance(transform.position, unit.position) <= (BasicSize + size * 0.125f) + 2) {
				unit.GetComponent<EntityBase>().OnDead();
				hp++;
				SetSize();
				return;
			}
		}
	}
	
	protected void SetSize() {
		size = hp;
		float tempsize = (BasicSize + size * 0.125f) * (transform.localScale.x != 0 ?(transform.localScale.x / Mathf.Abs(transform.localScale.x)) : 1);
		transform.localScale = new Vector3(tempsize, Mathf.Abs(tempsize), 1);
		GameEngine.direct.ResetCamera();		
	}

	public bool IsSlideing() {
		foreach (Collider2D collider in touching.Keys) {
			if (collider.name == "Ice") {
				return true;
			}
		}
		return false;
	}

	public float Accelerator(float value , float acc , float maxValue) {
		acc = acc * Time.deltaTime;

		if (maxValue > 0) {
			return value + acc >= maxValue ? maxValue : value + acc;

		} else if (maxValue < 0) {
			return value + acc <= maxValue ? maxValue : value + acc;
		}
		return 0;
	}

	public float Decelerator(float value, float dec, float finalValue) {
		dec = dec * Time.deltaTime;

		if (value > finalValue) {
			return value - dec <= finalValue ? finalValue : value - dec;

		} else if (value < finalValue) {
			return value + dec >= finalValue ? finalValue : value + dec;
		}
		return finalValue;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (Network.isServer) {
			GameEngine.RegistCheckPoint(collider.gameObject.name);
		}
	}
}