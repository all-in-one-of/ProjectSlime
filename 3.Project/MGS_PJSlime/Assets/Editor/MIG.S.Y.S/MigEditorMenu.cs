using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MigEditorMenu : EditorWindow {


    private static void FileCheck() {
        string infoName;
        string assetPath = "Assets/Resources/Info/";

        if (!Directory.Exists(assetPath)) {
            Directory.CreateDirectory(assetPath);
        }
        
        infoName = "RegionInfo";
        if(!File.Exists(assetPath + infoName + ".asset")) {

            //RegionInfo regionInfo = ScriptableObject.CreateInstance<RegionInfo>();
            //AssetDatabase.CreateAsset(regionInfo, assetPath + infoName + ".asset");
        }
    }


    [MenuItem("MigEditor/方格編輯功能/區域編輯器", false, 0)]
    static void ModelCreater() {
        FileCheck();
        //EditorWindow.GetWindow(typeof(RegionEditor));
    }
}