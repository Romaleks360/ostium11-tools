using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ostium11.UI
{
    public class CustomInputModule : StandaloneInputModule
    {
        [Space]
        [Tooltip("If pointer is over these objects IsPointerOverGameObject will still return false")]
        [SerializeField] List<GameObject> _ignoreObjects;

        public override bool IsPointerOverGameObject(int pointerId)
        {
            var enterObject = GetLastPointerEventData(pointerId)?.pointerEnter;
            return enterObject != null && !_ignoreObjects.Contains(enterObject);

            //return base.IsPointerOverGameObject(pointerId);
        }
    }
}