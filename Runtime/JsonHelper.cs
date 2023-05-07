using System;
using UnityEngine;

namespace Ostium11
{
    public static class JsonHelper
    {
        [Serializable] class JsonArray<T> { public T[] arr; }

        public static T[] ArrayFromJson<T>(string json) => JsonUtility.FromJson<JsonArray<T>>(string.Concat("{\"arr\":", json, "}")).arr;

        public static string ArrayToJson<T>(T[] arr) => JsonUtility.ToJson(new JsonArray<T>() { arr = arr })[7..^1];
    }
}