using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Ostium11
{
    [System.Serializable]
    [JsonObject]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] List<Pair> _pairs = new();

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var pair in _pairs)
                if (!ContainsKey(pair.key))
                    Add(pair.key, pair.value);
        }

        public void OnBeforeSerialize()
        {
            if (!Application.isPlaying)
                return;

            ForceUpdateSerializedData();
        }

        public void ForceUpdateSerializedData()
        {
            _pairs.Clear();
            foreach (var (k, v) in this)
                _pairs.Add(new Pair(k, v));
        }

        [OnDeserialized] public void OnDeserialized(StreamingContext context) => OnAfterDeserialize();
        [OnSerializing] public void OnSerializing(StreamingContext context) => OnBeforeSerialize();

        [System.Serializable]
        struct Pair
        {
            public TKey key;
            public TValue value;

            public Pair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
