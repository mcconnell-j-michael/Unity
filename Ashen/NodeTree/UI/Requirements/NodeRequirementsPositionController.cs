using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ashen.NodeTreeSystem
{
    [ExecuteAlways]
    public class NodeRequirementsPositionController : MonoBehaviour
    {
        public Transform reference;

        public List<I_NodeRequirements> nodeRequirements;
        public List<NodeUI> requiresReferenceList;
        public NodeUI requiresReferenceFocus;
        public NodeUI source;

        public RectTransformSide requiresBound;
        public RectTransformSide sourceBound;

        private RectTransform requiresRect;
        private RectTransform sourceRect;
        private RectTransform rectTransform;

        [Range(0, 100)]
        public int locationX;
        [Range(0, 100)]
        public int locationY;

        public bool hide;

        public TextMeshProUGUI text;

        [HideInInspector]
        public bool disable;

        private Vector3 cachedStart;
        private Vector3 cachedEnd;
        private int cachedLocationX;
        private int cachedLocationY;

        // Use this for initialization
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(List<I_NodeRequirements> nodeRequirements, List<NodeUI> requiresReferenceList, NodeUI requiresReferenceFocus, NodeUI source, int levelRequired)
        {
            this.nodeRequirements = nodeRequirements;
            this.requiresReferenceList = requiresReferenceList;
            this.requiresReferenceFocus = requiresReferenceFocus;
            this.source = source;
            text.text = "LV " + levelRequired;
        }

        // Update is called once per frame
        void Update()
        {
            if (hide || disable || requiresReferenceFocus == null || source == null || requiresReferenceList == null)
            {
                text.enabled = false;
            }
            else
            {
                if (requiresRect == null)
                {
                    requiresRect = requiresReferenceFocus.GetComponent<RectTransform>();
                }
                if (sourceRect == null)
                {
                    sourceRect = source.GetComponent<RectTransform>();
                }
                text.enabled = true;
                Vector3 startPosition = this.GetPosition(requiresRect, requiresBound);
                Vector3 endingPosition = this.GetPosition(sourceRect, sourceBound);
                if (cachedStart == startPosition && cachedEnd == endingPosition && cachedLocationX == locationX && cachedLocationY == locationY)
                {
                    return;
                }
                cachedStart = startPosition;
                cachedEnd = endingPosition;
                cachedLocationX = locationX;
                cachedLocationY = locationY;

                float percentageX = locationX / 100f;
                float percentageY = locationY / 100f;

                float xDistance = (endingPosition.x - startPosition.x) * percentageX;
                float yDistance = (endingPosition.y - startPosition.y) * percentageY;

                Vector3 newPosition = new Vector3(startPosition.x + xDistance, startPosition.y + yDistance, startPosition.z);

                rectTransform.anchoredPosition = newPosition;
            }
        }

        private Vector3 GetPosition(RectTransform transform, RectTransformSide side)
        {
            Vector3 position = reference.transform.InverseTransformPoint(transform.position);
            switch (side)
            {
                case RectTransformSide.TOP:
                    position.y += ((transform.rect.height / 2f) + (rectTransform.localScale.y * rectTransform.rect.height / 2f));
                    break;
                case RectTransformSide.BOTTOM:
                    position.y -= ((transform.rect.height / 2f) + (rectTransform.localScale.y * rectTransform.rect.height / 2f));
                    break;
                case RectTransformSide.RIGHT:
                    position.x += ((transform.rect.width / 2f) + (rectTransform.localScale.x * rectTransform.rect.width / 2f));
                    break;
                case RectTransformSide.LEFT:
                    position.x -= ((transform.rect.width / 2f) + (rectTransform.localScale.x * rectTransform.rect.width / 2f));
                    break;
            }
            return position;
        }
    }
}