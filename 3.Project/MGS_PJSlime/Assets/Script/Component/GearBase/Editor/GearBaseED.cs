using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(TriggerBase))]
public class TriggerBaseED : Editor {
	TriggerBase script;

	public void OnEnable() {
		script = (TriggerBase)target;		
	}

	public override void OnInspectorGUI() { 
		EditorTools.TitleField("觸發機關");
		script.triggerType		= (TriggerBase.TriggerType)EditorTools.EnumField(script.triggerType, "觸發模式");
		script.resetTime		= EditorTools.FloatField(script.resetTime, "按鈕冷卻時間(s)");
		script.weightMode		= EditorTools.BoolField(script.weightMode, "壓力需求");
		script.triggerWeight	= EditorTools.IntField(script.triggerWeight, "壓力值");
		EditorTools.Mig();
	}
}

[CustomEditor(typeof(PatrolBase))]
public class PatrolBaseED : Editor {
	PatrolBase script;

	public void OnEnable() {
		script = (PatrolBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("移動機關");
		script.triggerType  = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType , "機關模式");
		EditorTools.LabelField("壓力機關");
		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("triggers");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

		script.accMode		= EditorTools.BoolField(script.accMode		, "緩衝模式(損毀)");
		script.carryMode	= EditorTools.BoolField(script.carryMode	, "運輸模式");
		script.positive		= EditorTools.BoolField(script.positive		, "正極狀態");
		script.vector		= EditorTools.Vector2Field(script.vector	, "移動速度(m/s)");
		script.onceTime		= EditorTools.FloatField(script.onceTime	, "時間(s)");
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
