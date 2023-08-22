using System.Collections.Generic;
using UnityEngine;

namespace Ostium11
{
    [System.Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] List<Pair> _pairs;

        public void OnAfterDeserialize()
        {
            foreach (var pair in _pairs)
                if (!ContainsKey(pair.key))
                    Add(pair.key, pair.value);

            if (!HasMissingEnumKeys(out var keyType, out var enumValuesArray))
                return;
            Stack<int> idsToRemove = new();

            for (int i = 0; i < _pairs.Count; i++)
                if (!keyType.IsEnumDefined(_pairs[i].key))
                {
                    idsToRemove.Push(i);
                    Remove(_pairs[i].key);
                }

            while (idsToRemove.Count > 0)
                _pairs.RemoveAt(idsToRemove.Pop());
        }

        public void OnBeforeSerialize()
        {
            if (!HasMissingEnumKeys(out var keyType, out var enumValues))
                return;

            foreach (var enumValue in enumValues)
                if (_pairs.FindIndex(p => p.key.Equals((TKey)enumValue)) == -1)
                    _pairs.Add(new Pair((TKey)enumValue, default));
        }

        bool HasMissingEnumKeys(out System.Type keyType, out System.Array values)
        {
            values = null;

            keyType = typeof(TKey);
            if (!keyType.IsEnum)
                return false;

            values = keyType.GetEnumValues();
            bool hasMissingValues = true;

            if (values.Length == _pairs.Count)
            {
                for (int i = 0; i < _pairs.Count; i++)
                    if (!values.GetValue(i).Equals(_pairs[i].key))
                        goto Skip;

                hasMissingValues = false;
            }

            Skip:

            return hasMissingValues;
        }

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
