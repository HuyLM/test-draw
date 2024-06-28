using AtoGame.Base.UI;
using UnityEditor;
using UnityEngine;

namespace AtoGame.Base.UI
{
    [CustomEditor(typeof(DOTweenLocalRotate)), CanEditMultipleObjects]
    public class DOTweenLocalRotateInspector : DOTweenTransitionInspector
    {
        private DOTweenLocalRotate dOTweenLocalRotate;
        private SerializedProperty targetProperty;
        private SerializedProperty fromCurrentProperty;
        private SerializedProperty fromProperty;
        private SerializedProperty toProperty;
        private SerializedProperty modeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            dOTweenLocalRotate = transition as DOTweenLocalRotate;
            targetProperty = serializedObject.FindProperty("target");
            fromCurrentProperty = serializedObject.FindProperty("fromCurrent");
            fromProperty = serializedObject.FindProperty("from");
            toProperty = serializedObject.FindProperty("to");
            modeProperty = serializedObject.FindProperty("mode");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(targetProperty);
            EditorGUILayout.PropertyField(modeProperty);

            EditorGUILayout.PropertyField(fromCurrentProperty);
            if (!dOTweenLocalRotate.FromCurrent)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(fromProperty);
                if (GUILayout.Button("Set From", GUILayout.Width(100)))
                {
                    dOTweenLocalRotate.SetFromState();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(toProperty);
            if (GUILayout.Button("Set To", GUILayout.Width(100)))
            {
                dOTweenLocalRotate.SetToState();
            }
            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

