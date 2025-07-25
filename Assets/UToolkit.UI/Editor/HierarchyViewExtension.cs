using UnityEditor;
using UnityEngine;

namespace UToolkit.UI.Editor
{
    [InitializeOnLoad]
    public class HierarchyViewExtension
    {
        static HierarchyViewExtension()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go != null)
            {
                var reference = go.GetComponentInParent<UIReference>();

                if (reference != null && reference.components != null)
                {
                    var components = go.GetComponents<Component>();

                    for (int i = 0; i < components.Length; i++)
                    {
                        if (reference.components.Contains(components[i]))
                        {
                            GUI.DrawTexture(new Rect(selectionRect.xMax - 20, selectionRect.y, 20, selectionRect.height), EditorGUIUtility.IconContent("d_cs Script Icon").image);
                            return;
                        }
                    }
                }
            }
        }
    }
}