using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundPatrolBase : GearBase {
	public Vector2 pivot;
	public float aroundRadius = 5;
	public float aroundSpeed = 10;
	public bool accMode = true;
	public float startAngle = 0;
	public bool carryMode = false;
	public bool positive = true;

	protected List<Transform> carryobj = new List<Transform>();
	protected bool firstTrigger = false;
	protected bool unTriggerable = false;
	
	protected float angled;

	protected override void FStart() {
		pivot = transform.position;
		angled = startAngle;

		angled += ((positive ? aroundSpeed : -aroundSpeed) * Time.deltaTime) % 360;//累加已经转过的角度
		float posX = aroundRadius * Mathf.Sin(angled * Mathf.Deg2Rad);//计算x位置
		float posY = aroundRadius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置
		transform.position = new Vector2(posX, posY) + pivot;//更新位置

		if (triggerType != TriggerType.always) {
			active = false;
		}
	}

	void FixedUpdate() {
		if (!unTriggerable) {
			if (active || (triggerType == TriggerType.continuous && IsTriggering())) {

				Vector2 shift = transform.position;

				angled += ((positive ? aroundSpeed : -aroundSpeed) * Time.deltaTime) % 360;//累加已经转过的角度
				float posX = aroundRadius * Mathf.Sin(angled * Mathf.Deg2Rad);//计算x位置
				float posY = aroundRadius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置
				shift = new Vector2(posX, posY) + pivot - shift;
				transform.position = new Vector2(posX, posY) + pivot;//更新位置
				CarryObj(shift);				
			}
		}
		if (!active && triggerType == TriggerType.once) {
			if (IsTriggering()) {
				active = true;
			}
		}
	}

	public override bool BaseTrigger() {
		return true;
	}

	public override bool Trigger() {
		if (triggerType == TriggerType.button) {
			if (firstTrigger) {
				positive = !positive;
			}

			unTriggerable = false;
			active = true;
			positive = !positive;
			return BaseTrigger();

		} else if (triggerType == TriggerType.oncebutton) {
			active = true;
			return BaseTrigger();
		}
		return false;
	}

	protected void CarryObj(Vector2 shift) {
		if (carryobj.Count > 0) {
			foreach (Transform obj in carryobj) {
				obj.position = (Vector2)obj.position + shift;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (carryMode && !carryobj.Contains(collision.transform)) {
			carryobj.Add(collision.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (carryMode && carryobj.Contains(collision.transform)) {
			carryobj.Remove(collision.transform);
		}
	}
}
