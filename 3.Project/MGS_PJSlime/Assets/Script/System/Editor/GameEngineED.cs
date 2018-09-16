using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System;

[CustomEditor(typeof(GameEngine))]
public class GameEngineED : Editor {
	GameEngine script;

	public void OnEnable() {
		script = (GameEngine)target;
	}

	public override void OnInspectorGUI() {
		SerializedObject serializedObject;
		SerializedProperty property;
		
		EditorTools.TitleField("<<Prefab>>");
		script.cameraManager = (GameObject)EditorTools.ObjectField(script.cameraManager, typeof(GameObject), "CameraManager");
		script.audioManager = (GameObject)EditorTools.ObjectField(script.audioManager, typeof(GameObject) , "AudioManager" );
		script.uiManager = (GameObject)EditorTools.ObjectField(script.uiManager, typeof(GameObject), "UIManager");
		EditorTools.Space();

		EditorTools.TitleField("<<Init>>");
		script.preTester = EditorTools.BoolField(script.preTester, "測試場景");
		if (script.preTester) {
			EditorTools.LabelField("(Assets\\Resources\\Stage)", "<<TestStage>>");
			script.testStage = (GameObject)EditorTools.ObjectField(script.testStage, typeof(GameObject), "目標場景");

		} else {
			EditorTools.LabelField("(Assets\\Resources\\Stage)", "<<StageList>>");
			serializedObject = new SerializedObject(target);
			property = serializedObject.FindProperty("stageList");
			serializedObject.Update();
			EditorGUILayout.PropertyField(property, true);
			serializedObject.ApplyModifiedProperties();
		}

		EditorTools.Space();

		EditorTools.TitleField("<<PlayerSet>>");
		serializedObject = new SerializedObject(target);
		property = serializedObject.FindProperty("players");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

		serializedObject = new SerializedObject(target);
		property = serializedObject.FindProperty("playerUIs");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();
		
		EditorTools.Space();
		EditorTools.TitleField("<<ArenaSet>>");

		EditorTools.LabelField("-生成參數-");
		script.broSize		= EditorTools.IntField(script.broSize		, "大哥大小");
		script.baseSize		= EditorTools.IntField(script.baseSize		, "一般大小");
		script.bornSize		= EditorTools.IntField(script.bornSize		, "重生大小");
		script.bornReqSize	= EditorTools.IntField(script.bornReqSize	, "重生需求量");
		script.bornCost		= EditorTools.IntField(script.bornCost		, "重生消耗");

		EditorTools.Space();
		EditorTools.LabelField("-地面參數-");
		script.walkXSpeed	= EditorTools.FloatField(script.walkXSpeed	, "移動速度(m/s)");
		script.walkXAcc		= EditorTools.FloatField(script.walkXAcc	, "加速度(m/s)");
		script.walkXDec		= EditorTools.FloatField(script.walkXDec	, "減速度(m/s)");

		EditorTools.Space();
		EditorTools.LabelField("-跳躍參數-");
		script.jumpGape		= EditorTools.IntField(script.jumpGape		, "跳躍力");
		script.jumpXSpeed	= EditorTools.FloatField(script.jumpXSpeed	, "移動速度(m/s)");
		script.jumpXAcc		= EditorTools.FloatField(script.jumpXAcc	, "加速度(m/s)");
		script.jumpXDec		= EditorTools.FloatField(script.jumpXDec	, "減速度(m/s)");
		script.jumpYForce	= EditorTools.FloatField(script.jumpYForce	, "跳躍速度(m/s)");
		script.jumpDuraion	= EditorTools.FloatField(script.jumpDuraion	, "跳躍時間(s)");
		script.jumpYDec		= EditorTools.FloatField(script.jumpYDec	, "落下減速度(m/s)");

		EditorTools.Space();
		EditorTools.LabelField("-水中參數-");
		script.waterXSpeed	 = EditorTools.FloatField(script.waterXSpeed, "移動速度(m/s)");
		script.waterXAcc	 = EditorTools.FloatField(script.waterXAcc, "加速度(m/s)");
		script.waterXDec	 = EditorTools.FloatField(script.waterXDec, "減速度(m/s)");
		script.waterYForce   = EditorTools.FloatField(script.waterYForce, "跳躍速度(m/s)");
		script.waterColdDown = EditorTools.FloatField(script.waterColdDown, "跳躍冷卻(s)");
		script.waterYDec	 = EditorTools.FloatField(script.waterYDec, "落下減速度(m/s)");
		script.waterYSpeed	= EditorTools.FloatField(script.waterYSpeed, "落下速度(m/s)");

		EditorTools.Space();
		EditorTools.LabelField("-冰上參數-");
		script.iceXAcc		= EditorTools.FloatField(script.iceXAcc		, "加速度(m/s)");
		script.iceXDec		= EditorTools.FloatField(script.iceXDec		, "減速度(m/s)");
		
		EditorTools.Mig();
	}
}
