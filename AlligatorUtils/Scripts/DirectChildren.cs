using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Alligator.Utility
{
    public static class DirectChildren
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
    }
}

