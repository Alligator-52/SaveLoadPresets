using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class RuntimeSave : Editor
{
    //private static RuntimeSave _runtimeSave;
    private static GameObject _saveObject;

    [MenuItem("GameObject/Custom Utilities/Save Game Object")]
    public static void SaveGameObject()
    {
        //_runtimeSave = This
        var activeSelection = Selection.activeObject;
        if (activeSelection.GetType() != typeof(GameObject))
            return;

        GameObject currentObject = (GameObject)activeSelection;
        
        if (EditorApplication.isPlaying)
        {
            Debug.Log("We are here now");
            _saveObject = currentObject;
            Debug.Log($"save object : {_saveObject}");
        }

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

    public GameObject GetActiveGameObject()
    {
        var activeSelection = Selection.activeObject;
        if (activeSelection.GetType() != typeof(GameObject))
            return null;

        var activeGameObj = (GameObject)activeSelection;

        return activeGameObj;
    }
}
