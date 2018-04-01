using UnityEngine;
using System.Collections;
using UnityEditor;
using System;


[CustomEditor(typeof(GroundBase))]
public class GroundBaseED : Editor {
	GroundBase script;

	public void OnEnable() {
		script = (GroundBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("地板物件");
		script.iceMode = EditorTools.BoolField(script.iceMode, "滑冰模式");
		
		/*
	if (script.triggerType == GearBase.TriggerType.once || script.triggerType == GearBase.TriggerType.continuous) {
		EditorTools.TitleField("移動機關 - 壓力模式");
		script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");

		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("triggers");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

	} else if (script.triggerType == GearBase.TriggerType.button || script.triggerType == GearBase.TriggerType.oncebutton) {
		EditorTools.TitleField("移動機關 - 按鈕模式");
		script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");

		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("triggers");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

	} else {
		EditorTools.TitleField("移動機關 - 自動模式");
		script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");
	}

	script.accMode		= EditorTools.BoolField(script.accMode		, "緩衝模式(損毀)");
	script.carryMode	= EditorTools.BoolField(script.carryMode	, "運輸模式");
	script.positive		= EditorTools.BoolField(script.positive		, "正極狀態");
	script.vector		= EditorTools.Vector2Field(script.vector	, "移動速度(m/s)");
	script.onceTime		= EditorTools.FloatField(script.onceTime	, "時間(s)");

	EditorTools.Vector2Field(script.pa, "#A點");
	EditorTools.Vector2Field(script.pb, "#B點");*/
		EditorTools.Mig();
	}
}
