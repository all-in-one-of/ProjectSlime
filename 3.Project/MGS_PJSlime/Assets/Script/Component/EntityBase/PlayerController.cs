using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : EntityBase {	
	private static float BasicSize = 0.3f;

	public enum State {
		None,
		Jump,
		Fall,
	};

	//public float[] damageTable = { 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2 };
	public int[] damageTable = { 0, 0, 0, 0, 1, 1, 1, 4, 4, 4 , 4};

	public State state = State.None;
	public Animator anim;

	public bool newDamage = false;
	public int PlayerIndex = 0;
	public Vector2 velocityOut;
	public Vector2 velocitSim;
	public Vector2 deVelocity;

	public Transform isInWater;
	public SpriteRenderer sprite;
	public Dictionary<Collider2D, int> touching = new Dictionary<Collider2D, int>();
	public Transform eating = null;

	public AudioSource bornAudio;
	public AudioSource jumpAudio;
	public AudioSource eatAudio;

	public float size = 0;

	private float jumpTimer;
	private float swimTimer;

	private bool jumpPreCommand = false;

	protected override void FStart() {
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();

		if (Network.isServer) {
			rb.simulated = true;
			SetSize();
		}
	}

	/*
	 if (Input.GetAxisRaw("Fire1") != 0) {
					if (jumpPreCommand == false) {
						// Call your event function here.
						jumpPreCommand = true;
					}
				}
				if (Input.GetAxisRaw("Fire1") == 0) {
					jumpPreCommand = false;
				}
	*/

	void Update () {
		if ((Network.isClient || Network.isServer)) {
			
			float horizonDirection = 0;
			bool downCommand = false;
			bool jumpCommand = false;
			bool jumpNowCommand = false;
			bool eatCommand = false;
			bool skipper = false;

			if (PlayerIndex == 0) {
				if (isDead) {
					if (Input.GetKeyDown(KeyCode.E)) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("LeftHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("LeftHorizon") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.Q);
				jumpCommand = Input.GetKey(KeyCode.Space);				
				eatCommand = Input.GetKeyDown(KeyCode.E);
				if (Input.GetKey(KeyCode.Space)) {
					if (jumpPreCommand == false) {
						jumpNowCommand = true;
						jumpPreCommand = true;
					}
				}
				if (!Input.GetKey(KeyCode.Space)) {
					jumpPreCommand = false;
				}

			} else if (PlayerIndex == 1) {
				if (isDead) {
					if (Input.GetKeyDown(KeyCode.Period)) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("RightHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("RightHorizon") < 0 ? -1 : 0);
				downCommand = Input.GetKey(KeyCode.Slash);
				jumpCommand = Input.GetKey(KeyCode.Period);
				eatCommand = Input.GetKeyDown(KeyCode.Comma);
				if (Input.GetKey(KeyCode.Period)) {
					if (jumpPreCommand == false) {
						jumpNowCommand = true;
						jumpPreCommand = true;
					}
				}
				if (!Input.GetKey(KeyCode.Period)) {
					jumpPreCommand = false;
				}

			} else if (PlayerIndex == 2) {
				if (isDead) {
					if (Input.GetAxisRaw("LHPanel") > 0) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("PS4LeftHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4LeftHorizon") < 0 ? -1 : 0);
				downCommand = Input.GetAxisRaw("LVPanel") > 0;
				jumpCommand = Input.GetAxisRaw("LVPanel") < 0;
				eatCommand = Input.GetAxisRaw("LHPanel") > 0;
				if (Input.GetAxisRaw("LVPanel") < 0) {
					if (jumpPreCommand == false) {
						jumpNowCommand = true;
						jumpPreCommand = true;
					}
				}
				if (Input.GetAxisRaw("LVPanel") == 0) {
					jumpPreCommand = false;
				}

			} else if (PlayerIndex == 3) {
				if (isDead) {
					if (Input.GetAxisRaw("PS4RightHorizonPanel") > 0) {
						GameEngine.direct.OnReborn(this);
					}
					return;
				}
				horizonDirection = (Input.GetAxisRaw("PS4RightHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4RightHorizon") < 0 ? -1 : 0);
				downCommand = Input.GetAxisRaw("PS4RightVerticalPanel") < 0;
				jumpCommand = Input.GetAxisRaw("PS4RightVerticalPanel") > 0;
				eatCommand = Input.GetAxisRaw("PS4RightHorizonPanel") > 0;
				if (Input.GetAxisRaw("PS4RightVerticalPanel") > 0) {
					if (jumpPreCommand == false) {
						jumpNowCommand = true;
						jumpPreCommand = true;
					}
				}
				if (Input.GetAxisRaw("PS4RightVerticalPanel") == 0) {
					jumpPreCommand = false;
				}
			}

			if (eatCommand && !skipper) {
				CmdDigestive();
				skipper = true;
			}

			if (downCommand && !skipper) {
				CmdCrouch(horizonDirection);
				skipper = true;
			}

			if (jumpNowCommand && !skipper) {
				CmdJump();

			} else if (jumpCommand && !skipper) {
				CmdJumpForce();
			}

			if (horizonDirection != 0 && !skipper) {
				CmdMove(horizonDirection);
				skipper = true;
			}

			if (!skipper) {
				CmdIdle();
				skipper = true;
			}

			if (eating) {
				eating.transform.position = Vector2.Lerp(eating.transform.position, transform.position, 0.1f);
			}
		}

		if (Network.isServer) {
			if (isInvincible) {
				invincibleTimer += Time.deltaTime;
				
				if (invincibleTimer < 2f) {
					float remainder = invincibleTimer % 0.2f;
					sprite.color = remainder > 0.1f ? Color.white : new Color(1 , 1 , 1 , 0.4f);

				} else {
					invincibleTimer = 0;
					sprite.color = Color.white;
					isInvincible = false;
				}
			}

			RpcApplyTransform(transform.position , transform.localScale);
		}		
	}

	protected override void FFixedUpdate() {
		if (!isDead) {
			if (velocityOut.x != 0) {
				velocitSim.x = velocitSim.x + velocityOut.x;
				velocityOut = Vector2.zero;
			}

			if (touching.ContainsValue(2) && velocitSim.x > 1) {
				velocitSim.x = 0;

			} else if(touching.ContainsValue(3) && velocitSim.x < 1) {
				velocitSim.x = 0;
			}

			if (isInWater) {
				velocitSim.y = rb.velocity.y - GameEngine.direct.waterYDec * Time.deltaTime;
				if (velocitSim.y <= -GameEngine.direct.waterYSpeed) {
					velocitSim.y = -GameEngine.direct.waterYSpeed;
				}
			} else {
				velocitSim.y = rb.velocity.y - GameEngine.direct.jumpYDec * Time.deltaTime;
			}
			
			rb.velocity = velocitSim;
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
	public void CmdRegist(int PlayerIndex, int hp) {
		this.PlayerIndex = PlayerIndex;
		this.hp = hp;
		GameEngine.direct.OnRegist(this);
		SetSize();
	}

	[Command]
	public void CmdCrouch(float direction) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		if (state != State.Jump) {//地面發呆
			Facing(direction);

			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Crouch") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) {
				RpcState("Crouch");
			}

			if (rb.velocity.x != 0) {
				if (!IsSlideing()) {
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.walkXDec, 0);
				} else {
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
				}
			}
		} else {//空中移動
			velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
		}
	}

	[Command]
	public void CmdDigestive() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			return;
		}

		//if (state != State.Jump) {
			bool eatCheck = false;
			foreach (Transform unit in GameEngine.direct.units) {
				if (Vector2.Distance(transform.position, unit.position) <= SizeFormula(size) + 3) {
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
		//}
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
	public void CmdJump() {	
		if (isInWater) {
			if (Time.timeSinceLevelLoad - swimTimer >= GameEngine.direct.waterColdDown) {
				swimTimer = Time.timeSinceLevelLoad;
				jumpAudio.Play();
				state = State.Jump;
				RpcState("Jump");
				rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.waterYForce );				
			}
			
		} else {
			if (state == State.None) {
				jumpTimer = Time.timeSinceLevelLoad;
				jumpAudio.Play();
				state = State.Jump;
				RpcState("Jump");
				rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.jumpYForce * (( GameEngine.direct.jumpGape - size) /  GameEngine.direct.jumpGape));
			}
		}
	}

	[Command]
	public void CmdJumpForce() {
		if (state == State.Jump && Time.timeSinceLevelLoad - jumpTimer < GameEngine.direct.jumpDuraion) {
			rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.jumpYForce * (( GameEngine.direct.jumpGape - size) /  GameEngine.direct.jumpGape));
		}
	}

	protected void Facing(float direction) {
		if (direction != 0) {
			transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
		}
	}

	[Command]
	public void CmdMove(float direction) {		
		if ((direction == 1 && touching.ContainsValue(2)) || (direction == -1 && touching.ContainsValue(3))) {
			direction = 0;
		}

		if (state != State.Jump) {//地面發呆
			if (direction != 0) {				
				if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk")) {
					RpcState("Walk");
				}

				Facing(direction);

				if (!IsSlideing()) {
					velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.walkXAcc, direction * GameEngine.direct.walkXSpeed);
				} else {
					velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.iceXAcc, direction * GameEngine.direct.walkXSpeed);
				}				
				return;
			}
			CmdIdle();
		} else  {//空中移動
			if (direction != 0) {
				Facing(direction);
				if (isInWater) {
					velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.waterXAcc, direction * GameEngine.direct.waterXSpeed);
				} else {
					velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.jumpXAcc, direction * GameEngine.direct.jumpXSpeed);
				}
			}				
		}
	}

	[Command]
	public void CmdLand() {
		RpcState("Idle");
		state = State.None;
	}

	[Command]
	public void CmdIdle() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Eat")) {
			if (state != State.Jump) {
				if (rb.velocity.x != 0) {
					if (!IsSlideing()) {
						velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.walkXDec, 0);
					} else {
						velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
					}
				}
			} else {//空中移動
				velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
			}
			return;
		}

		if (state != State.Jump) {//地面發呆
			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
				RpcState("Idle");
			}

			if (rb.velocity.x != 0) {
				if (!IsSlideing()) {
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.walkXDec, 0);
				} else {
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
				}
			}
		} else {//空中移動
			velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.iceXDec, 0);
		}
	}

	public override void Attack(int damage, bool firstOrder = false) {
		if (!isInvincible || firstOrder) {
			isInvincible = true;
			hp = hp - damage;
			if (hp == 0) {
				OnDead();
			} else {
				SetSize();
			}
		}
	}

	public override void OnDead() {
		isDead = true;
		GameEngine.direct.OnDead(this);
		rb.simulated = false;
		transform.localScale = Vector3.zero;
		velocitSim = Vector2.zero;
	}

	public void Reborn(int size = 2) {
		bornAudio.Play();
		rb.simulated = true;
		hp = 2;
		SetSize();
		isDead = false;
		state = State.Fall;
		touching = new Dictionary<Collider2D, int>();
	}
	
	protected void OnCollisionExit2D(Collision2D collision) {
		if (Network.isServer) {
			touching.Remove(collision.collider);

			if (isDead) {
				return;
			}

			if (touching.Count == 0 && state == State.None) {
				state = State.Fall;
				RpcState("Jump");
			}
		}
	}

	protected override void FOnCollisionEnter2D(Collision2D collision) {
		if (isDead) {
			return;
		}

		if (collision.contacts.Length == 0) {
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

		if (collision.transform.tag == "End") {
			GameEngine.direct.OnVictory();

		} else if (collision.transform.tag == "Dead" || collision.transform.tag == "Scene") {
			OnDead();

		} else if (collision.transform.tag == "Slime") {
			PlayerController ipc = collision.gameObject.GetComponent<PlayerController>();

			float xv = velocitSim.x >= 0 ? 1 : -1;
			float ixv = ipc.velocitSim.x >= 0 ? 1 : -1;

			float xe = velocitSim.x * 0.75f + xv * size;
			float ixe = ipc.velocitSim.x * 0.75f + ixv * ipc.size;

			if (Mathf.Abs(xe) > Mathf.Abs(ixe) && velocitSim.x != 0) {
				Debug.Log(name + "[1]:" + name + "/" + xe + "撞" + collision.gameObject.name + "/" + ixe);

				if (Mathf.Abs(xe + ixe) >= 6) {
					collision.gameObject.GetComponent<PlayerController>().velocityOut.x = xv * Mathf.Abs(xe + ixe) * 0.5f;
				}
			}

			if (pointOfContact == new Vector2(0, 1)) {
				CmdLand();
			}

		} else if (state != State.None && collision.transform.tag == "Water") {
			velocitSim.y *= 0.2f;

		} else if (state != State.None && collision.transform.tag == "Ground") {
			CmdLand();

		} else if (collision.transform.tag == "Ceil") {

		}
	}

	/*
	protected override void FOnCollisionStay2D(Collision2D collision) {
		if (isDead ) {
			return;
		}

		if (collision.contacts.Length == 0) {
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
	}*/
	
	private void TouchSide(Collision2D collision, int side) {
		if (!touching.ContainsKey(collision.collider)) {
			touching.Add(collision.collider, side);
		} else {
			touching[collision.collider] = side;
		}
	}

	protected void Eat() {
		eatAudio.Play();
		foreach (Transform unit in GameEngine.direct.units) {
			if (Vector2.Distance(transform.position, unit.position) <= SizeFormula(size) + 3) {
				eating = unit;
				unit.GetComponent<EntityBase>().OnDead();
				hp++;
				SetSize();
				return;
			}
		}
	}
	
	protected void SetSize() {
		size = hp;
		float tempsize = SizeFormula(size) * (transform.localScale.x != 0 ?(transform.localScale.x / Mathf.Abs(transform.localScale.x)) : 1);
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
		if (Network.isServer && collider.tag == "Water") {
			isInWater = collider.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (Network.isServer && collider.tag == "Water") {
			isInWater = null;
			rb.velocity = new Vector2(rb.velocity.x, 3 * GameEngine.direct.waterYForce );
		}
	}

	private float SizeFormula(float value) {
		return (BasicSize + value * 0.15f);
	}
}