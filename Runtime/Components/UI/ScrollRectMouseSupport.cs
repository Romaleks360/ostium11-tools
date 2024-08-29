using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectMouseSupport : ScrollRect {
	private const float MOUSESCROLL_SPEED = 15;
	private const float MOUSESCROLL_DISTANCE = 300;

	private Vector2 _bbox;
	private Vector2 _targetScrollPos;
	private Coroutine _scrollRoutine;
	private bool _canScroll = true;

	private Vector2 TargetScrollPos {
		get => _targetScrollPos;
		set {
			_targetScrollPos = value;
			if (_targetScrollPos.y < _bbox.x) _targetScrollPos.y = _bbox.x;
			if (_targetScrollPos.y > _bbox.y) _targetScrollPos.y = _bbox.y;
		}
	}

#if UNITY_EDITOR
	protected override void OnValidate() {
		scrollSensitivity = 0;
	}
#endif

	public override void OnScroll(PointerEventData data) {
		base.OnScroll(data);

		if (!vertical) return;

		if (!_canScroll || data.scrollDelta.y == 0) return;

		float viewportH = viewRect.rect.height;
		float contentH = content.rect.height;

		if (contentH <= viewportH) {
			StopMouseScrollAndReset();
			return;
		}

		_bbox = new Vector2(0, contentH - viewportH);
		velocity = Vector2.zero;
		float sign = Mathf.Sign(data.scrollDelta.y);

		if (_scrollRoutine == null ||
			Math.Abs(sign - Mathf.Sign(content.anchoredPosition.y - TargetScrollPos.y)) > float.Epsilon)
			TargetScrollPos = content.anchoredPosition + Vector2.up * -data.scrollDelta.y * MOUSESCROLL_DISTANCE;
		else
			TargetScrollPos += Vector2.up * -data.scrollDelta.y * MOUSESCROLL_DISTANCE;

		ForceStartScroll();
	}

	public void ScrollToEnd() {
		ScrollToPosition(content.rect.height - viewRect.rect.height);
	}

	public void ScrollToStart() {
		ScrollToPosition(0);
	}

	private void ScrollToPosition(float ypos) {
		float viewportH = viewRect.rect.height;
		float contentH = content.rect.height;
		if (contentH <= viewportH) {
			StopMouseScrollAndReset();
			return;
		}

		_bbox = new Vector2(0, contentH - viewportH);

		TargetScrollPos = new Vector2(content.anchoredPosition.x, ypos);

		ForceStartScroll();
	}

	private void ForceStartScroll() {
		_scrollRoutine ??= StartCoroutine(DoScroll());
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		base.OnBeginDrag(eventData);
		StopMouseScroll();
		_canScroll = false;
	}

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);
		_canScroll = true;
	}

	private IEnumerator DoScroll() {
		while (Mathf.Abs(content.anchoredPosition.y - _targetScrollPos.y) >= 3.5f) {
			if (content.rect.height <= viewRect.rect.height)
				StopMouseScrollAndReset();

			content.anchoredPosition =
				Vector2.Lerp(content.anchoredPosition, _targetScrollPos, Time.deltaTime * MOUSESCROLL_SPEED);
			yield return null;
		}

		content.anchoredPosition = TargetScrollPos;

		_scrollRoutine = null;
	}

	private void StopMouseScroll() {
		if (_scrollRoutine == null) return;

		StopCoroutine(_scrollRoutine);
		_scrollRoutine = null;
	}

	private void StopMouseScrollAndReset() {
		if (_scrollRoutine == null) return;

		StopCoroutine(_scrollRoutine);
		_scrollRoutine = null;
		Vector2 anchoredPosition = content.anchoredPosition;
		anchoredPosition.y = 0;
		content.anchoredPosition = anchoredPosition;
		_targetScrollPos = anchoredPosition;
	}
}