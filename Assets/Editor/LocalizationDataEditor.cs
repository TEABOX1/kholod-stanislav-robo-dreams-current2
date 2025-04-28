using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AllInOne
{
    [CustomEditor(typeof(LocalizationData))]
    public class LocalizationDataEditor : UnityEditor.Editor
    {
        private bool _languagesFoldout = false;
        private bool _termsFoldout = false;
        private bool _dataFoldout = false;
        private bool[] _languageFoldouts;
        private GUIContent[] _termLabels;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _languagesFoldout = EditorGUILayout.Foldout(_languagesFoldout, "Languages");
            
            SerializedProperty languages = serializedObject.FindProperty("_languages");
            SerializedProperty terms = serializedObject.FindProperty("_terms");
            SerializedProperty languageEntries = serializedObject.FindProperty("_languageEntries");

            if (_languagesFoldout)
            {
                EditorGUI.indentLevel++;
                
                for (int i = 0; i < languages.arraySize; ++i)
                {
                    SerializedProperty language = languages.GetArrayElementAtIndex(i);
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUILayout.PropertyField(language);
                    
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        languages.DeleteArrayElementAtIndex(i);
                        languageEntries.DeleteArrayElementAtIndex(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Language"))
                {
                    languages.InsertArrayElementAtIndex(languages.arraySize);
                }
                
                EditorGUI.indentLevel--;
            }

            if (_languageFoldouts == null || _languageFoldouts.Length != languages.arraySize)
                Array.Resize(ref _languageFoldouts, languages.arraySize);
            
            _termsFoldout = EditorGUILayout.Foldout(_termsFoldout, "Terms");
            
            if (_termsFoldout)
            {
                EditorGUI.indentLevel++;
                
                for (int i = 0; i < terms.arraySize; ++i)
                {
                    SerializedProperty term = terms.GetArrayElementAtIndex(i);
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUILayout.PropertyField(term);
                    
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        terms.DeleteArrayElementAtIndex(i);
                        for (int j = 0; j < languageEntries.arraySize; ++j)
                        {
                            languageEntries.GetArrayElementAtIndex(j).FindPropertyRelative("terms").DeleteArrayElementAtIndex(i);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Term"))
                {
                    terms.InsertArrayElementAtIndex(terms.arraySize);
                }
                
                EditorGUI.indentLevel--;
            }
            
            if (_termLabels == null || _termLabels.Length != terms.arraySize)
            {
                Array.Resize(ref _termLabels, terms.arraySize);
            }
            
            for (int i = 0; i < terms.arraySize; ++i)
            {
                string term = terms.GetArrayElementAtIndex(i).stringValue;
                if (_termLabels[i] == null)
                {
                    _termLabels[i] = new GUIContent(term);
                }
                else if (_termLabels[i].text != term)
                {
                    _termLabels[i].text = term;
                }
            }
            
            _dataFoldout = EditorGUILayout.Foldout(_dataFoldout, "Data");

            if (languageEntries.arraySize != languages.arraySize)
            {
                languageEntries.arraySize = languages.arraySize;
            }
            
            for (int i = 0; i < languageEntries.arraySize; ++i)
            {
                if (languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("terms").arraySize != terms.arraySize)
                {
                    languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("terms").arraySize = terms.arraySize;
                }
            }
            
            if (_dataFoldout)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < languageEntries.arraySize; ++i)
                {
                    _languageFoldouts[i] = EditorGUILayout.Foldout(_languageFoldouts[i], languages.GetArrayElementAtIndex(i).stringValue);
                    
                    if (_languageFoldouts[i])
                    {
                        EditorGUI.indentLevel++;
                        SerializedProperty termArray = languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("terms");
                        
                        for (int j = 0; j < termArray.arraySize; ++j)
                        {
                            EditorGUILayout.PropertyField(termArray.GetArrayElementAtIndex(j), _termLabels[j]);
                        }
                        
                        EditorGUI.indentLevel--;
                    }
                }
                
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}