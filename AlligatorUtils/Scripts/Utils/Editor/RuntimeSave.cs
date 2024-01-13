using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

public class RuntimeSave : Editor
{
    private static GameObject _saveObject;
    private static string _savePath = "Assets/TempObjects";

    [MenuItem("GameObject/Custom Utilities/Save GameObjects", priority = -100000)]
    public static void SaveGameObject()
    {
        Object[] activeSelections = Selection.objects;
        for (int i = 0; i < activeSelections.Length; i++)
        {
            if (activeSelections[i].GetType() != typeof(GameObject))
            {
                Debug.Log($"{activeSelections[i]} is an invalid type, couldnt save!");
                continue;
            }
            GameObject currentObject = (GameObject)activeSelections[i];
            _saveObject = currentObject;
            if (!AssetDatabase.IsValidFolder(_savePath))
            {
                AssetDatabase.CreateFolder("Assets", $"TempObjects");

            }
            PrefabUtility.SaveAsPrefabAsset(_saveObject, _savePath + $"/{currentObject.name}.prefab");
        }
    }

    [MenuItem("GameObject/Custom Utilities/Load GameObjects", priority = -100000)]
    public static void LoadGameObject()
    {
        var prefabFiles = Directory.GetFiles(_savePath, "*.prefab");
        for (int i = 0; i < prefabFiles.Length; i++)
        {
            Object savedObject = AssetDatabase.LoadAssetAtPath<Object>(prefabFiles[i].Replace("\\", "/"));
            _saveObject = (GameObject)savedObject;
            GameObject newObject = Instantiate(_saveObject);
            newObject.name = _saveObject.name;
        }

        List<string> outFailedPaths = new List<string>();
        AssetDatabase.DeleteAssets(prefabFiles, outFailedPaths);
    }
}