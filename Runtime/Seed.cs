using UnityEngine;

namespace Ostium11
{
    /// <summary>
    /// Sets UnityEngine.Random seed and restores it on Dispose.
    /// </summary>
    public struct Seed : System.IDisposable
    {
        readonly Random.State _prevState;

        public Seed(int seed)
        {
            _prevState = Random.state;
            Random.InitState(seed);
        }

        public void Dispose()
        {
            Random.state = _prevState;
        }
    }
}