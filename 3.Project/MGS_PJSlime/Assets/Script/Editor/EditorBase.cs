using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public abstract class EditorBase : EditorWindow {
	void OnEnable() {
		OnInit();
	}

	void OnProjectChange() {
		OnInit();
	}

	void OnDestroy() {
		OnEnd();
	}

	protected abstract void OnInit();
	protected abstract void OnEnd();
}
