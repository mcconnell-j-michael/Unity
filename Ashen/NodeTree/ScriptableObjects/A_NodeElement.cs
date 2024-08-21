using Sirenix.OdinInspector;

namespace Ashen.NodeTreeSystem
{
    public class A_NodeElement : SerializedScriptableObject
    {
        [PropertyRange(1, 10), OnValueChanged(nameof(UpdateLevels))]
        public int levels = 1;

        public virtual void UpdateLevels() { }
    }
}