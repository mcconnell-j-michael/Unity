using UnityEngine;
using System.Collections;
using System;

public class AutoPopulate : Attribute
{
    public Type instance;

    public AutoPopulate(Type instance = null)
    {
        this.instance = instance;
    }
}
