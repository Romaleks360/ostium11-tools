using UnityEngine;

namespace Ostium11.Components
{
    public class FPSCounter : MonoBehaviour
    {
        void OnGUI()
        {
            int fps = (int)(1f / Time.deltaTime);
            GUI.skin.label.fontSize = 48;
            GUI.Label(new Rect(50, 50, 500, 100), fps.ToString());
        }
    }
}