using UnityEngine;

namespace SpaceAce.GUI
{
    public sealed class PanelCache : GUICache
    {
        [SerializeField]
        private Canvas _canvas;

        public Canvas Canvas => _canvas;
    }
}