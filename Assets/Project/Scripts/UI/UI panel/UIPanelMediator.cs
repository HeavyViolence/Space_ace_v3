using Cysharp.Threading.Tasks;

using SpaceAce.Main.Audio;

using System;

using Zenject;

namespace SpaceAce.UI
{
    public abstract class UIPanelMediator : IInitializable, IDisposable
    {
        protected readonly UIPanels Panels;
        protected readonly UIServices Services;

        protected abstract UIPanel Target { get; }

        public UIPanelMediator(UIPanels panels, UIServices services)
        {
            Panels = panels ?? throw new ArgumentNullException();
            Services = services ?? throw new ArgumentNullException();
        }

        protected abstract void OnInitialize();
        protected abstract void OnDispose();

        protected abstract void OnPanelEnabled();
        protected abstract void OnPanelDisabled();

        protected abstract void OnPanelLock();
        protected abstract void OnPanelUnlock();

        protected void OnButtonClicked()
        {
            AudioProperties audio = Services.UIAudio.Click.Random;
            Services.AudioPlayer.PlayAsync(audio, null, UnityEngine.Vector3.zero, false, false).Forget();
        }

        protected void OnHoveredOver()
        {
            AudioProperties audio = Services.UIAudio.HoverOver.Random;
            Services.AudioPlayer.PlayAsync(audio, null, UnityEngine.Vector3.zero, false, false).Forget();
        }

        #region interfaces

        public void Initialize()
        {
            Target.Enabled += (_, _) => OnPanelEnabled();
            Target.Disabled += (_, _) => OnPanelDisabled();

            Target.Locked += (_, _) => OnPanelLock();
            Target.Unlocked += (_, _) => OnPanelUnlock();

            OnInitialize();
        }

        public void Dispose()
        {
            Target.Enabled -= (_, _) => OnPanelEnabled();
            Target.Disabled -= (_, _) => OnPanelDisabled();

            Target.Locked -= (_, _) => OnPanelLock();
            Target.Unlocked -= (_, _) => OnPanelUnlock();

            OnDispose();
        }

        #endregion
    }
}