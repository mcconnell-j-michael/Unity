using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    /**
     * The ResistanceTool will manage all the resistances to each damage type that a character can receive
     **/
    public class ResistanceTool : A_ConfigurableTool<ResistanceTool, ResistanceToolConfiguration>, I_Shiftable<DamageType, int>
    {
        //private ShiftableEquation[] resistances;
        //private List<I_Cacheable>[] cacheables;
        private DerivedAttribute[] attributes;

        //private int[] values;
        //private bool[] valid;
        private bool[] enabledResistances;
        private Dictionary<string, DamageType> keyToDamageType;

        private AttributeTool attributeTool;

        private AttributeLimiter resistanceAttributeLimiter;

        public override void Initialize()
        {
            base.Initialize();
            int size = DamageTypes.Count;
            //resistances = new ShiftableEquation[size];
            //cacheables = new List<I_Cacheable>[size];
            attributes = new DerivedAttribute[size];
            //values = new int[size];
            //valid = new bool[size];
            enabledResistances = new bool[size];
            keyToDamageType = new Dictionary<string, DamageType>();
            for (int x = 0; x < size; x++)
            {
                ShiftableEquation equation = new();
                //resistances[x] = equation;
                enabledResistances[x] = false;
                if (Config.ResistanceEquations.ContainsKey(DamageTypes.Instance[x]))
                {
                    equation.BaseValue = Config.ResistanceEquations[DamageTypes.Instance[x]].equation;
                    enabledResistances[x] = true;
                    attributes[x] = Config.ResistanceEquations[DamageTypes.Instance[x]];
                }
                //cacheables[x] = new List<I_Cacheable>();
            }
            resistanceAttributeLimiter = Config.ResistanceAttributeLimiter;
        }

        public void Start()
        {
            //foreach (DamageType damageType in DamageTypes.Instance)
            //{
            //    if (!enabledResistances[(int)damageType])
            //    {
            //        continue;
            //    }
            //    resistances[(int)damageType].BaseValue.AddInvalidationListener(toolManager.Get<DeliveryTool>(), this, "ResistanceTool-" + damageType.ToString());
            //    GetResistance(damageType, null);
            //}
            this.attributeTool = toolManager.Get<AttributeTool>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void AddShift(DamageType enumValue, int priority, ShiftCategory shiftCategory, string source, int value)
        {
            if (!enabledResistances[(int)enumValue])
            {
                return;
            }
            attributeTool.AddShift(attributes[(int)enumValue], priority, shiftCategory, source, new SimpleValue(value));
        }

        public void RemoveShift(DamageType enumValue, ShiftCategory shiftCategory, string source)
        {
            if (!enabledResistances[(int)enumValue])
            {
                return;
            }
            attributeTool.RemoveShift(attributes[(int)enumValue], shiftCategory, source);
        }

        public int GetResistance(DamageType damageType)
        {
            if (!enabledResistances[(int)damageType])
            {
                return 0;
            }
            int damageTypeNum = (int)damageType;
            return (int)attributeTool.GetAttribute(attributes[damageTypeNum], resistanceAttributeLimiter);
            //if (valid[damageTypeNum])
            //{
            //    return values[damageTypeNum];
            //}
            //valid[damageTypeNum] = true;
            //values[damageTypeNum] = (int)resistances[damageTypeNum].GetValue(toolManager.Get<DeliveryTool>(), extraArguments);
            //return values[damageTypeNum];
        }

        public float GetResistancePercentage(DamageType damageType)
        {
            float resistance = GetResistance(damageType);
            float resistancePercentage = (100f - resistance) / 100f;
            return resistancePercentage;
        }

        //public void OnChange(DamageType damageType)
        //{
        //    if (!enabledResistances[(int)damageType])
        //    {
        //        return;
        //    }
        //    valid[(int)damageType] = false;
        //    GetResistance(damageType, null);
        //    foreach (I_Cacheable cacheable in cacheables[(int)damageType])
        //    {
        //        cacheable.Recalculate(toolManager.Get<DeliveryTool>(), null);
        //    }
        //}

        //public void Cache(DamageType damageType, I_Cacheable toCache)
        //{
        //    if (!enabledResistances[(int)damageType])
        //    {
        //        return;
        //    }
        //    if (!cacheables[(int)damageType].Contains(toCache))
        //    {
        //        cacheables[(int)damageType].Add(toCache);
        //    }
        //}

        //public void Invalidate(I_DeliveryTool toolManager, string key)
        //{
        //    DamageType damageType = keyToDamageType[key];
        //    OnChange(damageType);
        //}

        /*[ReadOnly, ShowInInspector]
        private Dictionary<DamageType, int> resistanceValues = new Dictionary<DamageType, int>();
        
        [Button]
        private void BuildResistanceValues()
        {
            if (DamageTypes.Instance == null)
            {
                resistanceValues = null;
                return;
            }
            resistanceValues = new Dictionary<DamageType, int>();
            foreach (DamageType dt in DamageTypes.Instance)
            {
                resistanceValues.Add(dt, 0);
            }
            foreach (DamageType dt in DamageTypes.Instance)
            {
                if (values != null && values.Length == DamageTypes.Instance.Count)
                {
                    resistanceValues[dt] = values[(int)dt];
                }
            }
        }*/
    }
}
