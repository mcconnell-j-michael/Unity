using Sirenix.OdinInspector;
using UnityEngine;

/**
 * Every LogTool must implement this abstract class. This is an abstract
 * clas because we want to force all LogTools to extend SerializedScriptableObject
 **/
public abstract class A_LogTool : SerializedScriptableObject
{
    public abstract void DebugLog(string toLog);
    public abstract void ErrorLog(string toLog);
}
