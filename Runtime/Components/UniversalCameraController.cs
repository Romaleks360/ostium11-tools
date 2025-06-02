using System;
using System.Collections.Generic;
using Ostium11.Extensions;
using Ostium11.TimerTasks;
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
            public Vector3 moveDir;      // keyboard movement
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

        class MouseAndKeyboardInput : IInputProvider
        {
            bool _enabled = true;

            public void Reset() => _enabled = true;

            public bool TryGetInput(out InputData input)
            {
                if (!Input.mousePresent)
                {
                    input = new InputData();
                    return false;
                }

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
                    _enabled = !EventSystem.current.IsPointerOverGameObject();

                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2))
                    _enabled = true;

                if (!_enabled)
                {
                    input = new InputData();
                    return false;
                }

                var moveDir = Vector3.zero;

                // if (Input.GetMouseButton(0))
                if (true)
                {
                    if (Input.GetKey(KeyCode.W))
                        moveDir += Vector3.forward;
                    if (Input.GetKey(KeyCode.A))
                        moveDir += Vector3.left;
                    if (Input.GetKey(KeyCode.S))
                        moveDir += Vector3.back;
                    if (Input.GetKey(KeyCode.D))
                        moveDir += Vector3.right;
                    if (Input.GetKey(KeyCode.E))
                        moveDir += Vector3.up;
                    if (Input.GetKey(KeyCode.Q))
                        moveDir += Vector3.down;

                    Vector3.ClampMagnitude(moveDir, 1);

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        moveDir *= 2;
                }

                input = new InputData()
                {
                    dragDelta = Input.GetMouseButton(0) ? Input.mousePositionDelta : Vector2.zero,
                    altDragDelta = Input.GetMouseButton(2) ? Input.mousePositionDelta : Vector2.zero,
                    zoomCenter = Input.mousePosition,
                    zoomDelta = Mathf.Clamp(Input.mouseScrollDelta.y, -1f, 1f) / 10,
                    moveDir = moveDir,
                };

#if UNITY_WEBGL && !UNITY_EDITOR
                input.dragDelta /= 5;
                input.altDragDelta /= 5;
#endif
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
        [SerializeField] Vector2 _pivotDstMinMax = new(1, 100);
        [SerializeField] float _sensitivity;
        [SerializeField] float _moveSpeed = 25;
        [SerializeField] ControlScheme _controlScheme;
        [SerializeField] Camera _cam;

        int _dpi;
        Transform _camTransform;
        List<IInputProvider> _inputs;
        Transform _stickTarget;
        UpdateType _updateType = UpdateType.Late;

        public Camera Camera => _cam;
        public UpdateType UpdateType { get => _updateType; set => _updateType = value; }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public float PivotDistance
        {
            get => _pivotDst;
            set => _pivotDst = value;
        }

        public Transform StickTarget { get => _stickTarget; set => _stickTarget = value; }

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
            _inputs = new List<IInputProvider>() { new TouchInput(), new MouseAndKeyboardInput() };
        }

        void OnEnable()
        {
            ResetInput();
            // clear inertia
        }

        void Update()
        {
            if (_updateType == UpdateType.Normal)
                Tick();
        }

        void LateUpdate()
        {
            if (_updateType == UpdateType.Late)
                Tick();
        }

        private void FixedUpdate()
        {
            if (_updateType == UpdateType.Fixed)
                Tick();
        }

        public void Tick()
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
                ApplyMove(input.moveDir);
            }
        }

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

        void ApplyMove(Vector3 moveDir)
        {
            if (moveDir == Vector3.zero)
                return;

            _pivotDst = 0;
            _camTransform.position += _moveSpeed * Time.deltaTime * (_camTransform.rotation * moveDir);
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
