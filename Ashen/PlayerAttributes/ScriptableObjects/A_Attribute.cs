using Ashen.DeliverySystem;

public abstract class A_Attribute<T, E, F> : A_EnumSO<T, E> where T : A_Attribute<T, E, F> where E : A_EnumList<T, E>
{
    public abstract F Get(I_DeliveryTool toolManager);
    public abstract string GetAttributeType();

    public bool isOperation()
    {
        return false;
    }

    public override string ToString()
    {
        string value = base.ToString();
        if (value != null && value.Contains(" "))
        {
            return value.Substring(0, value.IndexOf(' '));
        }
        return value;
    }
}
