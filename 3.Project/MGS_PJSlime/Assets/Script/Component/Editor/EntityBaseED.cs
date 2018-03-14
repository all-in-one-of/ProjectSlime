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
		script.attack	= EditorTools.IntField(script.attack	, "傷害");
		script.hp		= EditorTools.IntField(script.hp		, "生命值");
		script.eatAble	= EditorTools.BoolField(script.eatAble	, "可食用");

		EditorTools.Space();

		script.aiGape.x = EditorTools.FloatField(script.aiGape.x, "AI低頻(s)");
		script.aiGape.y = EditorTools.FloatField(script.aiGape.y, "AI高頻(s)");
		

		script.moveAI	= EditorTools.BoolField(script.moveAI	, "移動AI");
		script.jumpAI	= EditorTools.BoolField(script.jumpAI	, "跳躍AI");
		script.shootAI	= EditorTools.BoolField(script.shootAI	, "射擊AI");

		script.moveShift	= EditorTools.FloatField(script.moveShift, "移動範圍(m)");
		script.moveSpeed	= EditorTools.FloatField(script.moveSpeed, "移動速度(m/s)");			
		script.jumpForce	= EditorTools.FloatField(script.jumpForce, "跳躍力");
		script.bullet		= (GameObject)EditorTools.ObjectField(script.bullet , typeof(GameObject), "彈種");
		script.bulletSpeed	= EditorTools.FloatField(script.bulletSpeed, "槍口初速(m/s)");
		
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
		script.attack	= EditorTools.IntField(script.attack	, "傷害");
		script.hp		= EditorTools.IntField(script.hp		, "生命值");
		script.eatAble	= EditorTools.BoolField(script.eatAble	, "可食用");

		EditorTools.Space();
		EditorTools.Mig();
	}
}
