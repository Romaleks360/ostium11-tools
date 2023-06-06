using UnityEngine;

namespace Ostium11.Components
{
    public class OpenURL : MonoBehaviour
    {
        [SerializeField] string _url;

        public void Open() => Application.OpenURL(_url);
    }
}
