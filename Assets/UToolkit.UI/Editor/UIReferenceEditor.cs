using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UToolkit.UI.Editor
{
    [CustomEditor(typeof(UIReference))]
    public class UIReferenceEditor : UnityEditor.Editor
    {
        private readonly string _savePath = "Assets/UToolkit_Gen/UI";

        public override void OnInspectorGUI()
        {
            UIReference reference = (UIReference)target;

            if (reference.components == null) return;

            EditorGUILayout.LabelField("组件引用", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);

            for (int i = 0; i < reference.components.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                var component = reference.components[i];
                if (component != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField(component.gameObject.GetInstanceID().ToString(), GUILayout.Width(80));
                    EditorGUILayout.LabelField(component.GetType().FullName);
                    EditorGUILayout.LabelField(component.name, GUILayout.MaxWidth(150));
                    GUI.enabled = true;
                    if (GUILayout.Button("聚焦", GUILayout.Width(80)))
                    {
                        EditorGUIUtility.PingObject(component);
                    }
                    if (GUILayout.Button("移除", GUILayout.Width(80)))
                    {
                        if (EditorUtility.DisplayDialog("移除组件引用", $"是否确定移除组件引用:{component.name}({component.GetType().FullName})?", "确认", "取消"))
                        {
                            reference.components.RemoveAt(i);
                            EditorUtility.SetDirty(reference);
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button("清除空引用"))
            {
                for (int i = reference.components.Count - 1; i >= 0; i--)
                {
                    if (reference.components[i] == null)
                    {
                        reference.components.RemoveAt(i);
                    }
                }
            }

            if (GUILayout.Button("构建脚本"))
            {
                GenereateScript();
            }
            EditorGUILayout.EndHorizontal();
            EditorUtility.SetDirty(reference);
        }

        private void GenereateScript()
        {
            UIReference reference = (UIReference)target;
            Dictionary<string, int> componentCount = new Dictionary<string, int>();

            string className = $"{target.name}";
            if (!className.EndsWith("View"))
            {
                className = className + "View";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"// Auto-generated script for UIReference {DateTime.Now}");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UToolkit.UI;");
            sb.AppendLine();

            sb.AppendLine($"public class {className} : View");
            sb.AppendLine("{");

            for (int i = 0; i < reference.components.Count; i++)
            {
                Component component = reference.components[i];
                Type type = component.GetType();
                string propertyName = $"{component.name}_{type.Name}";
                if (componentCount.TryGetValue(propertyName, out int count))
                {
                    count++;
                    propertyName = $"{propertyName}{count}";
                    componentCount[propertyName] = count;
                }
                else
                {
                    componentCount[propertyName] = 1;
                }
                sb.AppendLine($"\tpublic {type.FullName} {propertyName} {{ get; private set; }}");
            }

            sb.AppendLine();

            componentCount.Clear();
            sb.AppendLine("\tpublic override void InitViewComponents(UIReference reference)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tbase.InitViewComponents(reference);");
            for (int i = 0; i < reference.components.Count; i++)
            {
                Component component = reference.components[i];
                Type type = component.GetType();
                string propertyName = $"{component.name}_{type.Name}";
                if (componentCount.TryGetValue(propertyName, out int count))
                {
                    count++;
                    propertyName = $"{propertyName}{count}";
                    componentCount[propertyName] = count;
                }
                else
                {
                    componentCount[propertyName] = 1;
                }
                sb.AppendLine($"\t\t{propertyName} = _reference.components[{i}] as {type.FullName};");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            File.WriteAllText($"{_savePath}/{className}.cs", sb.ToString());

            AssetDatabase.Refresh();
        }
    }
}