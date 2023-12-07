using UnityEngine;

namespace Ostium11.Components
{
    public class SetAllChildrenActive : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(isActive);
        }
    }
}
