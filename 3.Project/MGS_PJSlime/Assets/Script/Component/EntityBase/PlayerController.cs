using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : EntityBase {	
	private static float BasicSize = 0.15f;

	private const int LANDLAYER = (1 << 8) | (1 << 9) | (1 << 11);
	private const float DETECTOFFSET = 0.05f;

	public int[] damageTable = { 0, 0, 0, 0, 1, 1, 1, 4, 4, 4 , 4};
	
	public bool newDamage = false;
	public int playerID = 0;
	public Vector2 velocityOut;
	public Vector2 velocitSim;
	public Vector2 deVelocity;

	public List<Transform> nowTrigger;
	public SpriteRenderer sprite;
	public Transform eating = null;

	public AudioSource bornAudio;
	public AudioSource jumpAudio;
	public AudioSource eatAudio;

	public bool eatSkill = true;
	public float size = 0;

	public float animSpeed = 1.6f;

	private float jumpTimer;
	private float swimTimer;

	private bool jumpPreCommand = false;
	private int jumpCounter = 0;
	private string stateOver = "";
	
	//判斷撞到甚麼
	private Vector2 bcWidth;
	private Vector2 bcHeight;
	private Vector2 groundOffset;
	private Vector2 faceOffset;
	private Collider2D onGround;
	private Collider2D onFace;

	public bool Secret = false;

	protected override void FStart() {
		SetSize();
		
		faceOffset = new Vector2(bc.offset.x + bc.size.x * 0.5f, bc.offset.y);
		groundOffset = new Vector2(bc.offset.x, bc.offset.y - bc.size.y * 0.5f);

		bcWidth = new Vector2(bc.size.x * 0.8f, 0);
		bcHeight = new Vector2(0, bc.size.y * 0.8f);

		//GetComponent<MeshRenderer>().materials[0] = GameEngine.direct.playerMaterial[playerID];
		GameEngine.direct.playerMaterial[playerID].enableInstancing = true;
	}

	public bool fuck;

	void Update () {
		if (!fuck) {
			GetComponent<MeshRenderer>().material = GameEngine.direct.playerMaterial[playerID];
			//fuck = true;
		}

		float horizonDirection = 0;
		//bool downCommand = false;
		bool jumpCommand = false;
		bool jumpNowCommand = false;
		bool eatCommand = false;
		bool skipper = false;

		if (playerID == 0) {
			if (isDead) {
				if (Input.GetKeyDown(KeyCode.E)) {
					GameEngine.direct.OnReborn(this);
				}
				return;
			}
			horizonDirection = (Input.GetAxisRaw("LeftHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("LeftHorizon") < 0 ? -1 : 0);
			//downCommand = Input.GetKey(KeyCode.Q);
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

		} else if (playerID == 1) {
			if (isDead) {
				if (Input.GetKeyDown(KeyCode.Comma)) {
					GameEngine.direct.OnReborn(this);
				}
				return;
			}
			horizonDirection = (Input.GetAxisRaw("RightHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("RightHorizon") < 0 ? -1 : 0);
			//downCommand = Input.GetKey(KeyCode.Slash);
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

		} else if (playerID == 2) {
			if (isDead) {
				if (Input.GetAxisRaw("LHPanel") > 0) {
					GameEngine.direct.OnReborn(this);
				}
				return;
			}
			horizonDirection = (Input.GetAxisRaw("PS4LeftHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4LeftHorizon") < 0 ? -1 : 0);
			//downCommand = Input.GetAxisRaw("LVPanel") > 0;
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

		} else if (playerID == 3) {
			if (isDead) {
				if (Input.GetAxisRaw("PS4RightHorizonPanel") > 0) {
					GameEngine.direct.OnReborn(this);
				}
				return;
			}
			horizonDirection = (Input.GetAxisRaw("PS4RightHorizon") > 0 ? 1 : 0) + (Input.GetAxisRaw("PS4RightHorizon") < 0 ? -1 : 0);
			//downCommand = Input.GetAxisRaw("PS4RightVerticalPanel") < 0;
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

		if (eatSkill && eatCommand && !skipper) {
			CmdEat();
			skipper = true;
		}
		/*
		if (downCommand && !skipper) {
			CmdCrouch(horizonDirection);
			skipper = true;
		}*/

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
			CmdUnmove();
			skipper = true;
		}

		if (eating) {
			eating.transform.position = Vector2.Lerp(eating.transform.position, transform.position, 0.1f);
		}

		if (isInvincible) {
			invincibleTimer += Time.deltaTime;

			if (invincibleTimer < 2f) {
				float remainder = invincibleTimer % 0.2f;
				skam.skeleton.a = remainder > 0.1f ? 1 : 0.4f;

			} else {
				invincibleTimer = 0;
				skam.skeleton.a = 1;
				isInvincible = false;
			}
		}

		RpcTransform(transform.position, transform.localScale);
	}

	protected override void FFixedUpdate() {
		if (!isDead) {
			//更新碰撞狀態
			bool preGround = onGround;
			
			onGround = Physics2D.OverlapBox((Vector2)transform.position + groundOffset * transform.localScale.x - new Vector2(0, DETECTOFFSET), bcWidth * transform.localScale.x , 0, LANDLAYER);

			if (facing == 1) {
				onFace = Physics2D.OverlapBox((Vector2)transform.position + faceOffset * transform.localScale.x + new Vector2(DETECTOFFSET, 0), bcHeight * transform.localScale.x, 0, LANDLAYER);
			} else {
				onFace = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(-faceOffset.x , faceOffset.y) * transform.localScale.x - new Vector2(DETECTOFFSET, 0), bcHeight * transform.localScale.x, 0, LANDLAYER);
			}

			if (!preGround && onGround) {
				CmdEnGround();
			}


			if (velocityOut.x != 0) {
				velocitSim.x = velocitSim.x + velocityOut.x;
				velocityOut = Vector2.zero;
			}

			/*
			//撞牆停止，實作有誤
			if (onFace) {
				velocitSim.x = 0;
			}*/

			if (OnTrigger("Water")) {
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

	public void SetState(string animValue, bool loopValue = false, bool returnValue = false, bool baseValue = false) {
		state = baseValue ? "" : animValue;
		RpcState(animValue, loopValue, returnValue, 0);
	}

	public void SetOverState(string animValue, bool loopValue = false, bool returnValue = false, bool baseValue = false) {
		stateOver = baseValue ? "" : animValue;
		RpcState(animValue, loopValue, returnValue, 1);
	}

	//[ClientRpc]
	public void RpcTransform(Vector2 position, Vector2 localScale) {
		transform.position = position;
	}
		
	//[ClientRpc]
	public void RpcState(string animValue, bool loopValue , bool returnValue , int trackValue) { 
		Spine.TrackEntry entry = skam.state.SetAnimation(trackValue, animValue, loopValue);

		if (returnValue) {
			if (trackValue == 1) {
				skam.state.AddEmptyAnimation(trackValue, 0, 0f);
				entry.End += delegate {
					stateOver = "";
				};
			} else {
				skam.state.AddAnimation(trackValue, "Idle", true, 0f);
				entry.End += delegate {
					state = "";
				};
			}
		}
	} 	

	//[Command]
	public void CmdRegist(int playerID, int hpValue) {
		this.playerID = playerID;

		GameEngine.direct.OnRegist(this);
		SetHP(hpValue);
	}

	//[Command]
	public void CmdCrouch(float direction) {
		if (state == "Eat") {
			return;
		}

		if (onGround) {//地面發呆
			Face(direction == 1);

			if (state != "Crouch" && state != "Jump") {
				ScoreSystem.AddRecord(playerID, 3, 1);
				SetState("Crouch");
			}

			if (rb.velocity.x != 0) {
				if (!IsSlideing()) {
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.walkXDec, 0);
				} else {
					velocitSim.x = Decelerator(velocitSim.x, (GameEngine.direct.iceXDec + GameEngine.GetBuffer(playerID).iceXDec), 0);
				}
			}
		} else {//空中移動
			velocitSim.x = Decelerator(velocitSim.x, (GameEngine.direct.iceXDec + GameEngine.GetBuffer(playerID).iceXDec), 0);
		}
	}

	//[Command]
	public void CmdEat() {
		if (stateOver == "Eat") {
			return;
		}

		SetOverState(state == "Jump" ? "Eat2" : "Eat" , false , true);
		Eat();
	}
	
	//[Command]
	public void CmdJump() {
		if (OnTrigger("Water")) { //WATER
			if (Time.timeSinceLevelLoad - swimTimer >= GameEngine.direct.waterColdDown) {
				ScoreSystem.AddRecord(playerID, 4, 1);
				swimTimer = Time.timeSinceLevelLoad;
				jumpAudio.Play();
				SetState("Jump");
				rb.velocity = new Vector2(rb.velocity.x, (GameEngine.direct.waterYForce + GameEngine.GetBuffer(playerID).waterYForce));
			}

		} else if (onGround) { //LAND
			ScoreSystem.AddRecord(playerID, 2, 1);
			jumpTimer = Time.timeSinceLevelLoad;
			jumpAudio.Play();
			SetState("Jump");
			rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.jumpYForce * (1 + GameEngine.GetBuffer(playerID).jumpYForce));
			jumpCounter++;
			if (this == GameEngine.mainPlayer) {
				CameraManager.direct.Bump();
			}
		} else if (jumpCounter < GameEngine.direct.jumpMaxCount/**/) { //AIR
			jumpTimer = Time.timeSinceLevelLoad;
			jumpAudio.Play();
			SetState(Secret ? "Roll" : "Jump");
			rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.jumpYForce * (1 + GameEngine.GetBuffer(playerID).jumpYForce) * GameEngine.direct.jumpReduce);
			if (jumpCounter == 0) {
				jumpCounter++;
			}
			jumpCounter++;
			if (this == GameEngine.mainPlayer) {
				CameraManager.direct.Bump();
			}
		}
	}

	//[Command]
	public void CmdJumpForce() {
		if (!onGround && Time.timeSinceLevelLoad - jumpTimer < GameEngine.direct.jumpDuraion && jumpCounter < 2) {
			rb.velocity = new Vector2(rb.velocity.x, GameEngine.direct.jumpYForce * (1 + GameEngine.GetBuffer(playerID).jumpYForce));
		}
	}
	
	//[Command]
	public void CmdMove(float direction) {		
		if (onGround) {//地面發呆
			if (direction != 0) {				
				if (state != "Walk" && state != "Jump") {
					SetState("Walk" , true );
				}

				Face(direction == 1);
				if (!IsSlideing()) {//GROUND
					velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.walkXAcc, direction * (GameEngine.direct.walkXSpeed + GameEngine.GetBuffer(playerID).walkXSpeed));
				} else {//ICE
					velocitSim.x = Accelerator(velocitSim.x, direction * (GameEngine.direct.iceXAcc + GameEngine.GetBuffer(playerID).iceXAcc), direction * (GameEngine.direct.walkXSpeed + GameEngine.GetBuffer(playerID).walkXSpeed));
				}				
				return;
			}
			CmdUnmove();
		} else if (direction != 0) {
			Face(direction == 1);
			if (OnTrigger("Water")) {//WATER
				velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.waterXAcc, direction * (GameEngine.direct.waterXSpeed + GameEngine.GetBuffer(playerID).waterXSpeed));
			} else {//AIR
				velocitSim.x = Accelerator(velocitSim.x, direction * GameEngine.direct.jumpXAcc, direction * GameEngine.direct.jumpXSpeed);
			}
		}
	}

	//[Command]
	public void CmdEnGround() {
		SetState("Idle" , true , false , true);
		jumpCounter = 0;
	}

	//[Command]
	public void CmdEnWater() {
		jumpCounter = 0;
		rb.velocity *= 0.05f;
	}

	//[Command]
	public void CmdDeWater() {
		if (rb.velocity.y > 0) {
			float tempMaxForce = GameEngine.direct.jumpYForce * (1 + GameEngine.GetBuffer(playerID).jumpYForce);
			float tempForce = rb.velocity.y * tempMaxForce * 0.75f;

			rb.velocity = new Vector2(rb.velocity.x, tempForce > tempMaxForce ? tempMaxForce : tempForce);
		}
	}

	//[Command]
	public void CmdUnmove() {
		if (onGround) { 
			if (rb.velocity.x != 0) {
				if (!IsSlideing()) { //GROUND
					velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.walkXDec, 0);
				} else { //ICE
					velocitSim.x = Decelerator(velocitSim.x, (GameEngine.direct.iceXDec + GameEngine.GetBuffer(playerID).iceXDec), 0);
				}
			} else if (state == "Walk") { //發呆
				if (state != "") {
					SetState("Idle", true, false, true);
				}
			} 
		} else {
			if (OnTrigger("Water")) {//WATER
				velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.waterXDec, 0);
			} else {//AIR
				velocitSim.x = Decelerator(velocitSim.x, GameEngine.direct.jumpXDec, 0);
			}
		}
		
	}

	public override void Attack(int damage, bool firstOrder = false) {
		if (!isInvincible || firstOrder) {
			isInvincible = true;
			SetHP(hp - damage);
		}
	}

	public override void OnDead() {
		isDead = true;
		GameEngine.direct.OnDead(this);
		rb.simulated = false;
		transform.localScale = Vector3.zero;
		velocitSim = Vector2.zero;
	}

	public void Reborn() {
		ScoreSystem.AddRecord(playerID, 7, 1);
		nowTrigger = new List<Transform>();
		bornAudio.Play();
		rb.simulated = true;
		SetHP(GameEngine.direct.bornSize);
		isDead = false;
	}
	
	protected void OnCollisionExit2D(Collision2D collision) {
		if (isDead) {
			return;
		}

		/*
		if (onGround) {
			SetState("Jump");
		}*/
	}

	protected override void FOnCollisionEnter2D(Collision2D collision) {
		if (isDead) {
			return;
		}

		if (collision.contacts.Length == 0) {
			return;
		}
				
		if (collision.transform.tag == "End") {
			GameEngine.direct.OnVictory();

		} else if (collision.transform.tag == "Dead") {
			ScoreSystem.AddRecord(playerID, 1, 1);
			OnDead();

		} else if (collision.transform.tag == "Slime") {
			PlayerController ipc = collision.gameObject.GetComponent<PlayerController>();

			float xv = velocitSim.x >= 0 ? 1 : -1;
			float ixv = ipc.velocitSim.x >= 0 ? 1 : -1;

			float xe = velocitSim.x * 0.75f + xv * size;
			float ixe = ipc.velocitSim.x * 0.75f + ixv * ipc.size;

			if (Mathf.Abs(xe) > Mathf.Abs(ixe) && velocitSim.x != 0) {
				if (Mathf.Abs(xe + ixe) >= 6) {
					collision.gameObject.GetComponent<PlayerController>().velocityOut.x = xv * Mathf.Abs(xe + ixe) * 0.5f;
				}
			}
		}
	}	

	protected void Eat() {
		eatAudio.Play();
		EntityBase target = GameEngine.direct.GetUnitInRange(GetSizeFormula(size) + 1, transform.position);
		if (target && target.eatAble) {
			if (target.eatBuffer && target.buffer != null) {
				GameEngine.AddBufferEffect( playerID, target.buffer);
			}

			if (target.GetComponent<ProjectileBase>()) {
				ScoreSystem.AddRecord(playerID, 5, 1);

			} else if (target.GetComponent<EnemyBase>()) {
				ScoreSystem.AddRecord(playerID, 6, 1);
			}

			SetHP(hp+1);
			ScoreSystem.ModifyScore(target.bonus);

			eating = target.transform;
			target.OnDead();
			return;

		} else {
			ScoreSystem.AddRecord(playerID , 0 , 1);
		}
	}

	protected void SetHP(int value) {
		hp = value;
		if (hp <= 0) {
			OnDead();
		} else {
			SetSize();
		}
	}

	protected void SetSize() {
		size = hp;
		float tempsize = GetSizeFormula(size);
		transform.localScale = new Vector3(tempsize, tempsize, 1);
		GameEngine.direct.ResetCamera();		
	}
	
	public bool IsSlideing() {
		return onGround && onGround.tag == "Ice";
	}

	public void EndCollision(Collision2D collision) {
		//touching.Remove(collision.collider);
	}

	public void EndCollider(Collider2D collider) {
		//touching.Remove(collider);
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

	private void OnTriggerStay2D(Collider2D collider) {
		if (nowTrigger.Contains(collider.transform)) {

		} else {
			if (collider.tag == "Water") {//入水
				nowTrigger.Add(collider.transform);
				CmdEnWater();

				if (rb.velocity.y > -0.3f) {
					EffectManager.direct.Play("e_waterflows", transform.position);
				} else if (rb.velocity.y > -0.6f) {
					EffectManager.direct.Play("e_waterflowm", transform.position);
				} else {
					EffectManager.direct.Play("e_waterflowl", transform.position);
				}

			} else if (collider.tag == "CheckPoint") {//踩點
				GameEngine.RegistCheckPoint(collider.transform);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		nowTrigger.Remove(collider.transform);

		if (collider.tag == "Water" && !OnTrigger("Water")) {//出水
			CmdDeWater();
			EffectManager.direct.Play("e_waterflowm", transform.position);
		}
	}

	private bool OnTrigger(Transform objTag) {
		foreach (Transform obj in nowTrigger) {
			if (obj == objTag) {
				return true;
			}
		}
		return false;
	}

	private bool OnTrigger(string objTag) {
		foreach (Transform obj in nowTrigger) {
			if (obj.tag == objTag) {
				return true;
			}
		}
		return false;
	}

	private float GetSizeFormula(float sizeValue) {
		return (BasicSize + sizeValue * 0.04f);
	}
}