using Cysharp.Threading.Tasks;

using System;
using System.Threading;

namespace SpaceAce.Auxiliary
{
    public sealed class PausableCancellationTokenSource : CancellationTokenSource
    {
        public PausableCancellationTokenSource(float delay, Func<bool> pauseCondition)
        {
            if (delay < 0f)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            CancelAsync(delay, pauseCondition).Forget();
        }

        private async UniTask CancelAsync(float delay, Func<bool> pauseCondition)
        {
            await AuxAsync.DelayAsync(() => delay, pauseCondition, Token);

            if (Token.IsCancellationRequested == false)
            {
                Cancel();
            }
        }
    }
}