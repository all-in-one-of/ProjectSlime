using UnityEngine;
using System.Collections;
using UnityEditor;
using System;


[CustomEditor(typeof(SpawnerBase))]
public class SpawnerBaseED : Editor {
	SpawnerBase script;

	public void OnEnable() {
		script = (SpawnerBase)target;
	}

	public override void OnInspectorGUI() {
		EditorGUILayout.LabelField("生成單位");
		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("spawnObject");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();
		
		script.spawnOffset = EditorTools.Vector2Field(script.spawnOffset, "生成範圍(m)");
		script.acGape.x = EditorTools.FloatField(script.acGape.x, "最快生成(s)");
		script.acGape.y = EditorTools.FloatField(script.acGape.y, "最慢生成(s)");
		
		
		EditorTools.Mig();
	}
}
