using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Ostium11.UI
{
    public class UIFastButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
		public event Action OnClick;

		public void OnPointerDown(PointerEventData eventData) => OnClick?.Invoke();

		// Have to implement this for Unity to stop passing the Event Data onto underlying UI elements
		public void OnPointerClick(PointerEventData eventData) { }
	}
}
