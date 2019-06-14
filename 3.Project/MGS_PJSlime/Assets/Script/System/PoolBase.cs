using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase : MonoBehaviour{
	public abstract void Fstart();
	public abstract void Init();
	public abstract GameObject Play(string index , Vector2 pos);

	public int poolSoftSize = 50;

	void Start() {
		Fstart();
	}
}
