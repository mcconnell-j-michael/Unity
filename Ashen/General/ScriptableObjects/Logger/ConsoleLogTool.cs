using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The ConsoleLogTool is used to log messages to the Unity console
 **/
[CreateAssetMenu(fileName = "ConsoleLogTool", menuName = "Custom/Managers/Logging/ConsoleLogTool")]
public class ConsoleLogTool : A_LogTool
{
    public override void DebugLog(string toLog)
    {
        Debug.Log(toLog);
    }

    public override void ErrorLog(string toLog)
    {
        Debug.LogError(toLog);
    }
}
