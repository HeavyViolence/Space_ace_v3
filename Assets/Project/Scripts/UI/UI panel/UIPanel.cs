using Cysharp.Threading.Tasks;

using System;

using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceAce.UI
{
    public abstract class UIPanel
    {
        public event EventHandler Enabled, Disabled, Locked, Unlocked;

        private readonly VisualTreeAsset _panelAsset;

        protected readonly UIDocument Document;
        protected readonly UIServices Services;

        protected abstract string PanelName { get; }

        public bool Active { get; private set; } = false;
        public bool IsLocked { get; private set; } = false;

        public UIPanel(VisualTreeAsset asset, PanelSettings settings, UIServices services)
        {
            _panelAsset = asset == null ? throw new ArgumentNullException() : asset;
            Services = services ?? throw new ArgumentNullException();

            if (settings == null)
            {
                throw new ArgumentNullException();
            }

            GameObject panel = new(PanelName);

            Document = panel.AddComponent<UIDocument>();
            Document.panelSettings = settings;
        }

        public async UniTask EnableAsync()
        {
            if (Active == true)
            {
                return;
            }

            Document.visualTreeAsset = _panelAsset;
            Document.rootVisualElement.style.opacity = 0f;

            OnBind();
            await LocalizeAsync();

            Document.rootVisualElement.style.opacity = 1f;

            Active = true;
            Enabled?.Invoke(this, EventArgs.Empty);
        }

        public void Disable()
        {
            if (Active == false)
            {
                return;
            }

            OnClear();

            Active = false;
            Disabled?.Invoke(this, EventArgs.Empty);

            Document.visualTreeAsset = null;
        }

        public void Lock()
        {
            if (Active == false || (Active == true && IsLocked == true))
            {
                return;
            }

            Document.rootVisualElement.style.opacity = 0.5f;

            IsLocked = true;
            Locked?.Invoke(this, EventArgs.Empty);
        }

        public void Unlock()
        {
            if (Active == false || (Active == true && IsLocked == false))
            {
                return;
            }

            Document.rootVisualElement.style.opacity = 1f;

            IsLocked = false;
            Unlocked?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void OnBind();
        protected abstract void OnClear();

        protected abstract UniTask LocalizeAsync();
    }
}