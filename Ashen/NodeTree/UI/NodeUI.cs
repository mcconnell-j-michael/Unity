using Ashen.SkillTreeSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ashen.NodeTreeSystem
{
    public class NodeUI : MonoBehaviour
    {
        public NodeTreeUIManager nodeTreeUI;
        public Node node;

        [FoldoutGroup("Elements")] public Image background;
        [FoldoutGroup("Elements")] public Image textBackground;
        [FoldoutGroup("Elements")] public Image currentBackground;
        [FoldoutGroup("Elements")] public Image totalImage;
        [FoldoutGroup("Elements")] public TextMeshProUGUI nodeNameText;
        [FoldoutGroup("Elements")] public TextMeshProUGUI currentValueText;
        [FoldoutGroup("Elements")] public TextMeshProUGUI maxValueText;
        [FoldoutGroup("Elements"), SerializeField] private TierLevelsManager tierLevelManager;

        public UnityEvent onFirstNodePoint;
        public UnityEvent onResetToZero;

        public bool valid = false;
        public bool selected = false;

        public int column;

        public NodeUI Up { get; set; }
        public NodeUI Down { get; set; }
        public NodeUI Left { get; set; }
        public NodeUI Right { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                nodeTreeUI.OnClickNode(this, node);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                nodeTreeUI.OnClickRight(this, node);
            }

        }

        public void Selected()
        {
            selected = true;
            background.color = nodeTreeUI.selectedNodeColor;
            if (valid)
            {
                OnValidOption();
            }
            else
            {
                OnDisabledNode();
            }
            nodeTreeUI.UpdateScreenPosition(column);
        }

        public void Deselected()
        {
            selected = false;
            if (valid)
            {
                OnValidOption();
            }
            else
            {
                OnDisabledNode();
            }
        }

        public void OnDisabledNode()
        {
            valid = false;
            if (selected)
            {
                background.color = nodeTreeUI.selectedNodeColor;
            }
            else
            {
                background.color = nodeTreeUI.invalidNode.one;
            }
            textBackground.color = nodeTreeUI.invalidNode.two;
            currentBackground.color = nodeTreeUI.invalidNode.one;
            totalImage.color = nodeTreeUI.invalidNode.three;
            nodeNameText.color = nodeTreeUI.invalidNode.one;
            currentValueText.color = nodeTreeUI.invalidNode.two;
            maxValueText.color = nodeTreeUI.invalidNode.one;
        }

        public void OnResetToZero()
        {
            if (onResetToZero != null)
            {
                onResetToZero.Invoke();
            }
        }

        public void OnFirstNodePoint()
        {
            if (onFirstNodePoint != null)
            {
                onFirstNodePoint.Invoke();
            }
        }

        public void OnMaxNode()
        {
            OnValidOption();
        }

        public void OnValidOption()
        {
            valid = true;
            if (selected)
            {
                background.color = nodeTreeUI.selectedNodeColor;
            }
            else
            {
                background.color = nodeTreeUI.validNode.one;
            }
            textBackground.color = nodeTreeUI.validNode.two;
            currentBackground.color = nodeTreeUI.validNode.one;
            totalImage.color = nodeTreeUI.validNode.three;
            nodeNameText.color = nodeTreeUI.validNode.one;
            currentValueText.color = nodeTreeUI.validNode.two;
            maxValueText.color = nodeTreeUI.validNode.one;
        }

        public void OnMissingPoints()
        {
            OnValidOption();
        }

        public void RegisterNode(Node node)
        {
            this.node = node;
            nodeNameText.text = node.displayName;
            maxValueText.text = node.maxRanks.ToString();
            currentValueText.text = "0";
        }

        public void SetTierLevel(int tierLevel)
        {
            tierLevelManager.SetTier(tierLevel);
        }
    }
}