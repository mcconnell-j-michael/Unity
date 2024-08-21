using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    public class MinOperation : A_ArgumentOperation
    {
        public override float RunOperation(List<float> args)
        {
            if (args.Count == 1)
            {
                return args[0];
            }
            if (args.Count != 0)
            {
                return Mathf.Min(args.ToArray());
            }
            return 0;
        }

        public override string Representation()
        {
            return "Min";
        }
    }
}