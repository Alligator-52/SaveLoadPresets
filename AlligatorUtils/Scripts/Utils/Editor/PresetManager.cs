using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using Alligator.Utility;

public class PresetManager : EditorWindow
{
    private static string parentDirectory = "Assets/Saved Presets/";

    [MenuItem("Alligator/Utils/Preset Manager %g")]
    public static void PresetManagerWindow()
    {
        var window = GetWindow<PresetManager>("Preset Manager");
        window.minSize = new Vector2(400, 500);
        window.maxSize = new Vector2(400, 500);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        Label header = new Label("Select gameObject(s) from the heirarchy and\n" + "select the desired operation:")
        {
            style =
            {
                fontSize = 16,
                unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                unityTextAlign = TextAnchor.MiddleCenter,
                marginTop = 15,
            }
        };

        Button saveSinglePresetButton = new Button()
        {
            text = "Save Preset",
            style =
            {
                width = 180,
                height = 50,
            }
        };

        saveSinglePresetButton.clicked += () =>
        {
            var selectedObject = Selection.activeObject;
            if (selectedObject.GetType() != typeof(GameObject)) 
            {
                EditorUtility.DisplayDialog("Invalid Selection!", "The Selected Object is not a GameObject", "OK");
                return;
            }
            var activeGameObject = (GameObject) selectedObject;
            SaveLoadPresets.SaveSinglePreset(activeGameObject, parentDirectory);
        };

        Button saveMultiPresetsButton = new Button()
        {
            text = "Save with Child Presets",
            style =
            {
                width = 180,
                height = 50,
            }
        };

        saveMultiPresetsButton.clicked += () =>
        {
            var selectedObject = Selection.activeObject;
            if (selectedObject.GetType() != typeof(GameObject))
            {
                EditorUtility.DisplayDialog("Invalid Selection!", "The Selected Object is not a GameObject", "OK");
                return;
            }
            var activeGameObject = (GameObject)selectedObject;
            _ = SaveLoadPresets.SavePresetsDFS(parentDirectory, activeGameObject);
        };

        Button assignPresetsButton = new Button()
        {
            text = "Assign Preset(s)",
            style =
            {
                width = 370,
                height = 50,
            }
        };

        assignPresetsButton.clicked += () =>
        {
            Object[] selectedObjects = Selection.objects;
            string parentDirectory = "Assets/Saved Presets/";

            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject.GetType() != typeof(GameObject))
                {
                    EditorUtility.DisplayDialog("Invalid Selection!", "The Selected Object is not a GameObject", "OK");
                    return;
                }
                var selectedGameObject = (GameObject)selectedObject;
                SaveLoadPresets.AssignPresetsDFS(parentDirectory, selectedGameObject); 
            }
        };

        ListView selectedObjectsList = new ListView();

        VisualElement saveButtonsContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.Center,
                marginTop = 20,
            }
        };

        VisualElement listContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.Center,
                marginTop = 10,
            }
        };

        saveButtonsContainer.Add(saveSinglePresetButton);
        saveButtonsContainer.Add(saveMultiPresetsButton);
        listContainer.Add(assignPresetsButton);
        listContainer.Add(selectedObjectsList);

        root.Add(header);
        root.Add(saveButtonsContainer);
        root.Add(listContainer);

    }
}
