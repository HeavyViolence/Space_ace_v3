using UnityEngine;

namespace SpaceAce.GUI
{
    public abstract class GUIPanelCache : GUICache
    {
        [SerializeField]
        private Canvas _canvas;

        public Canvas Canvas => _canvas;
    }
}