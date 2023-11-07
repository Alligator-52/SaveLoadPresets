using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;
using System.IO;
using System;
using System.Threading.Tasks;


namespace Alligator.Utility
{
    public class SaveLoadPresets : Editor
    {
        /*private static List<Type> typeList = new List<Type>
        {
            typeof(Transform),
            typeof(MeshFilter),
            typeof(MeshRenderer),
            typeof(GraphicRaycaster),
            typeof(CanvasScaler)
        };*/

        
        //make changes to loadAssets if necessary

        #region save presets
        [MenuItem("GameObject/Custom Utilities/Save Presets", priority = -100)]
        public static async void SavePresets()
        {
            var selectedObject = Selection.activeObject;

            if (selectedObject.GetType() != typeof(GameObject))
            {
                EditorUtility.DisplayDialog("Inappropriate selection!", "The selected object was not type of GameObject", "Ok");
                return;
            }

            GameObject gameObject = (GameObject)selectedObject;

            string parentDirectory = "Assets/Saved Presets/";
            if (gameObject != null)
            {
                if (gameObject.transform.childCount != 0)
                {
                    var option = EditorUtility.DisplayDialogComplex("Children found", "The selected game object has child game objects",
                                                        ok: "Create preset for only this game Object",
                                                        alt: "Create presets for child game objects too",
                                                        cancel: "Ok");

                    switch (option)
                    {
                        case 0:
                            SaveSinglePreset(gameObject, parentDirectory);
                            break;
                        case 1:
                            Debug.Log("ok was chosen");
                            return;
                        case 2:
                            await SavePresetsDFS(parentDirectory, gameObject);
                            break;
                        default:
                            Debug.Log("Default Option");
                            return;
                    }
                }
                else
                {
                    SaveSinglePreset(gameObject, parentDirectory);
                }

            }
            AssetDatabase.Refresh();
        }

        private static void SaveSinglePreset(GameObject gameObject, string parentDirectory)
        {
            Component[] components = null;
            components = gameObject.GetComponents(typeof(Component));

            string currentDirectory = "";

            if (gameObject.transform.parent == null)
            {
                currentDirectory = $"{parentDirectory}{gameObject.name}/";
            }
            else
            {
                currentDirectory = UtilityMethods.GetHierarchyPath(gameObject.transform, parentDirectory);
            }

            if (!Directory.Exists(currentDirectory))
            {
                Directory.CreateDirectory(currentDirectory);
            }

            if (components.Length != 0)
            {
                foreach (var component in components)
                {
                    Preset preset = new Preset(component);
                    var presetDirectory = $"{currentDirectory}{component.GetType().Name}";
                    AssetDatabase.CreateAsset(preset, $"{presetDirectory}_{component.name}.preset");
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                Debug.Log("components array was empty!");
            }
        }
        private static async Task SavePresetsDFS(string parentFolder, GameObject parentObject)
        {
            if (parentObject == null)
                return;

            Component[] components = null;
            components = parentObject.GetComponents(typeof(Component));

            string currentDirectory = $"{parentFolder}{parentObject.name}/";
            if (!Directory.Exists(currentDirectory))
            {
                Directory.CreateDirectory(currentDirectory);
            }

            if (components.Length != 0)
            {
                foreach (var component in components)
                {
                    Preset preset = new Preset(component);
                    var presetDirectory = $"{currentDirectory}{component.GetType().Name}";
                    AssetDatabase.CreateAsset(preset, $"{presetDirectory}_{component.name}.preset");
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                Debug.Log("components array was empty!");
            }


            //List <Transform> childrenGameObjects = new List<Transform>(parentObject.GetComponentsInChildren<Transform>(true));
            List<Transform> childrenGameObjects = parentObject.GetComponentsInDirectChildren<Transform>();
            childrenGameObjects.Remove(parentObject.transform);

            foreach (var child in childrenGameObjects)
            {
                if (child != parentObject.transform)
                {
                    await SavePresetsDFS(currentDirectory, child.gameObject);
                }
            }

            await Task.Yield();
        }
        #endregion

        #region assign presets
        [MenuItem("GameObject/Custom Utilities/Load Presets", priority = -100)]
        public static void AssignPresets()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            string parentDirectory = "Assets/Saved Presets/";

            foreach (var selectedObject in selectedObjects)
            {
                AssignPresetsDFS(parentDirectory, selectedObject);
            }
        }

        private static void AssignPresetsDFS(string parentDirectory, GameObject parentObject)
        {
            var subDirectories = Directory.GetDirectories(parentDirectory, parentObject.name, SearchOption.AllDirectories);
            var currentDirectory = $"{subDirectories[0].Replace("\\", "/")}/";


            if (!Directory.Exists(currentDirectory))
            {
                Debug.LogWarning($"No presets found for {parentObject.name}");
                return;
            }

            string[] presetFiles = Directory.GetFiles(currentDirectory, "*.preset");

            foreach (var presetFile in presetFiles)
            {
                string componentName = Path.GetFileNameWithoutExtension(presetFile);

                Component component = parentObject.GetComponent(componentName);

                if (component == null)
                {
                    var myComponent = $"{componentName}, Assembly-CSharp";
                    Type componentType = Type.GetType(myComponent);
                    Debug.Log($"Component type of {componentName} is {componentType}");
                    if (componentType != null)
                    {
                        component = parentObject.AddComponent(componentType);
                    }
                    else
                    {
                        Debug.LogWarning($"Component type {componentName} not found.");
                        continue;
                    }
                }

                Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(presetFile);
                if (preset != null)
                {
                    preset.ApplyTo(component);
                }
                else
                {
                    Debug.LogWarning($"Failed to load preset for {componentName}");
                }
            }

            Transform[] childrenGameObject = parentObject.GetComponentsInChildren<Transform>();

            foreach (var child in childrenGameObject)
            {
                if (child != parentObject.transform)
                {
                    AssignPresetsDFS(currentDirectory, child.gameObject);
                }
            }
        }
        #endregion
    }

}

