using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.NodeTreeSystem;
using Ashen.ObjectPoolSystem;
using Ashen.SkillTree;
using Ashen.SkillTreeSystem;
using Assets.Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class SkillTreeTool : A_ConfigurableTool<SkillTreeTool, SkillTreeToolConfiguration>, I_Saveable, I_NodeTreeManager
    {
        public int skillPoints;
        [NonSerialized]
        public SkillTreeDefinition skillTree;

        [ShowInInspector]
        private Dictionary<Node, int> skillNodeToLevel;
        [ShowInInspector]
        private Dictionary<Node, I_ExtendedEffect> currentEffects;
        private DeliveryTool deliveryTool;
        private ShiftableTierLevelTool shiftableTierLevelTool;

        private SubSkillTreeKey subclass;

        public void Initialize(SkillTreeToolConfiguration config, SubSkillTreeKey subclass = null)
        {
            Config = config;
            this.subclass = subclass;
            Initialize();
        }

        public override void ReadArguments(Dictionary<string, object> arguments)
        {
            if (arguments != null && arguments.TryGetValue("subclass", out object subclass))
            {
                this.subclass = subclass as SubSkillTreeKey;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            skillNodeToLevel = new Dictionary<Node, int>();
            currentEffects = new Dictionary<Node, I_ExtendedEffect>();
            skillTree = Config.SkillTree;
            foreach (Node node in skillTree.GetNodes(subclass))
            {
                skillNodeToLevel.Add(node, 0);
                currentEffects.Add(node, null);
            }
            skillPoints = Config.DefaultSkillPoints;
        }

        public void SetSubclass(SubSkillTreeKey subclass)
        {
            this.subclass = subclass;
            foreach (Node node in skillTree.GetNodes(subclass))
            {
                if (!skillNodeToLevel.ContainsKey(node))
                {
                    skillNodeToLevel.Add(node, 0);
                }
                if (!currentEffects.ContainsKey(node))
                {
                    currentEffects.Add(node, null);
                }
            }
        }

        private void Start()
        {
            deliveryTool = toolManager.Get<DeliveryTool>();
            shiftableTierLevelTool = toolManager.Get<ShiftableTierLevelTool>();
        }

        public bool HasSkillNode(Node node)
        {
            return skillNodeToLevel.ContainsKey(node);
        }

        public int GetSkillNodeLevel(Node skillNode)
        {
            if (skillNodeToLevel.TryGetValue(skillNode, out int level))
            {
                return level;
            }
            throw new Exception("Invalid skill node");
        }

        public bool CanIncreaseSkillNode(Node skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            int maxLevel = skillNode.maxRanks;

            if (level == maxLevel)
            {
                return false;
            }

            return RequirementsMet(skillNode);
        }

        public bool RequirementsMet(Node skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            if (skillNode.hasRequirements)
            {
                foreach (I_NodeRequirements requirements in skillNode.requirements)
                {
                    if (!requirements.RequirementsMet(this))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public NodeIncreaseRequestResponse RequestSkillNodeIncrease(Node skillNode)
        {
            int level = GetSkillNodeLevel(skillNode);

            int maxLevel = skillNode.maxRanks;
            if (level == maxLevel)
            {
                return NodeIncreaseRequestResponse.MAX_LEVEL;
            }
            if (!RequirementsMet(skillNode))
            {
                return NodeIncreaseRequestResponse.REQUIREMENNTS_NOT_MET;
            }
            if (skillPoints < 1)
            {
                return NodeIncreaseRequestResponse.MISSING_SKILL_POINTS;
            }
            if (currentEffects.TryGetValue(skillNode, out I_ExtendedEffect currentEffect))
            {
                currentEffect?.Disable(false);
            }
            ApplySkill(skillNode, level + 1);
            skillPoints--;
            return NodeIncreaseRequestResponse.SUCCESS;
        }

        private void ApplySkill(Node skillNode, int rank)
        {
            Skill skill = skillNode.nodeElement as Skill;
            if (skill.skillType == Skill.SkillType.Ability)
            {
                Ability ability = skill.GetAbilityForLevel(toolManager, rank);
                AbilityHolder ah = toolManager.Get<AbilityTool>().AbilityHolder;
                ah.GrantAbility(skillNode.displayName, ability);
            }
            else if (skill.skillType == Skill.SkillType.Passive)
            {
                PassiveContainer container = skill.GetPassiveForLevel(rank);
                SkillNodeEffectBuilder builder = container.builder;
                DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
                EffectsArgumentPack effectPack = packs.GetPack<EffectsArgumentPack>();
                foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                {
                    if (container.ScaleDeliveryPacks[(int)argument] != null)
                    {
                        effectPack.SetFloatArgument(argument, (float)container.ScaleDeliveryPacks[(int)argument]);
                    }
                }
                I_ExtendedEffect effect = builder.Build(deliveryTool, deliveryTool, packs);
                currentEffects[skillNode] = effect;
                effect.Enable();
            }
            skillNodeToLevel[skillNode] = rank;
        }

        public int GetCurrentLevel(Node skillNode)
        {
            if (skillNodeToLevel.TryGetValue(skillNode, out int level))
            {
                return level;
            }
            throw new Exception("Invalid skill node");
        }

        public bool IsNodeMax(Node node)
        {
            if (skillNodeToLevel.TryGetValue(node, out int level))
            {
                return level == node.maxRanks;
            }
            throw new Exception("Invalid skill node");
        }

        public object CaptureState()
        {
            SkillTreeSaveData save = new SkillTreeSaveData();
            save.skills = new List<SkillSaveData>();
            foreach (KeyValuePair<Node, int> skillToLevel in skillNodeToLevel)
            {
                SkillSaveData skillSaveData = new SkillSaveData();
                skillSaveData.level = skillToLevel.Value;
                skillSaveData.skillNodeName = skillToLevel.Key.displayName;
                save.skills.Add(skillSaveData);
            }
            return save;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }
            Dictionary<string, Node> skillNameToSKillNode = new();
            foreach (Node node in skillTree.GetNodes(subclass))
            {
                skillNameToSKillNode.Add(node.displayName, node);
            }
            SkillTreeSaveData save = (SkillTreeSaveData)state;
            foreach (SkillSaveData skillSaveData in save.skills)
            {
                Node skillNode = skillNameToSKillNode[skillSaveData.skillNodeName];
                if (skillSaveData.level > 0)
                {
                    ApplySkill(skillNode, skillSaveData.level);
                }
            }
            skillPoints = save.skillPoints;
        }

        public void PrepareRestoreState()
        {
            foreach (Node node in skillTree.GetNodes(subclass))
            {
                Skill skill = node.nodeElement as Skill;
                if (skill.skillType == Skill.SkillType.Ability)
                {
                    AbilityHolder ah = toolManager.Get<AbilityTool>().AbilityHolder;
                    ah.RevokeAbility(node.displayName);
                }
                else if (skill.skillType == Skill.SkillType.Passive)
                {
                    if (currentEffects[node] != null)
                    {
                        currentEffects[node].Disable(false);
                        currentEffects[node] = null;
                    }
                }
                skillNodeToLevel[node] = 0;
            }
        }

        public List<List<NodeUIContainer>> GetNodeTreeDefinition()
        {
            return skillTree.GetSkillTree(subclass);
        }

        public int GetTotalPoints()
        {
            return skillPoints;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public int GetTierLevel(Node node)
        {
            AbilityHolder ah = toolManager.Get<AbilityTool>().AbilityHolder;
            List<AbilityTag> tags = ah.GetAbilityTags(node.displayName, toolManager);
            return shiftableTierLevelTool.CalculateTierLevel(tags);
        }
    }

    [Serializable]
    public struct SkillTreeSaveData
    {
        public List<SkillSaveData> skills;
        public int skillPoints;
    }

    [Serializable]
    public struct SkillSaveData
    {
        public int level;
        public string skillNodeName;
    }

    public enum NodeIncreaseRequestResponse
    {
        SUCCESS, MAX_LEVEL, REQUIREMENNTS_NOT_MET, MISSING_SKILL_POINTS
    }
}