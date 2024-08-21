using Ashen.DeliverySystem;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = "Random", menuName = "Custom/Enums/Operations/Random")]
    public class RandomOperation : A_ArgumentOperation
    {
        public override string Representation()
        {
            return "Rand";
        }

        public override float RunOperation(List<float> args)
        {
            if (args.Count != 2)
            {
                return 0;
            }
            return Random.Range(args[0], args[1]);
        }

        public override bool Cache(I_DeliveryTool source, Equation equation)
        {
            return false;
        }

        public override bool IsCachable()
        {
            return false;
        }
    }
}