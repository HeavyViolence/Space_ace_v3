using UnityEngine;

namespace SpaceAce.GUI
{
    public abstract class GUICache : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameObject;

        public GameObject GameObject => _gameObject;

        [SerializeField]
        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
    }
}