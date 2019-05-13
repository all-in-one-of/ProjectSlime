using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHoster : MonoBehaviour {
	public GameObject gameEngine;
	public GameObject netEngine;

	//log
	public short logLevel;
	public static short logLevelStatic;

	void Start() {
		if (!GameEngine.direct) {
			logLevelStatic = logLevel;
			Instantiate(gameEngine);
		} else {
			GameEngine.direct.Init();
		}

		/*
		if (!SkyTalker.direct) {
			Instantiate(netEngine);
		} else {
			
		}*/
	}

	public static void Log(string logValue, short levelValue = 0) {
		if (logLevelStatic == levelValue) {
			Debug.Log(logValue);
		}
	}
}
