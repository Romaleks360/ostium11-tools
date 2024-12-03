using Ostium11.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class ScrollRectMouseSupport : ScrollRect
    {
        private const float MOUSESCROLL_SPEED = 15;
        private const float MOUSESCROLL_DISTANCE = 300;

        private Vector2 _bbox;
        private Vector2 _targetScrollPos;
        private Coroutine _scrollRoutine;
        private bool _canScroll = true;

        private Vector2 TargetScrollPos
        {
            get => _targetScrollPos;
            set => _targetScrollPos = vertical ? value.SetY(Mathf.Clamp(value.y, 0, _bbox.y)) : value.SetX(Mathf.Clamp(value.x, _bbox.x, 0));

        }

#if UNITY_EDITOR
        protected override void OnValidate() => scrollSensitivity = 0;
#endif

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);

            if (!_canScroll || data.scrollDelta.y == 0) return;

            if (!ValidateViewport()) return;

            UpdateBBOX();

            velocity = Vector2.zero;
            var dir = vertical ? Vector2.up : Vector2.right;

            if (_scrollRoutine == null ||
                Math.Abs(Mathf.Sign(data.scrollDelta.y) -
                (vertical ? Mathf.Sign(content.anchoredPosition.y - TargetScrollPos.y) : Mathf.Sign(content.anchoredPosition.x - TargetScrollPos.x))) > float.Epsilon)
                TargetScrollPos = content.anchoredPosition + -data.scrollDelta.y * MOUSESCROLL_DISTANCE * dir;
            else
                TargetScrollPos += -data.scrollDelta.y * MOUSESCROLL_DISTANCE * dir;

            ForceStartScroll();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            StopMouseScroll(false);
            _canScroll = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            _canScroll = true;
        }

        /// <param name="value">[0,1] percentage from start to end of scroll</param>
        public void ScrollTo(float value)
        {
            if (!ValidateViewport()) return;

            UpdateBBOX();
            TargetScrollPos = vertical ? content.anchoredPosition.SetY(_bbox.y * value) : content.anchoredPosition.SetX(_bbox.x * value);
            ForceStartScroll();
        }

        private bool ValidateViewport()
        {
            if ((vertical && content.rect.height <= viewRect.rect.height) ||
                (horizontal && content.rect.width <= viewRect.rect.width))
            {
                StopMouseScroll(true);
                return false;
            }
            return true;
        }

        private void UpdateBBOX() => _bbox = new Vector2(Mathf.Min(0, viewport.rect.width - content.rect.width), Mathf.Max(0, content.rect.height - viewport.rect.height));

        private void ForceStartScroll() => _scrollRoutine ??= StartCoroutine(DoScroll());

        private IEnumerator DoScroll()
        {
            while ((content.anchoredPosition - _targetScrollPos).sqrMagnitude >= 12.25f)
            {
                ValidateViewport();
                content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, _targetScrollPos, Time.deltaTime * MOUSESCROLL_SPEED);
                yield return null;
            }

            content.anchoredPosition = _targetScrollPos;
            _scrollRoutine = null;
        }

        private void StopMouseScroll(bool reset)
        {
            if (_scrollRoutine == null)
                return;

            StopCoroutine(_scrollRoutine);
            _scrollRoutine = null;

            if (reset)
                content.anchoredPosition = _targetScrollPos = vertical ? content.anchoredPosition.SetY(0) : content.anchoredPosition.SetX(0);
        }
    }
}