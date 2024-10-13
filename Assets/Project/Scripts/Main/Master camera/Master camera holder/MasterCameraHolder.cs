using System;

using UnityEngine;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraHolder
    {
        public Camera MasterCamera { get; private set; }
        public Transform MasterCameraAnchor { get; private set; }

        public float ViewportLeftBound => MasterCamera.ViewportToWorldPoint(Vector3.zero).x;
        public Vector3 ViewportLeftPosition => new(ViewportLeftBound, 0f, 0f);

        public float ViewportRightBound => MasterCamera.ViewportToWorldPoint(Vector3.right).x;
        public Vector3 ViewportRightPosition => new(ViewportRightBound, 0f, 0f);

        public float ViewportUpperBound => MasterCamera.ViewportToWorldPoint(Vector3.up).y;
        public Vector3 ViewportUpperPosition => new(0f, ViewportUpperBound, 0f);

        public float ViewportLowerBound => MasterCamera.ViewportToWorldPoint(Vector3.zero).y;
        public Vector3 ViewportLowerPosition => new(0f, ViewportLowerBound, 0f);

        public MasterCameraHolder(GameObject masterCameraPrefab)
        {
            GameObject cameraObject = masterCameraPrefab == null ? throw new ArgumentNullException()
                                                                 : UnityEngine.Object.Instantiate(masterCameraPrefab, Vector3.zero, Quaternion.identity);

            MasterCamera = cameraObject.GetComponentInChildren<Camera>();

            if (MasterCamera == null)
            {
                throw new MissingComponentException($"Camera prefab is missing {typeof(Camera)}!");
            }

            MasterCameraAnchor = MasterCamera.transform;
        }

        public float GetDisplacedViewportLeftBound(float factor) => ViewportLeftBound * factor;

        public float GetDisplacedViewportRightBound(float factor) => ViewportRightBound * factor;

        public float GetDisplacedViewportUpperBound(float factor) => ViewportUpperBound * factor;

        public float GetDisplacedViewportLowerBound(float factor) => ViewportLowerBound * factor;

        public bool InsideViewport(Vector2 position) => position.x > ViewportLeftBound &&
                                                        position.x < ViewportRightBound &&
                                                        position.y < ViewportUpperBound &&
                                                        position.y > ViewportLowerBound;
    }
}