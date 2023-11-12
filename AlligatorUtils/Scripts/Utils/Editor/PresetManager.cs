using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Alligator.Utility;
using System.IO;

public class PresetManager : EditorWindow
{
    private static string parentDirectory = "Assets/Saved Presets/";

    //implement the component exclusion such that 
    // option 1 - if the exclude components is selected, then predefined components will be excluded, no need to additionally select which components to exclude
    // option 2 - if the exclude components is selected, user can choose which components to choose by selecting from the drop down field
    // and the selected components will show up in a list below and items from the list can be removed and cleared. And the list will also
    // be cleared after the presets are saved and all the values of the editor window will reset
    // option 3 (ideal) - add a component search box and selected components can be added to the list below and to a global list.
    // option 4 - user can select from a multi select drop down.

    private static List<string> componentList = new List<string>
    {
        "Transform",
        "MeshFilter",
        "MeshRenderer",
        "GraphicRaycaster",
        "CanvasScaler"
    };


    public static List<string> GetComponentsList()
    {
        return componentList;
    }

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
            tooltip = "Creates Commponent Presets for only the selected GameObject",
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
            tooltip = "Creates Commponent Presets of the selected GameObject and also for it's children",
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
            tooltip = "Assigns the created presets to the selected GameObject and it's children",
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
            if(!Directory.Exists(parentDirectory))
            {
                EditorUtility.DisplayDialog("No Presets Found!", "No Presets were found for the selected GameObject", "OK");
                return;
            }

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

        #region unimplemented section, exclude components

        /*Toggle excludeComponentsToggle = new Toggle("Exclude Components?")
        {
            style =
            {
                marginTop = 10,
            }
        };*/

        /*PopupField<string> excludedComponents = new PopupField<string>(GetComponentsList(), componentList[0])
        {
            style =
            {
                width = 180,
            }
        };
        excludedComponents.visible = false;*/

        /*DropdownField excludedComponents = new DropdownField(GetComponentsList(), componentList[0])
        {
            style =
            {
                width = 180,
            }
        };
        excludedComponents.visible = false;*/


        /*excludeComponentsToggle.RegisterValueChangedCallback(evt =>
        {
            excludedComponents.visible = evt.newValue;
        });

        VisualElement excludeComponentsContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginTop = 10,
                justifyContent = Justify.Center
            }
        };*/

        #endregion

        VisualElement saveButtonsContainer = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.Center,
                marginTop = 20,
            }
        };

        VisualElement assignButtonContainer = new VisualElement()
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
        assignButtonContainer.Add(assignPresetsButton);
        //excludeComponentsContainer.Add(excludeComponentsToggle);
        //excludeComponentsContainer.Add(excludedComponents);

        root.Add(header);
        root.Add(saveButtonsContainer);
        root.Add(assignButtonContainer);
        //root.Add(excludeComponentsContainer);

    }
}
