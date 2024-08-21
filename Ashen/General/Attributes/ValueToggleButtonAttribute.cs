using System;

public class ValueToggleButtonAttribute : Attribute
{
    public string MemberName;
    public ValueToggleButtonAttribute(string MemberName)
    {
        this.MemberName = MemberName;
    }
}