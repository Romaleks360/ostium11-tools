using System.Threading;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        public static UniTask Delay(float secondsDelay, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
            => Delay((int)(secondsDelay * 1000f), delayType, delayTiming, cancellationToken);

        public static UniTask Delay(float secondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
            => Delay((int)(secondsDelay * 1000f), ignoreTimeScale, delayTiming, cancellationToken);
    }
}