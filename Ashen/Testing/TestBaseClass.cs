using UnityEngine;

public class TestBaseClass
{
    public string name;

    public TestBaseClass()
    {
        name = "default name";
    }

    public void PrintName()
    {
        string someVariable = "test";
        Debug.Log(name);
    }
}
