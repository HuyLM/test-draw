using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class GameplayCamera : SingletonBind<GameplayCamera>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Camera findoutCamera;

        public Camera GetCamera(bool isFindOut = false)
        {
            if(findoutCamera)
            {
                return findoutCamera;
            }
            return _camera;
        }

        protected override void OnAwake()
        {
            if(_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
        }


        public Vector2 GetRightPosition()
        {
            float orthoSize = findoutCamera.orthographicSize;
            float aspect = findoutCamera.aspect;
            float camereWidthSize = orthoSize * aspect;// half
            return new Vector2(camereWidthSize, 0);
        }

        public Vector2 GetTopPosition()
        {
            float orthoSize = findoutCamera.orthographicSize;
            return new Vector2(0, orthoSize);
        }
    }
}
