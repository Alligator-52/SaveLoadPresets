using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.IO;
using System.Threading.Tasks;

public class RuntimeSave : Editor
{
    //private static RuntimeSave _runtimeSave;
    private static GameObject _saveObject;
    private static string _savePath = Application.dataPath + "/TempObjects/";

    [MenuItem("GameObject/Custom Utilities/Save Game Object")]
    public static void SaveGameObject()
    {
        //_runtimeSave = This
        var activeSelection = Selection.activeObject;
        if (activeSelection.GetType() != typeof(GameObject))
            return;

        GameObject currentObject = (GameObject)activeSelection;
        
        if (!EditorApplication.isPlaying)
        {
            Debug.Log("We are here now");

            //await CreateDirectory();
            _saveObject = currentObject;
            if (AssetDatabase.IsValidFolder(_savePath))
            {
                AssetDatabase.CreateFolder("Assets", "TempObjects");
            }
            PrefabUtility.SaveAsPrefabAsset(_saveObject, _savePath); //+ currentObject.name + ".prefab");
            //AssetDatabase.CreateAsset(_saveObject, Application.dataPath + _saveObject.name + ".prefab");
            //Debug.Log($"save object : {_saveObject}");
        }

    }

    private static async Task CreateDirectory()
    {
        Debug.Log("Creating Directory");
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
        await Task.Yield();
    }

    [MenuItem("GameObject/Custom Utilities/Load Game Object")]
    public static void LoadGameObject()
    {
        Debug.Log($"save object : {_saveObject}");
        if (_saveObject == null)
        {
            Debug.Log("Nothing found bruh");
            return;
        }
        
        if (!EditorApplication.isPlaying)
        {
            Instantiate(_saveObject);
            _saveObject = null;
        }

    }
}
