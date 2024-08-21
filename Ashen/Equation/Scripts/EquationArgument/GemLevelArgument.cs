using UnityEngine;
using System.Collections;

namespace Ashen.EquationSystem
{
    public class GemLevelArgument : I_EquationArgument
    {
        public static string GEM_LEVEL_ARGUMENT = "GemLevel";

        private int level;

        public GemLevelArgument(int level)
        {
            this.level = level;
        }

        public float GetValue()
        {
            return level;
        }

        public string GetKey()
        {
            return GEM_LEVEL_ARGUMENT;
        }

        public int DefaultValue()
        {
            return 1;
        }
    }
}