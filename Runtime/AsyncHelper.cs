using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ostium11
{
    public static class AsyncHelper
    {
        public static UniTask Repeat(Action action, float intervalSeconds, CancellationToken token)
            => UniTask.Create(async () =>
            {
                do
                {
                    await UniTask.Delay(intervalSeconds);
                    if (token.IsCancellationRequested)
                        return;
                    action.Invoke();
                } while (true);
			});
    }
}