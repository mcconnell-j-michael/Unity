using Ashen.EnumSystem;
using System;

namespace Ashen.EquationSystem
{
    [Serializable]
    public struct InvalidationIdentifier
    {
        public string source;
        public string key;
        public I_EnumSO enumKey;
    }
}