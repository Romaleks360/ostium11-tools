using System.Threading;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        public static UniTask Delay(float seconds) => Delay((int)(seconds * 1000f));

        public static UniTask Delay(float seconds, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
            => Delay((int)(seconds * 1000f), delayType, delayTiming, cancellationToken);

        public static UniTask Delay(float seconds, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
            => Delay((int)(seconds * 1000f), ignoreTimeScale, delayTiming, cancellationToken);
    }
}
