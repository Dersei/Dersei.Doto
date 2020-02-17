// Modifications made by Dominik Andrzejczak
// Modifications made by Luiz Wendt
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dersei.Doto.Editor
{
    /// <summary>
    /// Extends how ScriptableObject object references are displayed in the inspector
    /// Shows you all values under the object reference
    /// Also provides a button to create a new ScriptableObject if property is null.
    /// </summary>
    [CustomPropertyDrawer(typeof(DisplayScriptableAttribute), true)]
    public class ExtendedScriptableObjectDrawer : PropertyDrawer
    {
        private DisplayScriptableAttribute _displayScriptableAttribute;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_displayScriptableAttribute is null)
            {
                _displayScriptableAttribute = attribute as DisplayScriptableAttribute;
            }
            var totalHeight = EditorGUIUtility.singleLineHeight;
            if (!IsThereAnyVisibleProperty(property))
                return totalHeight;
            if (property.isExpanded)
            {
                var data = property.objectReferenceValue as ScriptableObject;
                if (data == null) return EditorGUIUtility.singleLineHeight;
                var serializedObject = new SerializedObject(data);
                var prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    do
                    {
                        if (prop.name == "m_Script" && !_displayScriptableAttribute.DisplayScript) continue;
                        var subProp = serializedObject.FindProperty(prop.name);
                        var height = EditorGUI.GetPropertyHeight(subProp, null, true) +
                                     EditorGUIUtility.standardVerticalSpacing;
                        totalHeight += height;
                    } while (prop.NextVisible(false));
                }

                // Add a tiny bit of height if open for the background
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        private void DisplayScriptableObject(Rect position, SerializedProperty property, GUIContent label)
        {
            if (IsThereAnyVisibleProperty(property))
            {
                property.isExpanded =
                    EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth,
                        EditorGUIUtility.singleLineHeight), property.isExpanded, property.displayName, true);
            }
            else
            {
                EditorGUI.LabelField(
                    new Rect(position.x, position.y, EditorGUIUtility.labelWidth,
                        EditorGUIUtility.singleLineHeight), property.displayName);
                property.isExpanded = false;
            }

            EditorGUI.PropertyField(
                new Rect(EditorGUIUtility.labelWidth + 18, position.y, position.width - EditorGUIUtility.labelWidth,
                    EditorGUIUtility.singleLineHeight), property, GUIContent.none, true);
            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
            if (property.objectReferenceValue == null) GUIUtility.ExitGUI();

            if (property.isExpanded)
            {
                // Draw a background that shows us clearly which fields are part of the ScriptableObject
                GUI.Box(
                    new Rect(0,
                        position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing -
                        1, Screen.width,
                        position.height - EditorGUIUtility.singleLineHeight -
                        EditorGUIUtility.standardVerticalSpacing), "");

                EditorGUI.indentLevel++;
                var data = (ScriptableObject) property.objectReferenceValue;
                var serializedObject = new SerializedObject(data);


                // Iterate over all the values and draw them
                var prop = serializedObject.GetIterator();
                var y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (prop.NextVisible(true))
                {
                    do
                    {
                        GUI.enabled = !_displayScriptableAttribute.DisableEditing;

                        switch (prop.name)
                        {
                            // Don't bother drawing the class file
                            case "m_Script" when !_displayScriptableAttribute.DisplayScript:
                                continue;
                            case "m_Script" when _displayScriptableAttribute.DisplayScript:
                                GUI.enabled = false;
                                break;
                        }

                        var height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                        EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
                        y += height + EditorGUIUtility.standardVerticalSpacing;
                        GUI.enabled = true;
                    } while (prop.NextVisible(false));
                }

                if (GUI.changed)
                    serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel--;
            }
        }

        private void DisplayEmptyObject(Rect position, SerializedProperty property, GUIContent label)
        {
            var creationCheck = _displayScriptableAttribute?.AllowCreation ?? false;
            if (!creationCheck)
            {
                EditorGUI.ObjectField(
                    new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property);
            }
            else
            {
                EditorGUI.ObjectField(
                    new Rect(position.x, position.y, position.width - 60, EditorGUIUtility.singleLineHeight), property);
                if (GUI.Button(
                    new Rect(position.x + position.width - 58, position.y, 58, EditorGUIUtility.singleLineHeight),
                    "Create"))
                {
                    var selectedAssetPath = "Assets";
                    if (property.serializedObject.targetObject is MonoBehaviour)
                    {
                        var ms = MonoScript.FromMonoBehaviour(
                            (MonoBehaviour) property.serializedObject.targetObject);
                        selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
                    }

                    var type = fieldInfo.FieldType;
                    if (type.IsArray) type = type.GetElementType();
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                        type = type.GetGenericArguments()[0];
                    property.objectReferenceValue = CreateAssetWithSavePrompt(type, selectedAssetPath);
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_displayScriptableAttribute is null)
            {
                _displayScriptableAttribute = attribute as DisplayScriptableAttribute;
            }
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue != null)
            {
                DisplayScriptableObject(position, property, label);
            }
            else
            {
                DisplayEmptyObject(position, property, label);
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        // Creates a new ScriptableObject via the default Save File panel
        private static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "New " + type.Name + ".asset", "asset",
                "Enter a file name for the ScriptableObject.", path);
            if (path == "") return null;
            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }

        private bool IsThereAnyVisibleProperty(SerializedProperty property)
        {
            var data = (ScriptableObject) property.objectReferenceValue;
            if (data is null) return false;
            var serializedObject = new SerializedObject(data);

            var prop = serializedObject.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.name == "m_Script" && !_displayScriptableAttribute.DisplayScript) continue;
                return true; //if theres any visible property other than m_script
            }

            return false;
        }
    }
}
#endif