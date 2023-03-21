using System;
using UnityEngine;

namespace Ostium11
{
    /// <summary>
    /// Use this to serialize reference to an Object implementing the interface T
    /// </summary>
    /// <typeparam name="T"> Interface type </typeparam>
    [Serializable]
    public class Interface<T> where T : class
    {
        [SerializeField] UnityEngine.Object _container;

        public T Value => _container as T;

        public static implicit operator T(Interface<T> i) => i._container as T;
    }
}