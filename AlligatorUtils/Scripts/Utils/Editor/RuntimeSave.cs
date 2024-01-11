using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RuntimeSave : Editor
{
    private static RuntimeSave _runtimeSave;
    private static GameObject _saveObject;

    [MenuItem("GameObject/Custom Utilities/Save Game Object")]
    public static void SaveGameObject()
    {
        GameObject activeObject = _runtimeSave.GetActiveGameObject();
        if (activeObject == null)
            return;
        if (EditorApplication.isPlaying)
        {
            _saveObject = activeObject;
        }

    }

    [MenuItem("GameObject/Custom Utilities/Load Game Object")]
    public static void LoadGameObject()
    {
        if(_saveObject == null)
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


    private void OnEnable()
    {
        _runtimeSave = this;
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
