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

            Target.Locked += (_, _) => OnPanelLocked();
            Target.Unlocked += (_, _) => OnPanelUnlocked();

            OnInitialize();
        }

        public void Dispose()
        {
            Target.Enabled -= (_, _) => OnPanelEnabled();
            Target.Disabled -= (_, _) => OnPanelDisabled();

            Target.Locked -= (_, _) => OnPanelLocked();
            Target.Unlocked -= (_, _) => OnPanelUnlocked();

            OnDispose();
        }

        #endregion

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }

        protected virtual void OnPanelEnabled() { }
        protected virtual void OnPanelDisabled() { }

        protected virtual void OnPanelLocked() { }
        protected virtual void OnPanelUnlocked() { }
    }
}