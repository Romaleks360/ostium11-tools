using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ostium11
{
    public class UIStateSwapper : MonoBehaviour
    {
        [SerializeField] SerializedDictionary<Image, State[]> _states;

        [Serializable]
        public struct State
        {
            public Color color;
            public Sprite sprite;
        }

        public void SetState(int index)
        {
            foreach (var (image, states) in _states)
            {
                image.color = states[index].color;
                image.sprite = states[index].sprite;
            }
        }
    }
}
