using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class InteractableButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Element[] elements;
        
        public Button Button => button;

        [Serializable]
        public struct Element
        {
            public Image Image;
            public Color NormalColor;
            public Color DisabledColor;
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
            foreach (var element in elements)
                element.Image.color = interactable ? element.NormalColor : element.DisabledColor;
        }
    }
}
