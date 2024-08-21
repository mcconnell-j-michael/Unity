using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class ActionPointUIManager : MonoBehaviour, I_ActionPointChangeListener
    {
        [SerializeField]
        private RectTransform containerTransform;
        [SerializeField]
        private GameObject actionPointHolder;
        [SerializeField]
        private ActionPoint actionPointPrefab;
        [SerializeField]
        private RectTransform background;
        [SerializeField]
        private RectTransform actionPointRect;
        [SerializeField]
        private GameObject actionTextBackground;
        [SerializeField]
        private TextMeshProUGUI actionText;

        [SerializeField]
        private int collapsedHeight;
        [SerializeField]
        private int lowHeight;
        [SerializeField]
        private int highHeight;
        [SerializeField]
        private int negativeDistance;
        [SerializeField]
        private int backgroundTop;

        [SerializeField]
        private int expandedHeight;
        [SerializeField]
        private int lowExpandedHeight;
        [SerializeField]
        private int highExpandedHeight;
        [SerializeField]
        private int expandedBackgroundTop;

        private List<ActionPoint> actionPoints;
        private ToolManager toolManager;
        private RectTransform actionPointHolderTransform;

        private float yPosDefault;
        private float xPosDefault;

        private float midPos;
        private float widthOfActionPoint;

        private bool selected;
        private bool expanded;

        private void Awake()
        {
            actionPoints = new();
            actionPointHolderTransform = actionPointHolder.transform as RectTransform;
            RectTransform actionPointRect = actionPointPrefab.GetComponent<RectTransform>();
            yPosDefault = -(actionPointRect.rect.height / 2);
            xPosDefault = actionPointRect.rect.width / 2;
            midPos = actionPointHolderTransform.rect.width / 2;
            widthOfActionPoint = actionPointRect.rect.width;
            selected = false;
            expanded = false;
        }

        public void RegisterToolManager(ToolManager toolManager)
        {
            if (this.toolManager)
            {
                UnRegisterToolManager();
            }
            if (!toolManager)
            {
                return;
            }
            this.toolManager = toolManager;
            CombatTool ct = toolManager.Get<CombatTool>();
            ct.AddListener(this);
        }

        public void UnRegisterToolManager()
        {
            if (!toolManager)
            {
                return;
            }
            CombatTool ct = toolManager.Get<CombatTool>();
            ct.RemoveListener(this);
            toolManager = null;
        }

        [Button]
        public void Selected()
        {
            containerTransform.anchoredPosition = new Vector2(containerTransform.anchoredPosition.x, expanded ? highExpandedHeight : highHeight);
            selected = true;
        }

        [Button]
        public void Expand(string text)
        {
            actionTextBackground.SetActive(true);
            background.SetTop(expandedBackgroundTop);
            actionPointRect.sizeDelta = new Vector2(actionPointRect.sizeDelta.x, expandedHeight);
            expanded = true;
            if (selected)
            {
                Selected();
            }
            else
            {
                Deselected();
            }
            actionText.text = text;
        }

        [Button]
        public void Collapse()
        {
            actionTextBackground.SetActive(false);
            background.SetTop(backgroundTop);
            actionPointRect.sizeDelta = new Vector2(actionPointRect.sizeDelta.x, collapsedHeight);
            expanded = false;
            if (selected)
            {
                Selected();
            }
            else
            {
                Deselected();
            }
        }

        [Button]
        public void Deselected()
        {
            containerTransform.anchoredPosition = new Vector2(containerTransform.anchoredPosition.x, expanded ? lowExpandedHeight : lowHeight);
            selected = false;
        }

        [Button]
        private void FindAndSetPositions()
        {
            actionPoints = new();
            actionPointHolderTransform = actionPointHolder.transform as RectTransform;
            RectTransform actionPointRect = actionPointPrefab.GetComponent<RectTransform>();
            yPosDefault = -(actionPointRect.rect.height / 2);
            xPosDefault = actionPointRect.rect.width / 2;
            midPos = actionPointHolderTransform.rect.width / 2;
            widthOfActionPoint = actionPointRect.rect.width;

            actionPoints.AddRange(GetComponentsInChildren<ActionPoint>());

            SetPositions();
        }

        private void SetPositions()
        {
            bool even = actionPoints.Count % 2 == 0;

            int midpoint = actionPoints.Count / 2;
            int lowMid = even ? midpoint - 1 : midpoint;
            int highMid = even ? midpoint : midpoint;

            float visibleWidth = widthOfActionPoint - negativeDistance;
            //int count = 0;
            float baseOffset = even ? (visibleWidth / 2) : 0;
            for (int x = 0; x < actionPoints.Count; x++)
            {
                int cnt = lowMid - x;
                ActionPoint actionPoint = actionPoints[x];
                RectTransform rect = actionPoint.GetComponent<RectTransform>();
                float xPosOffset = baseOffset + (cnt * (visibleWidth));
                rect.anchoredPosition = new Vector2(midPos - xPosOffset, 0);
            }
        }

        public void OnActionPointChange(ActionPointsUpdateValue updateValues)
        {
            while (actionPoints.Count > updateValues.maxResourceValue)
            {
                ActionPoint actionPoint = actionPoints[0];
                Destroy(actionPoint.gameObject);
                actionPoints.RemoveAt(0);
            }
            while (actionPoints.Count < updateValues.maxResourceValue)
            {
                actionPoints.Add(Instantiate(actionPointPrefab, actionPointHolder.gameObject.transform));
            }

            SetPositions();

            int curIndex = 0;
            ActionPointUpdateValue selectedActionPoint = null;
            foreach (ActionPointUpdateValue updateValue in updateValues.actionPointUpdates)
            {
                for (int y = 0; y < updateValue.actionPointCost; y++)
                {
                    if (curIndex >= actionPoints.Count)
                    {
                        break;
                    }
                    actionPoints[curIndex].PromiseActionPoint();
                    actionPoints[curIndex].DeselectActionPoint();
                    if (updateValue.selected)
                    {
                        actionPoints[curIndex].SelectActionPoint();
                        selectedActionPoint = updateValue;
                    }
                    curIndex++;
                }
            }

            int previewTotal = -updateValues.preview;
            for (int x = 0; x < updateValues.currentResourceValue; x++, curIndex++)
            {
                if (curIndex >= actionPoints.Count)
                {
                    break;
                }
                if (previewTotal > 0 && updateValues.previewValid)
                {
                    actionPoints[curIndex].PreviewActionPoint();
                    previewTotal--;
                }
                else
                {
                    actionPoints[curIndex].EnableActionPoint();
                }
                actionPoints[curIndex].DeselectActionPoint();
            }

            if (selectedActionPoint != null)
            {
                Expand(selectedActionPoint.name);
            }
            else
            {
                Collapse();
            }
            for (int x = curIndex; x < actionPoints.Count; x++)
            {
                actionPoints[x].DisableActionPoint();
            }
        }
    }
}