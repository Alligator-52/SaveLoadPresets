using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RuntimeSave : Editor
{
    private static RuntimeSave _runtimeSave;
    

    [MenuItem("GameObject/Custom Utilities/Save Game Object")]
    public static void SaveGameObject()
    {
        GameObject activeObject = _runtimeSave.GetActiveGameObject();

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
