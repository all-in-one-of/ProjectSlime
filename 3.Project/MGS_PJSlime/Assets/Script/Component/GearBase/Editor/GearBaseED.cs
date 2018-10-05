using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(AroundPatrolBase))]
public class AroundPatrolBaseED : Editor {
	AroundPatrolBase script;

	public void OnEnable() {
		script = (AroundPatrolBase)target;
	}

	public override void OnInspectorGUI() {
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

		script.accMode = EditorTools.BoolField(script.accMode, "緩衝模式(損毀)");
		script.carryMode = EditorTools.BoolField(script.carryMode, "運輸模式");
		script.positive = EditorTools.BoolField(script.positive, "正極狀態");

		script.startAngle = EditorTools.FloatField(script.startAngle, "初始角度");
		script.aroundSpeed = EditorTools.FloatField(script.aroundSpeed, "旋轉速度(a/s)");
		script.aroundRadius = EditorTools.FloatField(script.aroundRadius, "半徑(s)");
		
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
		
		script.accMode		= EditorTools.BoolField(script.accMode		, "OLD緩衝模式(損毀)");
		script.carryMode	= EditorTools.BoolField(script.carryMode	, "運輸模式");
		EditorTools.BoolField(script.positive		, "正極狀態");
		script.vector		= EditorTools.Vector2Field(script.vector	, "OLD移動速度(m/s)");
		script.speed		= EditorTools.FloatField(script.speed		, "移動速度(m/s)");
		script.onceTime		= EditorTools.FloatField(script.onceTime	, "OLD時間(s)");

		EditorTools.LabelField(((Vector2)script.transform.position).ToString(), "OLDA點");
		EditorTools.LabelField(((Vector2)script.transform.position + script.vector * script.onceTime).ToString(), "OLDB點");

		float length = 0;
		for (int i = 1; i < script.pointList.Count; i++) {
			length += Vector2.Distance(script.pointList[i - 1], script.pointList[i]);
		}

		EditorTools.LabelField(length.ToString("f2"), "總長度");

		var serializedObject2 = new SerializedObject(target);
		var property2 = serializedObject2.FindProperty("pointList");
		serializedObject2.Update();
		EditorGUILayout.PropertyField(property2, true);
		serializedObject2.ApplyModifiedProperties();

		EditorTools.Mig();
	}

	public void OnSceneGUI() {
		if (script.pointList.Count > 0) {
			GUIStyle lableStyle = new GUIStyle();
			lableStyle.normal.textColor = Color.white;

			int i = 0;
			Handles.Label(script.pointList[i] + new Vector2(0, 0.5f), "P: " + (i + 1), lableStyle);
			Handles.CubeHandleCap(0, script.pointList[i], Quaternion.identity, 0.1f, EventType.Repaint);

			for (i = 1; i < script.pointList.Count; i++) {
				Handles.DrawLine(script.pointList[i - 1], script.pointList[i]);
				Handles.Label(script.pointList[i] + new Vector2(0, 0.5f), "P: " + (i + 1), lableStyle);
				Handles.CubeHandleCap(0, script.pointList[i], Quaternion.identity, 0.1f, EventType.Repaint);
			}
		}
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
		if (script.triggerType == GearBase.TriggerType.once || script.triggerType == GearBase.TriggerType.continuous) {
			EditorTools.TitleField("生怪機關 - 壓力模式");
			script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");

			var serializedObject3 = new SerializedObject(target);
			var property3 = serializedObject3.FindProperty("triggers");
			serializedObject3.Update();
			EditorGUILayout.PropertyField(property3, true);
			serializedObject3.ApplyModifiedProperties();

		} else if (script.triggerType == GearBase.TriggerType.button || script.triggerType == GearBase.TriggerType.oncebutton) {
			EditorTools.TitleField("生怪機關 - 按鈕模式");
			script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");

			var serializedObject3 = new SerializedObject(target);
			var property3 = serializedObject3.FindProperty("triggers");
			serializedObject3.Update();
			EditorGUILayout.PropertyField(property3, true);
			serializedObject3.ApplyModifiedProperties();

		} else {
			EditorTools.TitleField("生怪機關 - 自動模式");
			script.triggerType = (GearBase.TriggerType)EditorTools.EnumField(script.triggerType, "機關模式");
		}

		EditorGUILayout.LabelField("生成單位");
		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("spawnObject");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

		script.toAlways		= EditorTools.BoolField(script.toAlways, "永久啟動");
		script.spawnOffset	= EditorTools.Vector2Field(script.spawnOffset, "生成範圍(m)");
		script.limit		= EditorTools.IntField(script.limit, "生成上限");
		script.acGape.x		= EditorTools.FloatField(script.acGape.x, "最快生成(s)");
		script.acGape.y		= EditorTools.FloatField(script.acGape.y, "最慢生成(s)");
				
		EditorTools.Mig();
	}
}
