namespace Ashen.ToolSystem
{
    public struct ShiftableChange<T>
    {
        public int priority;
        public string source;
        public T shift;
        public bool overwrite;
    }
}