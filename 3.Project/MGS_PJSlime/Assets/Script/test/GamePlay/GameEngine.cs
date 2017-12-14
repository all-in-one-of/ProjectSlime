using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour {
	public static GameEngine direct;
	public Transform units;

	private void Start() {
		direct = this;
	}
}