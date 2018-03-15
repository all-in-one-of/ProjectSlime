using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(PatrolBase))]
public class PatrolBaseED : Editor {
	PatrolBase script;

	public void OnEnable() {
		script = (PatrolBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("移動機關");
		script.active		= EditorTools.BoolField(script.active, "自動機關");
		script.accMode		= EditorTools.BoolField(script.accMode, "緩衝模式(損毀)");
		script.carryMode	= EditorTools.BoolField(script.carryMode, "運輸模式");
		script.vector		= EditorTools.Vector2Field(script.vector, "移動速度(m/s)");
		script.onceTime		= EditorTools.FloatField(script.onceTime, "時間(s)");
		EditorTools.Mig();
	}
}

[CustomEditor(typeof(BreakBase))]
public class BreakBaseED : Editor {
	BreakBase script;

	public void OnEnable() {
		script = (BreakBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("碎裂機關");
		script.active = EditorTools.BoolField(script.active, "自動機關");
		script.breakTime = EditorTools.FloatField(script.breakTime, "損壞時間(s)");
		script.resetTime = EditorTools.FloatField(script.resetTime, "回復時間(s)");		
		EditorTools.Mig();
	}
}

[CustomEditor(typeof(SpawnerBase))]
public class SpawnerBaseED : Editor {
	SpawnerBase script;

	public void OnEnable() {
		script = (SpawnerBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("生怪機關");
		script.active = EditorTools.BoolField(script.active, "自動機關");
		EditorGUILayout.LabelField("生成單位");
		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("spawnObject");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();
		
		script.spawnOffset	= EditorTools.Vector2Field(script.spawnOffset, "生成範圍(m)");
		script.limit		= EditorTools.IntField(script.limit, "生成上限");
		script.acGape.x		= EditorTools.FloatField(script.acGape.x, "最快生成(s)");
		script.acGape.y		= EditorTools.FloatField(script.acGape.y, "最慢生成(s)");
		
		
		EditorTools.Mig();
	}
}
