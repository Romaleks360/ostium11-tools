using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ostium11.UI
{
    public class GestureHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] float _tapTime = 0.2f;
        [SerializeField] float _holdTime = 0.3f;

        readonly float _tapMoveThresholdSqr = 10;

        PointerEventData _holdPointer;

        public event Action<Vector2> Tap;
        public event Action<Vector2> Hold;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> EndDrag;

        protected virtual void Update()
        {
            if (_holdPointer == null)
                return;

            if ((_holdPointer.position - _holdPointer.pressPosition).sqrMagnitude > _tapMoveThresholdSqr)
            {
                _holdPointer = null;
                return;
            }

            if (Time.unscaledTime - _holdPointer.clickTime < _holdTime)
                return;

            Hold?.Invoke(_holdPointer.position);
            _holdPointer = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
            _holdPointer ??= eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(eventData);
            if (_holdPointer?.pointerId == eventData.pointerId)
                _holdPointer = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if ((eventData.position - eventData.pressPosition).sqrMagnitude > _tapMoveThresholdSqr)
                return;

            if (Time.unscaledTime - eventData.clickTime > _tapTime)
                return;

            Tap?.Invoke(eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData) => BeginDrag?.Invoke(eventData);

        public void OnDrag(PointerEventData eventData) => Drag?.Invoke(eventData);

        public void OnEndDrag(PointerEventData eventData) => EndDrag?.Invoke(eventData);
    }

}