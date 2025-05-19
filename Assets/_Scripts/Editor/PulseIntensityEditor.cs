using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PulseIntensity))]
public class PulseIntensityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isTriggerProp = serializedObject.FindProperty("isTrigger");
        SerializedProperty minBloomProp = serializedObject.FindProperty("minBloomIntensity");
        SerializedProperty maxBloomProp = serializedObject.FindProperty("maxBloomIntensity");
        SerializedProperty speedProp = serializedObject.FindProperty("speed");
        SerializedProperty pulseDurationProp = serializedObject.FindProperty("pulseDuration");

        // Draw default script field (disabled)
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((PulseIntensity)target), typeof(PulseIntensity), false);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(isTriggerProp);
        EditorGUILayout.PropertyField(minBloomProp);
        EditorGUILayout.PropertyField(maxBloomProp);

        if (isTriggerProp.boolValue)
        {
            EditorGUILayout.PropertyField(pulseDurationProp);
        }
        else
        {
            EditorGUILayout.PropertyField(speedProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
