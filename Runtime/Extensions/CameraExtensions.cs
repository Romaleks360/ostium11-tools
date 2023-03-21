using UnityEngine;

namespace Ostium11.Extensions
{
    public static class CameraExtensions
    {
        public static float orthographicHeight(this Camera cam) => cam.orthographicSize * 2;
        public static float orthographicWidth(this Camera cam) => cam.orthographicSize * 2 * cam.aspect;

        public static void SetOrthographicHeight(this Camera cam, float y) => cam.orthographicSize = y / 2;
        public static void SetOrthographicWidth(this Camera cam, float x) => cam.orthographicSize = x / cam.aspect / 2;
    }
}