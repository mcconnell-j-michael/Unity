using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeliveryArgumentPacks : I_Poolable
{
    private static int currentIndex = 0;

    private static I_DeliveryArgumentPack[] deliveryArgumentsStatic;
    private static I_DeliveryArgumentPack[] DeliveryArguments
    {
        get
        {
            if (deliveryArgumentsStatic == null)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(I_DeliveryArgumentPack));
                IEnumerable<Type> arguments = assembly.GetTypes().Where(t => typeof(I_DeliveryArgumentPack).IsAssignableFrom(t) && t.IsAbstract == false && t.IsInterface == false);
                deliveryArgumentsStatic = new I_DeliveryArgumentPack[arguments.Count()];
                foreach (Type argument in arguments)
                {
                    I_DeliveryArgumentPack pack = Activator.CreateInstance(argument) as I_DeliveryArgumentPack;
                    pack.SetIndex(currentIndex);
                    deliveryArgumentsStatic[currentIndex] = pack;
                    currentIndex++;
                }
            }
            return deliveryArgumentsStatic;
        }
    }

    public I_DeliveryArgumentPack[] deliveryArguments;

    public DeliveryArgumentPacks()
    {
        deliveryArguments = new I_DeliveryArgumentPack[DeliveryArguments.Length];
        for (int x = 0; x < deliveryArguments.Length; x++)
        {
            deliveryArguments[x] = DeliveryArguments[x].Initialize();
        }
    }

    public T GetPack<T>() where T : A_DeliveryArgumentPack<T>, new()
    {
        int index = A_DeliveryArgumentPack<T>.Index;
        if (deliveryArguments[index] == null)
        {
            T returnValue = new T();
            deliveryArguments[index] = returnValue;
        }
        return deliveryArguments[index] as T;
    }

    private Dictionary<string, bool> booleanArguments;

    public void SetBoolean(string key, bool value)
    {
        if (booleanArguments == null)
        {
            booleanArguments = new Dictionary<string, bool>();
        }
        if (booleanArguments.ContainsKey(key))
        {
            booleanArguments[key] = value;
        }
        else
        {
            booleanArguments.Add(key, value);
        }
    }

    public bool IsTrue(string key)
    {
        if (booleanArguments == null)
        {
            return false;
        }
        if (!booleanArguments.ContainsKey(key))
        {
            return false;
        }
        return booleanArguments[key];
    }

    private Vector3? collisionSource;

    public void SetCollisionSource(Vector3 collision)
    {
        collisionSource = collision;
    }

    public Vector3? GetCollisionSource()
    {
        return collisionSource;
    }

    public void CopyInto(DeliveryArgumentPacks other)
    {
        if (this == other)
        {
            return;
        }
        for (int x = 0; x < other.deliveryArguments.Length; x++)
        {
            deliveryArguments[x].CopyInto(other.deliveryArguments[x]);
        }
    }

    public void Disable()
    { }

    public void Initialize()
    {
        foreach (I_DeliveryArgumentPack pack in deliveryArguments)
        {
            if (pack != null)
            {
                pack.Clear();
            }
        }
        if (booleanArguments != null)
        {
            booleanArguments.Clear();
        }
        collisionSource = null;
    }
}
