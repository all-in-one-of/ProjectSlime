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
		script.attack = EditorTools.IntField(script.attack		, "傷害");
		script.hp = EditorTools.IntField(script.hp				, "生命值");
		script.eatAble = EditorTools.BoolField(script.eatAble	, "可食用");

		EditorTools.Space();
		EditorTools.Mig();
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
		script.attack	= EditorTools.IntField(script.attack	, "傷害");
		script.hp		= EditorTools.IntField(script.hp		, "生命值");
		script.bonus	= EditorTools.IntField(script.bonus		, "分數");
		script.eatAble	= EditorTools.BoolField(script.eatAble	, "可食用");
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
		script.bulletSpeed	= EditorTools.FloatField(script.bulletSpeed, "槍口初速(m/s)");
		script.bulletLife	= EditorTools.FloatField(script.bulletLife, "子彈持續時間(s)");

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
		script.attack	= EditorTools.IntField(script.attack	, "傷害");
		script.hp		= EditorTools.IntField(script.hp		, "生命值");
		script.eatAble	= EditorTools.BoolField(script.eatAble	, "可食用");

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
		script.hp = EditorTools.IntField(script.hp, "生命值");
		script.eatAble = EditorTools.BoolField(script.eatAble, "可食用");

		EditorTools.Space();
		EditorTools.Mig();
	}
}
