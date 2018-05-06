using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class EditorTools {
    private static GUILayoutOption STD_WidthS	= GUILayout.Width(100);
    private static GUILayoutOption STD_WidthM	= GUILayout.Width(150);
    private static GUILayoutOption STD_WidthL	= GUILayout.Width(200);
	private static GUILayoutOption STD_WidthXL	= GUILayout.Width(250);

	/// <summary> 繪製水平區塊 </summary>
	public static void BeginH(string value = null, float size = 0) {
		if (string.IsNullOrEmpty(value)) {
			if (size > 0) {
				EditorGUILayout.BeginHorizontal(GUILayout.Width(size));
			} else {
				EditorGUILayout.BeginHorizontal();
			}
		} else {
			if (size > 0) {
				EditorGUILayout.BeginHorizontal(value, GUILayout.Width(size));
			} else {
				EditorGUILayout.BeginHorizontal(value);
			}
		}
	}
	public static void EndH(string value = null) {
        EditorGUILayout.EndHorizontal();
    }

	/// <summary> 繪製垂直區塊 </summary>
	public static void BeginV(string value = null , float size = 0) {
        if (string.IsNullOrEmpty(value)) {
			if (size > 0) {
				EditorGUILayout.BeginVertical(GUILayout.Width(size));
			} else {
				EditorGUILayout.BeginVertical();
			}
        } else {
			if (size > 0) {
				EditorGUILayout.BeginVertical(value, GUILayout.Width(size));
			} else {
				EditorGUILayout.BeginVertical(value);
			}
        }
    }
	public static void EndV(string value = null) {
        EditorGUILayout.EndVertical();
    }

	/// <summary> Gape </summary>
	public static void Gape() {
		EditorGUILayout.LabelField("", GUILayout.Width(10));
	}

	/// <summary> 排版用 </summary>
	public static void Space() {
        EditorGUILayout.Space();
    }

	/// <summary> 搞笑用 </summary>
	public static void Mig() {
		TitleField("Made By MIG");
	}

	/// <summary> 標題顯示用 </summary>
	public static void TitleField(string value) {
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField(value);
		EditorGUILayout.EndHorizontal();
	}

	/// <summary> 資訊顯示用 </summary>
	public static void LabelField(string value , string info = null , string ps = null ) {
        EditorGUILayout.BeginHorizontal();
        if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}		
		if (!string.IsNullOrEmpty(ps)) {
			EditorGUILayout.LabelField(value , STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			EditorGUILayout.LabelField(value);
		}
		EditorGUILayout.EndHorizontal();
    }

	/// <summary> 資訊顯示用 </summary>
	public static string TextArea(string value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) { 
			EditorGUILayout.LabelField(info, STD_WidthS);
		}

		value = EditorGUILayout.TextArea(value, STD_WidthM);

		if (!string.IsNullOrEmpty(ps)) {			
			EditorGUILayout.LabelField(ps);
		} 
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 資訊顯示用 </summary>
	public static string TextField(string value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.TextField(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.TextField(value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> Enum欄位 </summary>
	public static Enum EnumField(Enum value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.EnumPopup(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.EnumPopup(value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> Enum欄位 </summary>
	public static int PopupField(string[] popup,int value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();

		
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.Popup(value , popup, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.Popup(value , popup);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 整數欄位 </summary>
	public static int IntField(int value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.IntField(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.IntField(value, STD_WidthM);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 浮點數欄位 </summary>
	public static float FloatField(float value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		
		value = EditorGUILayout.FloatField(value, STD_WidthM);

		if (!string.IsNullOrEmpty(ps)) {
			EditorGUILayout.LabelField(ps);
		} 
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 浮點數欄位 </summary>
	public static float Slider(float value, float left, float right,string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}

		value = EditorGUILayout.Slider(value, left, right, STD_WidthXL);

		if (!string.IsNullOrEmpty(ps)) {			
			EditorGUILayout.LabelField(ps);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 浮點數欄位 </summary>
	public static double DoubleField(double value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.DoubleField(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.DoubleField(value, STD_WidthM);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 曲線欄位 </summary>
	public static void PropertyField(SerializedProperty value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			EditorGUILayout.PropertyField(value, true, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			EditorGUILayout.PropertyField(value, true);
		}
		EditorGUILayout.EndHorizontal();
	}

	/// <summary> 曲線欄位 </summary>
	public static AnimationCurve CurveField(AnimationCurve value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.CurveField(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.CurveField(value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 顏色欄位 </summary>
	public static Color ColorField(Color value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.ColorField(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.ColorField(value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 物件欄位 </summary>
	public static UnityEngine.Object ObjectField(UnityEngine.Object value , Type type, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {			
			value = EditorGUILayout.ObjectField(value , type, true, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.ObjectField(value, type, true);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> Vector2欄位 </summary>
	public static Vector2 Vector2Field(Vector2 value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.Vector2Field("",value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.Vector2Field("", value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> Vector3欄位 </summary>
	public static Vector3 Vector3Field(Vector3 value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.Vector3Field("", value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.Vector3Field("", value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> Vector4欄位 </summary>
	public static Vector4 Vector4Field(Vector4 value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.Vector4Field("", value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.Vector4Field("", value);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	/// <summary> 布林欄位 </summary>
	public static bool BoolField(bool value, string info = null, string ps = null) {
		EditorGUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(info)) {
			EditorGUILayout.LabelField(info, STD_WidthS);
		}
		if (!string.IsNullOrEmpty(ps)) {
			value = EditorGUILayout.Toggle(value, STD_WidthM);
			EditorGUILayout.LabelField(ps);

		} else {
			value = EditorGUILayout.Toggle(value, STD_WidthM);
		}
		EditorGUILayout.EndHorizontal();
		return value;
	}

	public static void SetColor(Color color , bool enable = true) {
		if (enable) {
			GUI.color = color;
		}
	}
}
