using Cysharp.Threading.Tasks;

using SpaceAce.Main.Localization;

using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceAce.GUI
{
    public abstract class GUIPanel : MonoBehaviour
    {
        public event Action Enabled, Disabled;
        public event Action<PointerEventData> HoveredOverButton;

        [SerializeField]
        protected PanelCache Panel;

        public bool Active { get; private set; } = false;

        public async UniTask EnableAsync()
        {
            if (Active == true)
            {
                return;
            }

            OnBind();
            Panel.GameObject.SetActive(true);

            await TweenOnEnableAsync();

            Active = true;
            Enabled?.Invoke();
        }

        public async UniTask DisableAsync()
        {
            if (Active == false)
            {
                return;
            }

            OnClear();
            Panel.GameObject.SetActive(false);

            await TweenOnDisableAsync();

            Active = false;
            Disabled?.Invoke();
        }

        protected void OnHoveredOverButton(PointerEventData data)
        {
            HoveredOverButton?.Invoke(data);
        }

        public abstract UniTask LocalizeAsync(Localizer localizer);

        protected abstract UniTask TweenOnEnableAsync();
        protected abstract UniTask TweenOnDisableAsync();

        protected abstract void OnBind();
        protected abstract void OnClear();
    }
}