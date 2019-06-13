using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectManager : PoolBase {
	public static EffectManager direct;
	public Dictionary<string, GameObject> data = new Dictionary<string, GameObject>();

	public override void Fstart() {
		direct = this;
		DontDestroyOnLoad(this);
	}

	public override void Init() {
		GameObject[] loader = Resources.LoadAll<GameObject>("Prefab/Effect/");
		foreach (GameObject loaded in loader) {
			data.Add(loaded.name , loaded);
		}
	}

	public override void Play(string index , Vector2 pos) {
		Instantiate(data[index] , pos, Quaternion.identity, transform);
	}
}
