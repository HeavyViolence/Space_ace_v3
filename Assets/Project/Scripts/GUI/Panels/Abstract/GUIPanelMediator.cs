using Cysharp.Threading.Tasks;

using SpaceAce.Main;
using SpaceAce.Main.Audio;

using System;

using UnityEngine;
using UnityEngine.EventSystems;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.GUI
{
    public abstract class GUIPanelMediator : IInitializable, IDisposable
    {
        protected readonly GUIPanel Target;
        protected readonly GUIPanels Panels;
        protected readonly MainServices Services;

        [Inject]
        public GUIPanelMediator(GUIPanel target, GUIPanels panels, MainServices services)
        {
            Target = target == null ? throw new ArgumentNullException() : target;
            Panels = panels ?? throw new ArgumentNullException();
            Services = services ?? throw new ArgumentNullException();
        }

        protected void OnButtonClicked()
        {
            AudioProperties audio = Services.GUIAudio.Click.Random;
            Services.AudioPlayer.PlayAsync(audio, null, Vector3.zero, false, false).Forget();
        }

        protected void OnHoveredOverButton(PointerEventData data)
        {
            if (Application.isMobilePlatform == false)
            {
                AudioProperties audio = Services.GUIAudio.HoverOver.Random;
                Services.AudioPlayer.PlayAsync(audio, null, Vector3.zero, false, false).Forget();
            }
        }

        #region interfaces

        public void Initialize()
        {
            Target.Enabled += OnPanelEnabled;
            Target.Disabled += OnPanelDisabled;

            OnInitialize();
        }

        public void Dispose()
        {
            Target.Enabled -= OnPanelEnabled;
            Target.Disabled -= OnPanelDisabled;

            OnDispose();
        }

        #endregion

        protected async UniTask EnablePanelAsync()
        {
            if (Target.Active == false)
            {
                await Target.LocalizeAsync(Services.Localizer);
                await Target.EnableAsync();
            }
        }

        protected async UniTask DisablePanelAsync()
        {
            if (Target.Active == true)
            {
                await Target.DisableAsync();
            }
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }

        protected virtual void OnPanelEnabled() { }
        protected virtual void OnPanelDisabled() { }

        protected virtual void OnPanelLocked() { }
        protected virtual void OnPanelUnlocked() { }
    }
}