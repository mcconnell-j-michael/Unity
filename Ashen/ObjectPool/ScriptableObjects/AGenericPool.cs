using UnityEngine.Pool;

namespace Ashen.ObjectPoolSystem
{
    public class AGenericPool<T> : GenericPool<T> where T : class, I_Poolable, new()
    {
        public static new T Get()
        {
            T element = GenericPool<T>.Get();
            element.Initialize();
            return element;
        }

        public static new void Release(T element)
        {
            element.Disable();
            GenericPool<T>.Release(element);
        }
    }
}