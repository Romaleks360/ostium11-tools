using System;
using System.Collections.Generic;
using Ostium11.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ostium11.Components
{
    public class UniversalCameraController : MonoBehaviour
    {
        #region Types

        struct InputData
        {
            public Vector2 dragDelta;    // one finger / LMB drag. pixel coords
            public Vector2 altDragDelta; // two finger / RMB drag. pixel coords
            public Vector2 zoomCenter;   // two finger median / mouse position. pixel coords
            public float zoomDelta;      // two finger pinch / mouse wheel. percentage
        }

        #region Input Mapping

        enum DragAction { None, RotateSelf, RotateAroundPivot, MoveXY }

        enum ZoomAction { None, Fov, OrthoSize, MoveToPivot, MoveZ }

        [Serializable]
        struct ControlScheme
        {
            public DragAction dragAction;
            public DragAction altDragAction;
            public ZoomAction zoomAction;
            public bool zoomTowardsPointer;
        }

        #endregion

        #region Device Input

        interface IInputProvider
        {
            public void Reset();
            public bool TryGetInput(out InputData input);
        }

        class MouseInput : IInputProvider
        {
            Vector2 _prevMousePos;
            bool _enabled = true;

            public void Reset()
            {
                _prevMousePos = Input.mousePosition;
                _enabled = true;
            }

            public bool TryGetInput(out InputData input)
            {
                if (!Input.mousePresent)
                {
                    input = new InputData();
                    return false;
                }

                Vector2 mousePos = Input.mousePosition;

                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    _prevMousePos = mousePos;
                    _enabled = !EventSystem.current.IsPointerOverGameObject();
                }

                if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
                    _enabled = true;

                if (!_enabled)
                {
                    input = new InputData();
                    return false;
                }

                input = new InputData()
                {
                    dragDelta = Input.GetMouseButton(1) ? mousePos - _prevMousePos : Vector2.zero,
                    altDragDelta = Input.GetMouseButton(2) ? mousePos - _prevMousePos : Vector2.zero,
                    zoomCenter = mousePos,
                    zoomDelta = Input.mouseScrollDelta.y / 10,
                };

#if UNITY_WEBGL && !UNITY_EDITOR
                input.dragDelta /= 5;
                input.altDragDelta /= 5;
#endif

                _prevMousePos = mousePos;

                return true;
            }
        }

        class TouchInput : IInputProvider
        {
            Vector2? _prevTouchPos;
            Vector3? _prevPinchPos;
            bool _enabled = true;

            public void Reset()
            {
                _prevTouchPos = null;
                _prevPinchPos = null;
                _enabled = true;
            }

            public bool TryGetInput(out InputData input)
            {
                if (Input.touchCount == 0)
                {
                    Reset();
                    input = new InputData();
                    return false;
                }
                else if (Input.touchCount == 1)
                {
                    var touch = Input.GetTouch(0);
                    if (_prevTouchPos == null)
                        _enabled = !EventSystem.current.IsPointerOverGameObject(touch.fingerId);

                    if (!_enabled)
                    {
                        _prevTouchPos = touch.position;
                        input = new InputData();
                        return false;
                    }

                    var dragDelta = Vector2.zero;
                    var touchPos = touch.position;

                    if (_prevTouchPos != null)
                        dragDelta = touchPos - _prevTouchPos.Value;

                    _prevTouchPos = touchPos;
                    _prevPinchPos = null;

                    input = new InputData() { dragDelta = dragDelta, };
                    return true;
                }
                else
                {
                    if (!_enabled)
                    {
                        input = new InputData();
                        return false;
                    }

                    var altDragDelta = Vector2.zero;
                    var zoomDelta = 0f;

                    var touchPos1 = Input.GetTouch(0).position;
                    var touchPos2 = Input.GetTouch(1).position;

                    var pinchPos = (touchPos1 + touchPos2) / 2;
                    var dst = (touchPos1 - touchPos2).magnitude;

                    if (_prevPinchPos != null)
                    {
                        altDragDelta = pinchPos - _prevPinchPos.Value.GetXY();
                        zoomDelta = dst / _prevPinchPos.Value.z - 1f;
                    }

                    _prevTouchPos = null;
                    _prevPinchPos = new Vector3(pinchPos.x, pinchPos.y, dst);

                    input = new InputData()
                    {
                        altDragDelta = altDragDelta,
                        zoomCenter = pinchPos,
                        zoomDelta = zoomDelta,
                    };
                    return true;
                }
            }
        }

        #endregion

        public class POV
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public float PivotDst;
            public float Fov;
            public float OrthoSize;
        }

        #endregion

        [SerializeField] bool _collectInput = true;
        [SerializeField] float _pivotDst = 10;
        [SerializeField] Vector2 _pivotDstMinMax = new Vector2(1, 100);
        [SerializeField] float _sensitivity;
        [SerializeField] ControlScheme _controlScheme;
        [SerializeField] Camera _cam;

        int _dpi;
        Transform _camTransform;
        List<IInputProvider> _inputs;
        Transform _stickTarget;

        public Camera Camera => _cam;

        public float PivotDistance
        {
            get => _pivotDst;
            set => _pivotDst = value;
        }

        public Transform StickTarget => _stickTarget;

        public bool CollectInput
        {
            get => _collectInput;
            set
            {
                if (_collectInput != value)
                {
                    ResetInput();
                    _collectInput = value;
                }
            }
        }

        void Awake()
        {
            _dpi = (int)Screen.dpi;
            if (_dpi <= 0)
                _dpi = 400;

            _camTransform = _cam.transform;
            _inputs = new List<IInputProvider>() { new TouchInput(), new MouseInput() };
        }

        void OnEnable()
        {
            ResetInput();
            // clear inertia
        }

        void LateUpdate()
        {
            if (_stickTarget != null)
            {
                _camTransform.SetPositionAndRotation(_stickTarget.position, _stickTarget.rotation);
                return;
            }

            if (_collectInput && TryGetInput(out var input))
            {
                input.dragDelta /= _dpi;
                input.altDragDelta /= _dpi;
                ApplyDrag(input.dragDelta, _controlScheme.dragAction);
                ApplyDrag(input.altDragDelta, _controlScheme.altDragAction);
                ApplyZoom(input.zoomDelta, input.zoomCenter, _controlScheme.zoomAction);
            }
        }

        public void StickTo(Transform target) => _stickTarget = target;

        public void Unstick() => _stickTarget = null;

        public POV GetCurrentPOV() => new POV()
        {
            Position = _camTransform.position,
            Rotation = _camTransform.eulerAngles,
            PivotDst = _pivotDst,
            Fov = _cam.fieldOfView,
            OrthoSize = _cam.orthographicSize,
        };

        public void ApplyPOV(POV pov)
        {
            _camTransform.position = pov.Position;
            _camTransform.eulerAngles = pov.Rotation;
            _pivotDst = pov.PivotDst;
            _cam.fieldOfView = pov.Fov;
            _cam.orthographicSize = pov.OrthoSize;
        }

        bool TryGetInput(out InputData inputData)
        {
            foreach (var input in _inputs)
                if (input.TryGetInput(out inputData))
                    return true;

            inputData = new InputData();
            return false;
        }

        void ApplyDrag(Vector2 delta, DragAction action)
        {
            switch (action)
            {
                case DragAction.RotateSelf:
                    delta *= _sensitivity;
                    _camTransform.Rotate(delta.y, 0, 0, Space.Self);
                    _camTransform.Rotate(0, -delta.x, 0, Space.World);
                    break;
                case DragAction.RotateAroundPivot:
                    delta *= _sensitivity;
                    var pivot = _camTransform.position + _camTransform.forward * _pivotDst;
                    _camTransform.RotateAround(pivot, Vector3.up, delta.x);
                    _camTransform.RotateAround(pivot, _camTransform.right, -delta.y);

                    var up = _camTransform.up;
                    if (up.y < 0)
                    {
                        var angle = Vector3.Angle(up, Vector3.up) - 90;
                        _camTransform.RotateAround(pivot, _camTransform.right, angle * Mathf.Sign(_camTransform.forward.y));
                    }
                    break;
                case DragAction.MoveXY:
                    var plane = new Plane(_camTransform.forward, _camTransform.position + _camTransform.forward * _pivotDst);
                    var ray1 = _cam.ScreenPointToRay(Vector3.zero);
                    var ray2 = _cam.ScreenPointToRay(delta * _dpi);
                    plane.Raycast(ray1, out float dst1);
                    plane.Raycast(ray2, out float dst2);
                    _camTransform.position += ray1.GetPoint(dst1) - ray2.GetPoint(dst2);
                    break;
            }
        }

        void ApplyZoom(float percent, Vector2 center, ZoomAction action)
        {
            percent += 1f;
            switch (action)
            {
                case ZoomAction.Fov:
                    _cam.fieldOfView /= percent;
                    break;
                case ZoomAction.OrthoSize:
                    _cam.orthographicSize /= percent;
                    break;
                case ZoomAction.MoveToPivot:
                    var oldDst = _pivotDst;
                    _pivotDst /= percent;
                    _camTransform.position += _camTransform.forward * (oldDst - _pivotDst);
                    _pivotDst = Mathf.Clamp(_pivotDst, _pivotDstMinMax.x, _pivotDstMinMax.y);
                    break;
                case ZoomAction.MoveZ:
                    _camTransform.position += _camTransform.forward * ((percent - 1f) * _sensitivity);
                    break;
            }

            if (_controlScheme.zoomTowardsPointer)
            {
                var halfScreenSize = new Vector2(Screen.width / 2f, Screen.height / 2f);
                ApplyDrag((percent - 1f) * (halfScreenSize - center) / _dpi, DragAction.MoveXY);
            }
        }

        void ResetInput()
        {
            foreach (var input in _inputs)
                input.Reset();
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        void OnDrawGizmosSelected()
        {
            if (_cam == null) return;
            var pivotPos = _cam.transform.position + _cam.transform.forward * _pivotDst;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_cam.transform.position, pivotPos);
            Gizmos.DrawWireSphere(pivotPos, .5f);
        }
    }
}
