using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class FrameWork : EditorWindow {

    void OnEnable() {
        UpdateInfo();
    }

    void OnHierarchyChange() {
        UpdateInfo();
    }

    protected abstract void UpdateInfo();

    protected void BeginH(){
        GUILayout.BeginHorizontal();
    }

    protected void BeginV() {
        GUILayout.BeginVertical();
    }

    protected void BeginV(string ask) {
        GUILayout.BeginVertical(ask);
    }

    protected void EndH() {
        GUILayout.EndHorizontal();
    }

    protected void EndV() {
        GUILayout.EndVertical();
    }
}
