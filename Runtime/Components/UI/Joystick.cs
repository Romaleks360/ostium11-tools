using System;
using Ostium11.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ostium11.UI
{
    // WIP
    public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] RectTransform _handle;
        [SerializeField] bool _x = true;
        [SerializeField] bool _y = true;

        public bool X { get => _x; set => _x = value; }
        public bool Y { get => _y; set => _y = value; }

        public event Action<Vector2> Drag;

        public void OnDrag(PointerEventData eventData)
        {
            var dir = eventData.position - (Vector2)transform.position;
            var halfSize = this.rectTransform().rect.size / 2;
            dir /= halfSize;
            dir.x = _x ? Mathf.Clamp(dir.x, -1, 1) : 0;
            dir.y = _y ? Mathf.Clamp(dir.y, -1, 1) : 0;
            _handle.localPosition = dir * halfSize;
            Drag?.Invoke(dir);
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData)
        {
            _handle.localPosition = Vector2.zero;
            Drag?.Invoke(Vector2.zero);
        }

#if UNITY_EDITOR

        [UnityEditor.MenuItem("GameObject/UI/Joystick", false, 10)]
        private static void CreateJoystick(UnityEditor.MenuCommand menuCommand)
        {
            var knob = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            var go = new GameObject("Joystick");
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            var joystick = go.AddComponent<Joystick>();
            go.AddComponent<Image>().sprite = knob;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            var handle = new GameObject("Handle");
            UnityEditor.GameObjectUtility.SetParentAndAlign(handle, go);
            handle.AddComponent<Image>().sprite = knob;
            joystick._handle = handle.GetComponent<RectTransform>();

            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }

#endif
    }
}