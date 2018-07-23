using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(EntityBase))]
public class EntityBaseED : Editor {
	EntityBase script;

	public void OnEnable() {
		script = (EntityBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("實體物件");

		BaseED(script);

		EditorTools.Space();
		EditorTools.Mig();
	}

	public static void BaseED(EntityBase value) {
		value.attack	= EditorTools.IntField(value.attack, "傷害");
		value.hp		= EditorTools.IntField(value.hp, "生命值");
		value.eatAble	= EditorTools.BoolField(value.eatAble, "可食用");
		value.eatBuffer = EditorTools.BoolField(value.eatBuffer, "食用加成");

		if (value.eatBuffer) {
			EditorTools.LabelField("<<食用Buffer>>");
			value.buffer.walkXSpeed = EditorTools.FloatField(value.buffer.walkXSpeed, "移動速度");
			value.buffer.waterXSpeed = EditorTools.FloatField(value.buffer.waterXSpeed, "水中移動速度");
			value.buffer.jumpYForce = EditorTools.FloatField(value.buffer.jumpYForce, "跳躍力");
			value.buffer.waterYForce = EditorTools.FloatField(value.buffer.waterYForce, "水中跳躍力");
			value.buffer.iceXAcc = EditorTools.FloatField(value.buffer.iceXAcc, "滑冰加速度");
			value.buffer.iceXDec = EditorTools.FloatField(value.buffer.iceXDec, "滑冰減速度");
			EditorTools.Space();
		}
	}
}

[CustomEditor(typeof(EnemyBase))]
public class EnemyBaseED : Editor {
	EnemyBase script;

	public void OnEnable() {
		script = (EnemyBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("單位物件");
		EntityBaseED.BaseED(script);
		script.bonus	= EditorTools.IntField(script.bonus		, "分數");
		script.isUndead = EditorTools.BoolField(script.isUndead	, "抗岩漿");

		EditorTools.Space();

		script.aiGape.x = EditorTools.FloatField(script.aiGape.x, "最快動作(s)");
		script.aiGape.y = EditorTools.FloatField(script.aiGape.y, "最慢動作(s)");
		

		script.moveAI		= EditorTools.BoolField(script.moveAI	, "移動AI");
		script.agressiveAI = EditorTools.BoolField(script.agressiveAI, "侵略性AI");

		script.jumpAI		= EditorTools.BoolField(script.jumpAI	, "跳躍AI");
		script.shootAI		= EditorTools.BoolField(script.shootAI	, "射擊AI");

		script.moveShift	= EditorTools.FloatField(script.moveShift, "移動範圍(m)");
		script.moveSpeed	= EditorTools.FloatField(script.moveSpeed, "移動速度(m/s)");			
		script.jumpForce	= EditorTools.FloatField(script.jumpForce, "跳躍力");
		script.bullet		= (GameObject)EditorTools.ObjectField(script.bullet , typeof(GameObject), "彈種");

		EditorTools.Mig();
	}
}

[CustomEditor(typeof(ProjectileBase))]
public class ProjectileBaseED : Editor {
	ProjectileBase script;

	public void OnEnable() {
		script = (ProjectileBase)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("子彈物件");
		EntityBaseED.BaseED(script);
		script.isCruise		= EditorTools.BoolField(script.isCruise , "追蹤模式");
		script.cruiseRate	= EditorTools.FloatField(script.cruiseRate , "追蹤率");
		script.constLifeTime	= EditorTools.FloatField(script.constLifeTime, "持續時間");
		script.constSpeed		= EditorTools.FloatField(script.constSpeed, "飛行速度");

		EditorTools.Space();
		EditorTools.Mig();
	}
}

//[CustomEditor(typeof(PlayerController))]
public class PlayerControllerED : Editor {
	PlayerController script;

	public void OnEnable() {
		script = (PlayerController)target;
	}

	public override void OnInspectorGUI() {
		EditorTools.TitleField("玩家物件");
		EntityBaseED.BaseED(script);

		EditorTools.Space();
		EditorTools.Mig();
	}
}


