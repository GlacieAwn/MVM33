#if FMOD_PRESENT
using FMODUnity;
using UnityEditor;
using UnityEngine;

namespace RCWorld.Audio.Editor
{
    [CustomEditor(typeof(FmodOneShotPlayer))]
    internal sealed class FmodOneShotPlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            FmodAudioParameterEditorUtility.Draw(serializedObject, "eventReference", "parameters", "One Shot Event");
        }
    }

    [CustomEditor(typeof(FmodMusicPlayer))]
    internal sealed class FmodMusicPlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            FmodAudioParameterEditorUtility.Draw(serializedObject, "musicEvent", "parameters", "Music Event", "playOnStart");
        }
    }

    [CustomEditor(typeof(AudioManager))]
    internal sealed class AudioManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("oneShotPlayer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loopedMusicPlayer"));
            EditorGUILayout.Space();
            FmodAudioParameterEditorUtility.Draw(serializedObject, "sceneStartEvent", "sceneStartParameters", "Scene Start Event", false);
            serializedObject.ApplyModifiedProperties();
        }
    }

    internal static class FmodAudioParameterEditorUtility
    {
        public static void Draw(SerializedObject serializedObject, string eventPropertyName, string parametersPropertyName, string label, string additionalPropertyName = null)
        {
            Draw(serializedObject, eventPropertyName, parametersPropertyName, label, additionalPropertyName, true);
        }

        public static void Draw(SerializedObject serializedObject, string eventPropertyName, string parametersPropertyName, string label, bool applyChanges)
        {
            Draw(serializedObject, eventPropertyName, parametersPropertyName, label, null, applyChanges);
        }

        private static void Draw(SerializedObject serializedObject, string eventPropertyName, string parametersPropertyName, string label, string additionalPropertyName, bool applyChanges)
        {
            serializedObject.Update();
            SerializedProperty eventProperty = serializedObject.FindProperty(eventPropertyName);
            SerializedProperty parametersProperty = serializedObject.FindProperty(parametersPropertyName);

            EditorGUILayout.PropertyField(eventProperty, new GUIContent(label));

            if (!string.IsNullOrEmpty(additionalPropertyName))
                EditorGUILayout.PropertyField(serializedObject.FindProperty(additionalPropertyName));

            if (GUILayout.Button("Refresh Event Parameters"))
                RefreshParameters(eventProperty, parametersProperty);

            EditorGUILayout.PropertyField(parametersProperty, new GUIContent("Parameters"), true);

            if (applyChanges)
                serializedObject.ApplyModifiedProperties();
        }

        private static void RefreshParameters(SerializedProperty eventProperty, SerializedProperty parametersProperty)
        {
            EventReference eventReference = eventProperty.GetEventReference();
            EditorEventRef editorEvent = EventManager.EventFromGUID(eventReference.Guid);

            if (editorEvent == null && !string.IsNullOrEmpty(eventReference.Path))
                editorEvent = EventManager.EventFromPath(eventReference.Path);

            if (editorEvent == null)
                return;

            parametersProperty.arraySize = editorEvent.LocalParameters.Count;
            for (int i = 0; i < editorEvent.LocalParameters.Count; i++)
            {
                SerializedProperty parameter = parametersProperty.GetArrayElementAtIndex(i);
                parameter.FindPropertyRelative("Name").stringValue = editorEvent.LocalParameters[i].Name;
                parameter.FindPropertyRelative("Value").floatValue = editorEvent.LocalParameters[i].Default;
            }
        }
    }
}
#endif
