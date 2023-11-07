using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor.UIElements;

public class PresetManager : EditorWindow
{
    [MenuItem("Alligator/Utils/Save and Load Presets %g")]
    public static void SaveLoadPresets()
    {
        var window = GetWindow<PresetManager>("Preset Manager");
        window.minSize = new Vector2(400, 500);
        window.maxSize = new Vector2(400, 500);
    }

    private ListView selectedObjectsList;
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        Label header = new Label("Select gameObject(s) from the heirarchy and\n" + "select the desired operation:")
        {
            style =
            {
                fontSize = 16,
                unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                unityTextAlign = TextAnchor.MiddleCenter
            }
        };

        selectedObjectsList = new ListView();

        root.Add(header);
        //root.Add(selectedObjectsList);



        /*Selection.selectionChanged += UpdateSelectedObjectsList;
        UpdateSelectedObjectsList();*/
    }

    /*private void OnDisable()
    {
        Selection.selectionChanged -= UpdateSelectedObjectsList;
    }
    private void UpdateSelectedObjectsList()
    {
        selectedObjectsList.Clear();

        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            Label selectedObjectLabel = new Label(selectedObject.name)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            selectedObjectsList.Add(selectedObjectLabel);
        }

    }*/
}
