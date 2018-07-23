using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer {
	public float walkXSpeed = 0;
	public float jumpYForce = 0;
	public float waterXSpeed = 0;
	public float waterYForce = 0;
	public float iceXAcc = 0;
	public float iceXDec = 0;
		
	public void AddEffect(BufferEffect value) {
		walkXSpeed += value.walkXSpeed;
		jumpYForce += value.jumpYForce;
		waterXSpeed += value.waterXSpeed;
		waterYForce += value.waterYForce;
		iceXAcc += value.iceXAcc;
		iceXDec += value.iceXDec;
	}

	public void RefreshEffect() {
		walkXSpeed = 0;
		jumpYForce = 0;
		waterXSpeed = 0;
		waterYForce = 0;
		iceXAcc = 0;
		iceXDec = 0;
}
}

[System.Serializable]
public class BufferEffect {
	public float walkXSpeed = 0;
	public float jumpYForce = 0;
	public float waterXSpeed = 0;
	public float waterYForce = 0;
	public float iceXAcc = 0;
	public float iceXDec = 0;
}
