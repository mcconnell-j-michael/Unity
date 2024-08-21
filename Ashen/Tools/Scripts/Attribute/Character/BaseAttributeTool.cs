using Sirenix.OdinInspector;
using System;

namespace Ashen.ToolSystem
{
    public class BaseAttributeTool : A_CacheableTool<BaseAttributeTool, BaseAttributeToolConfiguration, BaseAttribute, float>, I_Saveable
    {
        [ShowInInspector]
        private int[] attributeValues;

        public override void Initialize()
        {
            base.Initialize();
            int size = BaseAttributes.Count;
            attributeValues = new int[size];
            foreach (BaseAttribute attribute in BaseAttributes.Instance)
            {
                if (Config.DefaultBase.TryGetValue(attribute, out int value))
                {
                    attributeValues[(int)attribute] = value;
                }
            }
        }

        protected override int GetEnumListSize()
        {
            return BaseAttributes.Count;
        }

        public override float Get(BaseAttribute attribute, DeliveryArgumentPacks equationArguments)
        {
            if (attributeValues == null)
            {
                return 0;
            }
            return attributeValues[(int)attribute];
        }

        [Button]
        public void AddBase(BaseAttribute attribute, int flat)
        {
            attributeValues[(int)attribute] += flat;
            OnChange(attribute);
        }

        public object CaptureState()
        {
            int[] savedAttributeValues = new int[this.attributeValues.Length];
            for (int x = 0; x < this.attributeValues.Length; x++)
            {
                savedAttributeValues[x] = this.attributeValues[x];
            }
            return new BaseAttributeSaveData
            {
                baseValues = savedAttributeValues
            };
        }

        public void RestoreState(object state)
        {
            BaseAttributeSaveData saveData = (BaseAttributeSaveData)state;
            foreach (BaseAttribute attrib in BaseAttributes.Instance)
            {
                this.attributeValues[(int)attrib] = saveData.baseValues[(int)attrib];
                OnChange(attrib);
            }
        }

        public void PrepareRestoreState()
        { }

        [Serializable]
        private struct BaseAttributeSaveData
        {
            public int[] baseValues;
        }
    }
}