using UnityEngine;

namespace Ostium11.Components
{
    public class Log : MonoBehaviour
    {
        public void LogMessage(string message) => Debug.Log(message);
    }
}
