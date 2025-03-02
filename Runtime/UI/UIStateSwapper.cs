using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ostium11.UI
{
    public class UIStateSwapper : MonoBehaviour
    {
        [SerializeField] SerializedDictionary<Image, State[]> _states;

        public void SetState(int index)
        {
            foreach (var (image, states) in _states)
            {
                image.color = states[index].color;
                image.sprite = states[index].sprite;
            }
        }

        [Serializable]
        struct State
        {
            public Color color;
            public Sprite sprite;
        }
    }
}
