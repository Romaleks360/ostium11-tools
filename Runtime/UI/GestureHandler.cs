using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ostium11.UI
{
    public enum InputEvent { Tap, Hold, PointerDown, PointerUp, BeginDrag, Drag, EndDrag }

    public class GestureHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] float _tapTime = 0.2f;
        [SerializeField] float _holdTime = 0.3f;

        PointerEventData _holdPointer;

        public event Action<PointerEventData> Tap;
        public event Action<PointerEventData> Hold;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> EndDrag;

        public event Action<InputEvent, PointerEventData> Input;

        protected virtual void Update()
        {
            if (_holdPointer == null)
                return;

            if ((_holdPointer.position - _holdPointer.pressPosition).sqrMagnitude > 1)
            {
                _holdPointer = null;
                return;
            }

            if (Time.unscaledTime - _holdPointer.clickTime < _holdTime)
                return;

            Hold?.Invoke(_holdPointer);
            Input?.Invoke(InputEvent.Hold, _holdPointer);
            _holdPointer = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
            Input?.Invoke(InputEvent.PointerDown, eventData);
            if (_holdPointer == null)
                _holdPointer = eventData;
            else
                _holdPointer = null;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(eventData);
            Input?.Invoke(InputEvent.PointerUp, eventData);
            _holdPointer = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if ((eventData.position - eventData.pressPosition).sqrMagnitude > 1)
                return;

            if (Time.unscaledTime - eventData.clickTime > _tapTime)
                return;

            Tap?.Invoke(eventData);
            Input?.Invoke(InputEvent.Tap, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData);
            Input?.Invoke(InputEvent.BeginDrag, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
            Input?.Invoke(InputEvent.Drag, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(eventData);
            Input?.Invoke(InputEvent.EndDrag, eventData);
        }

    }

}