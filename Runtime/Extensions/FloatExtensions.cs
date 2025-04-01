using Cysharp.Threading.Tasks;

namespace Ostium11.Extensions
{
    public static class FloatExtensions
    {
        public static UniTask.Awaiter GetAwaiter(this float seconds) => UniTask.Delay(seconds).GetAwaiter();
    }
}