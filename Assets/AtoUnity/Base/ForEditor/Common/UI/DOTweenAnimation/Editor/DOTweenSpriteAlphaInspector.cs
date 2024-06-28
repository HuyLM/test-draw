
using UnityEditor;
using UnityEngine;

namespace AtoGame.Base.UI
{
    [CustomEditor(typeof(DOTweenSpriteAlpha)), CanEditMultipleObjects]
    public class DOTweenSpriteAlphaInspector : DOTweenTransitionInspector
    {
        private DOTweenSpriteAlpha dOTweenAlpha;
        private SerializedProperty targetProperty;
        private SerializedProperty fromCurrentProperty;
        private SerializedProperty fromProperty;
        private SerializedProperty toProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            dOTweenAlpha = transition as DOTweenSpriteAlpha;
            targetProperty = serializedObject.FindProperty("target");
            fromCurrentProperty = serializedObject.FindProperty("fromCurrent");
            fromProperty = serializedObject.FindProperty("from");
            toProperty = serializedObject.FindProperty("to");

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(targetProperty);

            EditorGUILayout.PropertyField(fromCurrentProperty);
            if (!dOTweenAlpha.FromCurrent)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.Slider(fromProperty, 0, 1);
                if (GUILayout.Button("Set From", GUILayout.Width(100)))
                {
                    dOTweenAlpha.SetFromState();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.Slider(toProperty, 0, 1);
            if (GUILayout.Button("Set To", GUILayout.Width(100)))
            {
                dOTweenAlpha.SetToState();
            }
            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}