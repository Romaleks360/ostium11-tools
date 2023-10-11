using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class DoubleClickButton : Button, IPointerClickHandler
    {
        public UnityEvent onDoubleClick;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (eventData.clickCount == 2)
                onDoubleClick.Invoke();
        }
    }
}
