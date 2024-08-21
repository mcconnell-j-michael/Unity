using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    /**
     * This class is used to manage the various attributes of a Character (I.E. Strength, Dexerity, etc.)
     **/
    public class AttributeTool : A_ShiftableCacheTool<AttributeTool, AttributeToolConfiguration, DerivedAttribute, Equation, float, I_DeliveryValue>
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override int GetEnumListSize()
        {
            return DerivedAttributes.Count;
        }

        protected override IEnumerator<DerivedAttribute> GetEnumeratorInternal()
        {
            return DerivedAttributes.Instance.GetEnumerator();
        }

        public override A_Shiftable<Equation, float, I_DeliveryValue> GenerateShiftableValues()
        {
            EquationShiftableValues shiftableValues = new EquationShiftableValues(GetEnumListSize());
            Dictionary<DerivedAttribute, Equation> overrideEquations = Config.OverrideEquations;
            for (int x = 0; x < GetEnumListSize(); x++)
            {
                ShiftableEquation equation = new();
                if (overrideEquations.TryGetValue(DerivedAttributes.Instance[x], out Equation overrideEq))
                {
                    equation.BaseValue = overrideEq;
                    shiftableValues.SetDefault(x, overrideEq);
                }
                else
                {
                    equation.BaseValue = DerivedAttributes.Instance[x].equation;
                    shiftableValues.SetDefault(x, DerivedAttributes.Instance[x].equation);
                }
            }
            return shiftableValues;
        }
    }
}
