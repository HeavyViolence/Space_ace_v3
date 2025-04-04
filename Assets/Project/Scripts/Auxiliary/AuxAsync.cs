using Cysharp.Threading.Tasks;

using System;
using System.Threading;

using UnityEngine;

namespace SpaceAce.Auxiliary
{
    public static class AuxAsync
    {
        public static async UniTask DelayAsync(Func<float> delayProvider,
                                               Func<bool> pauseCondition,
                                               CancellationToken token = default)
        {
            if (delayProvider is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            float timer = 0f;
            float delay = delayProvider();

            while (timer < delay)
            {
                if (token.IsCancellationRequested == true)
                {
                    return;
                }

                timer += Time.deltaTime;

                await UniTask.WaitUntil(() => pauseCondition() == false);
                await UniTask.Yield();
            }
        }

        public static async UniTask DelayAsync(Func<bool> delayCondition,
                                               Func<bool> pauseCondition,
                                               CancellationToken token = default)
        {
            if (delayCondition is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            while (delayCondition() == true && token.IsCancellationRequested == false)
            {
                await UniTask.WaitUntil(() => pauseCondition() == false);
                await UniTask.Yield();
            }
        }

        public static async UniTask DelayThenDoAsync(Action action,
                                                     Func<float> delayProvider,
                                                     Func<bool> pauseCondition,
                                                     CancellationToken token = default)
        {
            if (action is null || delayProvider is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            await DelayAsync(delayProvider, pauseCondition, token);
            action();
        }

        public static async UniTask DelayThenDoAsync(Action action,
                                                     Func<bool> delayCondition,
                                                     Func<bool> pauseCondition,
                                                     CancellationToken token = default)
        {
            if (action is null || delayCondition is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            await DelayAsync(delayCondition, pauseCondition, token);
            action();
        }

        public static async UniTask DoThenDelayAsync(Action action,
                                                     Func<float> delayProvider,
                                                     Func<bool> pauseCondition,
                                                     CancellationToken token = default)
        {
            if (action is null || delayProvider is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            action();
            await DelayAsync(delayProvider, pauseCondition, token);
        }

        public static async UniTask DoThenDelayAsync(Action action,
                                                     Func<bool> delayCondition,
                                                     Func<bool> pauseCondition,
                                                     CancellationToken token = default)
        {
            if (action is null || delayCondition is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            action();
            await DelayAsync(delayCondition, pauseCondition, token);
        }

        public static async UniTask DoForeverAsync(Action action,
                                                   Func<float> firstDelayProvider,
                                                   Func<float> regularDelayProvider,
                                                   Func<bool> pauseCondition,
                                                   CancellationToken token = default)
        {
            if (action is null || firstDelayProvider is null ||
                regularDelayProvider is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            await DelayAsync(firstDelayProvider, pauseCondition, token);

            while (token.IsCancellationRequested == false)
            {
                await DoThenDelayAsync(action, regularDelayProvider, pauseCondition, token);
            }
        }

        public static async UniTask DoForeverAsync(Action action,
                                                   Func<bool> firstDelayCondition,
                                                   Func<bool> regularDelayCondition,
                                                   Func<bool> pauseCondition,
                                                   CancellationToken token = default)
        {
            if (action is null || firstDelayCondition is null ||
                regularDelayCondition is null || pauseCondition is null)
            {
                throw new ArgumentNullException();
            }

            await DelayAsync(firstDelayCondition, pauseCondition, token);

            while (token.IsCancellationRequested == false)
            {
                await DoThenDelayAsync(action, regularDelayCondition, pauseCondition, token);
            }
        }

        public static async UniTaskVoid WaitForNextFrameThenDoAsync(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException();
            }

            await UniTask.NextFrame();
            action();
        }
    }
}