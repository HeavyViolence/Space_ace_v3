using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceAce.UI
{
    [CreateAssetMenu(fileName = "Level selection menu config",
                     menuName = "Space ace/Configs/UI/Level selection menu config")]
    public sealed class LevelSelectionMenuConfig : ScriptableObject
    {
        [SerializeField]
        private VisualTreeAsset _levelButton;

        [SerializeField]
        private VisualTreeAsset _itemPanel;

        public TemplateContainer LevelButton => _levelButton.CloneTree();
        public TemplateContainer ItemPanel => _itemPanel.CloneTree();
    }
}