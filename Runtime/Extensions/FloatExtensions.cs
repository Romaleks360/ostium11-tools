using Cysharp.Threading.Tasks;

namespace Ostium11.Extensions
{
    public static class FloatExtensions
    {
#if OSTIUM11_UNITASK_SUPPORT
        public static UniTask.Awaiter GetAwaiter(this float seconds) => UniTask.Delay(seconds).GetAwaiter();
#endif
    }
}