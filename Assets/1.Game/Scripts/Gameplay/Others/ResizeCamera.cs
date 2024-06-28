using AtoGame.Base.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [RequireComponent(typeof(Camera))]
    public class ResizeCamera : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField, Range(0f, 10f)] private float updateRate = 1f;


        private float currentAspect;
        private WaitForSeconds waitUpdate;
        private Camera _camera;
        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            StartCoroutine(AutoUpdate());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void CalculatorSize()
        {
            if (_camera == null) return;
            float aspect = Screen.height * 1f / Screen.width;
            if (aspect == currentAspect) return;

            float outputOrthoSize = curve.Evaluate(aspect);
            _camera.orthographicSize = outputOrthoSize;
            currentAspect = aspect;
        }

        private IEnumerator AutoUpdate()
        {
            while (true)
            {
                CalculatorSize();
                yield return Yielder.Wait(updateRate);
            }
        }
    }
}
