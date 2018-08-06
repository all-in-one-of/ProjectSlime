using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHoster : MonoBehaviour {
	public GameObject gameEngine;
	public GameObject netEngine;

	void Start() {
		if (!GameEngine.direct) {
			Instantiate(gameEngine);
		} else {
			GameEngine.direct.Init();
		}

		if (!SkyTalker.direct) {
			Instantiate(netEngine);
		} else {
			
		}
	}
}
