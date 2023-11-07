using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alligator.Utility
{
    public static class UtilityMethods
    {
        public static List<T> GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component
        {
            List<T> components = new List<T>();
            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                T component = gameObject.transform.GetChild(i).GetComponent<T>();
                if (component != null)
                    components.Add(component);
            }

            return components;
        }

        public static string GetHierarchyPath(Transform currentTransform, string parentDirectory)
        {
            string hierarchyPath = currentTransform.name;

            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                hierarchyPath = currentTransform.name + "/" + hierarchyPath;
            }

            hierarchyPath = parentDirectory + hierarchyPath + "/";

            return hierarchyPath;
        }
    }
}
