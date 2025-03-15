using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class PointerDownButton : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (IsActive() && IsInteractable())
                onClick.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData) { }
    }
}
