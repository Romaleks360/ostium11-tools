using UnityEngine;

namespace Ostium11
{
    public class OpenURL : MonoBehaviour
    {
        [SerializeField] string _url;

        public void Open() => Application.OpenURL(_url);
    }
}
