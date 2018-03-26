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
		
		if (script.triggerType == TriggerBase.TriggerType.continuous) {
			EditorTools.TitleField("觸發機關 - 壓力模式");
			script.triggerType = (TriggerBase.TriggerType)EditorTools.EnumField(script.triggerType, "觸發模式");
			
		} else if (script.triggerType == TriggerBase.TriggerType.once) {
			EditorTools.TitleField("觸發機關 - 按鈕模式");
			script.triggerType = (TriggerBase.TriggerType)EditorTools.EnumField(script.triggerType, "觸發模式");
			script.resetTime = EditorTools.FloatField(script.resetTime, "按鈕冷卻時間(s)");
		}
		
		script.weightMode = EditorTools.BoolField(script.weightMode, "壓力需求");
		script.triggerWeight = EditorTools.IntField(script.triggerWeight, "壓力值");

		EditorTools.Mig();
	}
}
