using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHoster : MonoBehaviour {
	public GameObject gameEngine;

	void Start() {
		if (!GameEngine.direct) {
			Instantiate(gameEngine);
		} else {
			GameEngine.direct.Init();
		}
	}

}
