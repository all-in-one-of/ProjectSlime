using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBase : EntityBase {
	public int cannonIndex;
	public List<Vector2> poss;
	public List<Vector2> angles;
	public List<GameObject> bullets;
	public bool isRandom;
	public bool isBonus;
	public float bonusRate = 0.5f;

	public Vector2 aiGape = new Vector2(2, 2);
	public Vector2 aiGapeMove = new Vector2(4, 6);
	public PatrolBase bonusPatrol;

	private Vector2 originPos;

	private float aiClock;
	private float aiClockMove;

	protected override void FStart() {
		base.FStart();
		originPos = transform.position;
	}

	protected override void FFixedUpdate() {
		if (isDead) {
			return;
		}
		AISeqence();
	}

	protected virtual void AISeqence() {
		if (aiClock < Time.timeSinceLevelLoad) {
			GameObject newBullet = PrototypeSystem.direct.SpawnUnit(bullets[cannonIndex], new Vector2(transform.position.x + 1, transform.position.y));
			newBullet.transform.SetParent(GameEngine.direct.units);
			newBullet.GetComponent<ProjectileBase>().FireProjectile(new Vector2(angles[cannonIndex].x, angles[cannonIndex].y).normalized);
			aiClock = Random.Range(aiGape.x, aiGape.y) + Time.timeSinceLevelLoad;
		}

		if (aiClockMove < Time.timeSinceLevelLoad) {
			cannonIndex = cannonIndex + 1 < bullets.Count ? cannonIndex + 1 : 0;
			transform.position = originPos + poss[cannonIndex];

			if (isBonus) {
				aiClock = Random.Range(aiGape.x, aiGape.y) * (1 - bonusPatrol.GetCompleteRate() * bonusRate) + Time.timeSinceLevelLoad;
				aiClockMove = Random.Range(aiGapeMove.x, aiGapeMove.y) * (1 - bonusPatrol.GetCompleteRate() * bonusRate) + Time.timeSinceLevelLoad;
			} else {
				aiClock = Random.Range(aiGape.x, aiGape.y) + Time.timeSinceLevelLoad;
				aiClockMove = Random.Range(aiGapeMove.x, aiGapeMove.y) + Time.timeSinceLevelLoad;
			}
		}
	}
}
