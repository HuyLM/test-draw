using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace AtoGame.Base.UI
{
    [CustomEditor(typeof(DOTweenAnimation)), CanEditMultipleObjects]
    public class DOTweenAnimationInspector : Editor
    {
        private bool isPlaying;
        protected DOTweenAnimation animation;


        protected virtual void OnEnable()
        {
            isPlaying = false;
            animation = target as DOTweenAnimation;
        }

        protected virtual void OnDisable()
        {
            Stop();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Load"))
            {
                animation.LoadTransitions();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(isPlaying);
            if (GUILayout.Button("Play"))
            {
                Play(false);
            }
            if (animation.Delay > 0)
            {
                if (GUILayout.Button("Play Delay"))
                {
                    Play(true);
                }
            }
            EditorGUI.EndDisabledGroup();
          
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(!isPlaying);
            if (GUILayout.Button("Stop"))
            {
                Stop();
            }
            EditorGUI.EndDisabledGroup();

            base.OnInspectorGUI();

        }

        private void Play(bool isDelay)
        {
            isPlaying = true;
            if(isDelay)
            {
                if (Application.isPlaying == false)
                {
                    Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(animation.Delay, () => {

                        foreach (var transition in animation.Transitions)
                        {
                            transition.PlayPreview();
                            DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(transition.Tween);
                        }
                        DG.DOTweenEditor.DOTweenEditorPreview.Start();
                    }), this);
                    return;
                }
            }
            foreach (var transition in animation.Transitions)
            {
                transition.PlayPreview();
                DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(transition.Tween);
            }
            DG.DOTweenEditor.DOTweenEditorPreview.Start();
        }

        IEnumerator CountEditorUpdates(float delay, Action onCompleted)
        {
            var waitForOneSecond = new Unity.EditorCoroutines.Editor.EditorWaitForSeconds(delay);
            yield return waitForOneSecond;
            onCompleted?.Invoke();
        }

        private void Stop()
        {
            if (isPlaying)
            {
                isPlaying = false;

                foreach (var transition in animation.Transitions)
                {
                    transition.StopPreview();
                }
                DG.DOTweenEditor.DOTweenEditorPreview.Stop();

            }
        }
    }
}
