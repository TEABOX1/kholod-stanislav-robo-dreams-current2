using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.

[CustomEditor(typeof(LayersName))]
public class MeshSortingOrderExample : Editor
{
    SerializedProperty m_Name;
    SerializedProperty m_Order;

    private SpriteRenderer rend;

    void OnEnable()
    {
        m_Name = serializedObject.FindProperty("MyName");
        m_Order = serializedObject.FindProperty("MyOrder");
    }

    void CheckRenderer()
    {
        if (Selection.activeGameObject.GetComponent<SpriteRenderer>())
        {
            rend = Selection.activeGameObject.GetComponent<SpriteRenderer>();
            rend.sortingLayerName = m_Name.stringValue;
            rend.sortingOrder = m_Order.intValue;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Name, new GUIContent("Name"));
        EditorGUILayout.PropertyField(m_Order, new GUIContent("Order"));
        CheckRenderer();

        serializedObject.ApplyModifiedProperties();
    }
}