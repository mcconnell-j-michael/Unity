using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

public class StaticUtilities
{
#if UNITY_EDITOR
    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = default;
            asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public static List<T> FindAssetsByTypeGeneral<T>() where T : class
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(UnityEngine.Object)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object asset = default;
            asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

            if (asset != null && asset is T)
            {
                assets.Add(asset as T);
            }
        }
        return assets;
    }
#endif



    public static string BuildListLabelText(string type, string propertyName)
    {
        return "@" + nameof(StaticUtilities) + "." + nameof(StaticUtilities.BuildStringList) + "<" + type + ">(" + propertyName + ")";
    }

    public const string BEFORE_TYPE = "@" + nameof(StaticUtilities) + "." + nameof(StaticUtilities.BuildStringList) + "<";
    public const string AFTER_TYPE = ">(";
    public const string END = ")";

    public static string BuildStringList<T>(List<T> list) where T : UnityEngine.Object
    {
        if (list == null || list.Count == 0)
        {
            return "EMPTY";
        }
        string returnValue = "";
        foreach (T listItem in list)
        {
            if (listItem == null)
            {
                returnValue += "null";
            }
            else if (returnValue == "")
            {
                returnValue = listItem.name;
            }
            else
            {
                returnValue += ";" + listItem.name;
            }
        }
        return returnValue;
    }

#if UNITY_EDITOR
    public static List<UnityEngine.Object> FindAssetsByType(Type type)
    {
        List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
#endif

    public static bool IsSubclassOf(Type type, Type toCheck)
    {
        bool isGeneric = type.IsGenericTypeDefinition;
        while (toCheck != null && toCheck != typeof(object))
        {
            if (isGeneric)
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (type == cur)
                {
                    return true;
                }
            }
            else if (type == toCheck)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    public static Type GetSublcassOf(Type type, Type toCheck)
    {
        bool isGeneric = type.IsGenericTypeDefinition;
        while (toCheck != null && toCheck != typeof(object))
        {
            if (isGeneric)
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (type == cur)
                {
                    return toCheck;
                }
            }
            else if (type == toCheck)
            {
                return toCheck;
            }
            toCheck = toCheck.BaseType;
        }
        return null;
    }

    public static string GetTabs(int depth)
    {
        string visualization = "";
        for (int x = 0; x < depth; x++)
        {
            visualization += "\t";
        }
        return visualization;
    }

    public static void SaveInterfaceValue(SerializationInfo info, string name, object value)
    {
        if (value == null)
        {
            info.AddValue(name + ".exists", false);
        }
        else
        {
            info.AddValue(name + ".exists", true);
            info.AddValue(name + "." + nameof(Type), value.GetType().FullName);
            info.AddValue(name, value);
        }
    }

    public static T LoadInterfaceValue<T>(SerializationInfo info, string name)
    {
        bool valueExists = info.GetBoolean(name + ".exists");
        if (valueExists)
        {
            Type valueType = Type.GetType(info.GetString(name + "." + nameof(Type)));
            return (T)info.GetValue(name, valueType);
        }
        else
        {
            return default;
        }
    }

    public static void SaveSOFromLibrary(SerializationInfo info, string name, ScriptableObject so)
    {
        if (so == null)
        {
            info.AddValue(name + ".exists", false);
        }
        else
        {
            info.AddValue(name + ".exists", true);
            info.AddValue(name, so.name);
        }
    }

    public static SO LoadSOFromLibrary<L, SO>(SerializationInfo info, string name)
        where L : A_ScriptableObjectLibrary<L, SO> where SO : ScriptableObject
    {
        bool valueExists = info.GetBoolean(name + ".exists");

        if (valueExists)
        {
            return A_ScriptableObjectLibrary<L, SO>.Instance.GetScriptableObject(name);
        }
        else
        {
            return null;
        }
    }

    public static void SaveList<T>(SerializationInfo info, string name, List<T> list, Action<string, T> saveItem)
    {
        if (list == null)
        {
            info.AddValue(name + ".exists", false);
            return;
        }
        info.AddValue(name + ".exists", true);
        info.AddValue(name + ".Count", list.Count);
        for (int x = 0; x < list.Count; x++)
        {
            string elementName = name + "[" + x + "]";
            saveItem(elementName, list[x]);
        }
    }

    public static List<T> LoadList<T>(SerializationInfo info, string name, Func<string, T> loadItem)
    {
        bool valueExists = info.GetBoolean(name + ".exists");
        if (!valueExists)
        {
            return null;
        }
        List<T> list = new();
        int size = info.GetInt32(name + ".Count");
        for (int x = 0; x < size; x++)
        {
            string elementName = name + "[" + x + "]";
            list.Add(loadItem(elementName));
        }
        return list;
    }

    public static void SaveArray<T>(SerializationInfo info, string name, T[] array, Action<string, T> saveItem)
    {
        if (array == null)
        {
            info.AddValue(name + ".exists", false);
            return;
        }
        info.AddValue(name + ".exists", true);
        info.AddValue(name + ".Count", array.Length);
        for (int x = 0; x < array.Length; x++)
        {
            string elementName = name + "[" + x + "]";
            saveItem(elementName, array[x]);
        }
    }

    public static T[] LoadArray<T>(SerializationInfo info, string name, Func<string, T> loadItem)
    {
        bool valueExists = info.GetBoolean(name + ".exists");
        if (!valueExists)
        {
            return null;
        }
        int size = info.GetInt32(name + ".Count");
        T[] array = new T[size];
        for (int x = 0; x < size; x++)
        {
            string elementName = name + "[" + x + "]";
            array[x] = loadItem(elementName);
        }
        return array;
    }

    public static void SaveDictionary<Key, Value>(
        SerializationInfo info,
        string name,
        Dictionary<Key, Value> dict,
        Action<string, Key> saveKey,
        Action<string, Value> saveValue
    )
    {
        if (dict == null)
        {
            info.AddValue(name + ".exists", false);
            return;
        }
        info.AddValue(name + ".exists", true);
        info.AddValue(name + ".Count", dict.Count);
        int count = 0;
        foreach (KeyValuePair<Key, Value> kvp in dict)
        {
            string countName = name + "[" + count + "]";
            saveKey(countName + ".Key", kvp.Key);
            saveValue(countName + ".Value", kvp.Value);
            count++;
        }
    }

    public static Dictionary<Key, Value> LoadDictionary<Key, Value>(
        SerializationInfo info,
        string name,
        Func<string, Key> loadKey,
        Func<string, Value> loadValue
    )
    {
        bool valueExists = info.GetBoolean(name + ".exists");
        if (!valueExists)
        {
            return null;
        }
        Dictionary<Key, Value> dict = new();
        int size = info.GetInt32(name + ".Count");
        for (int x = 0; x < size; x++)
        {
            string countName = name + "[" + x + "]";
            dict.Add(
                loadKey(countName + ".Key"),
                loadValue(countName + ".Value")
            );
        }
        return dict;
    }

}
