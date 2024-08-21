using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class InfusionUISelector : MonoBehaviour, I_EnumCacheable
    {
        [SerializeField]
        private GameObject canBeSelectedGlow;
        [SerializeField]
        private GameObject canNotBeSelectedGlow;

        [SerializeField]
        private TextMeshProUGUI count;
        [SerializeField]
        private Color[] colorPerInfusionLevel;

        [SerializeField]
        private CombatInfusion infusion;
        public CombatInfusion Infusion { get { return infusion; } }
        [SerializeField]
        private AbilitySO infusionEffect;
        public AbilitySO InfusionEffect { get { return infusionEffect; } }
        [SerializeField]
        private AbilitySO diffusionEffect;
        public AbilitySO DiffusionEffect { get { return diffusionEffect; } }

        [SerializeField]
        private CombatInfusion onLeft;
        public CombatInfusion OnLeft { get { return onLeft; } }
        [SerializeField]
        private CombatInfusion onRight;
        public CombatInfusion OnRight { get { return onRight; } }
        [SerializeField]
        private CombatInfusion onUp;
        public CombatInfusion OnUp { get { return onUp; } }
        [SerializeField]
        private CombatInfusion onDown;
        public CombatInfusion OnDown { get { return onDown; } }

        private bool canBeSubmitted;
        public bool CanBeSubmitted { get { return canBeSubmitted; } }

        private ToolManager toolManager;



        public void StartSelection(bool canBeSelected)
        {
            canBeSubmitted = canBeSelected;
        }

        public void StopSelection()
        {
            canBeSelectedGlow.SetActive(false);
            canNotBeSelectedGlow.SetActive(false);
        }

        public void Select()
        {
            canBeSelectedGlow.SetActive(canBeSubmitted);
            canNotBeSelectedGlow.SetActive(!canBeSubmitted);
        }

        public void Deselect()
        {
            canBeSelectedGlow.SetActive(false);
            canNotBeSelectedGlow.SetActive(false);
        }

        public void Register(ToolManager toolManager)
        {
            UnRegister();
            this.toolManager = toolManager;
            AttributeTool aTool = toolManager.Get<AttributeTool>();
            aTool.Cache(infusion.InfusionLevel, this);
            int level = (int)aTool.GetAttribute(infusion.InfusionLevel);
            SetInfusionLevel(level);
        }

        public void UnRegister()
        {
            if (toolManager != null)
            {
                AttributeTool aTool = toolManager.Get<AttributeTool>();
                aTool.UnCache(infusion.InfusionLevel, this);
                toolManager = null;
                SetInfusionLevel(0);
            }
        }

        public void Recalculate(I_EnumSO enumSO, I_DeliveryTool deliveryTool)
        {
            if (enumSO is DerivedAttribute attribute)
            {
                AttributeTool aTool = toolManager.Get<AttributeTool>();
                int level = (int)aTool.GetAttribute(attribute);
                SetInfusionLevel(level);
            }
        }

        private void SetInfusionLevel(int level)
        {
            count.text = level.ToString();
            count.color = colorPerInfusionLevel[Mathf.Min(colorPerInfusionLevel.Count() - 1, level)];
        }
    }
}
