using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UToolkit.UI.Editor
{
    public class UIReferenceSelectorEditor : EditorWindow
    {
        private UIReference _reference;
        private Component[] _components;
        private Vector2 _scrollPosition;

        [MenuItem("UToolkit/UI/引用选择器 %e")]
        private static void ShowWindow()
        {
            UIReferenceSelectorEditor window = GetWindow<UIReferenceSelectorEditor>("引用选择器");
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(600, 800);
        }

        private void OnEnable()
        {
            LoadUIReference();
            Selection.selectionChanged += LoadUIReference;
        }

        private void OnDestroy()
        {
            Selection.selectionChanged -= LoadUIReference;
        }

        private void OnGUI()
        {
            if (_reference == null)
            {
                EditorGUILayout.HelpBox("在选定的对象或它的父对象中没有找到\"UIReference\"", MessageType.Warning);
                return;
            }

            // 绘制组件
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < _components.Length; i++)
            {
                Component component = _components[i];
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(component.GetType().FullName);

                if (_reference.components.Contains(component))
                {
                    GUI.color = Color.red;
                    if (GUILayout.Button("移除", GUILayout.Width(60)))
                    {
                        _reference.components.Remove(component);
                        EditorUtility.SetDirty(_reference);
                    }
                }
                else
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button("添加", GUILayout.Width(60)))
                    {
                        _reference.components.Add(component);
                        EditorUtility.SetDirty(_reference);
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void LoadUIReference()
        {
            Debug.Log("Loading UIReference...");
            if (Selection.activeTransform == null) return;
            Transform selectedTransform = Selection.activeTransform;
            _reference = selectedTransform.GetComponentInParent<UIReference>();
            _components = selectedTransform.GetComponents<Component>();
        }
    }
}