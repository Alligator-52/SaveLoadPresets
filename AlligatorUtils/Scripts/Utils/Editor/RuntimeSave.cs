using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

public class RuntimeSave : Editor
{
    private static GameObject _saveObject;
    private static string _savePath = "Assets/TempObjects";

    [MenuItem("GameObject/Custom Utilities/Save Game Object", priority = -100000)]
    public static void SaveGameObject()
    {
        var activeSelection = Selection.activeObject;
        if (activeSelection.GetType() != typeof(GameObject))
            return;

        GameObject currentObject = (GameObject)activeSelection;

        _saveObject = currentObject;
        if (!AssetDatabase.IsValidFolder(_savePath))
        {
            AssetDatabase.CreateFolder("Assets", $"TempObjects");

        }
        PrefabUtility.SaveAsPrefabAsset(_saveObject, _savePath + $"/{currentObject.name}.prefab");

    }

    [MenuItem("GameObject/Custom Utilities/Load Game Object", priority = -100000)]
    public static void LoadGameObject()
    {
        var prefabFiles = Directory.GetFiles(_savePath, "*.prefab");
        for(int i = 0; i < prefabFiles.Length; i++)
        {
            var savedObject = AssetDatabase.LoadAssetAtPath<Object>(prefabFiles[i].Replace("\\","/"));
            _saveObject = (GameObject)savedObject;
            var newObject = Instantiate(_saveObject);
            newObject.name = _saveObject.name;
        }
    }
}
