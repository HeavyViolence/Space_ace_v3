using System;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceAce.GUI
{
    public sealed class ButtonCache : GUICache, IPointerEnterHandler
    {
        public event Action<PointerEventData> HoveredOver;

        [SerializeField]
        private Button _button;

        public Button Button => _button;

        [SerializeField]
        private TextMeshProUGUI _textMesh;

        public TextMeshProUGUI TextMesh => _textMesh;

        public void OnPointerEnter(PointerEventData data)
        {
            if (Button.enabled == true)
            {
                HoveredOver?.Invoke(data);
            }
        }
    }
}