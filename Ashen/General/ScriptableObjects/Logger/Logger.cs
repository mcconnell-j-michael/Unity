using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The Logger will use whatever LogTool that it is given and use it to log messages
 **/
[CreateAssetMenu(fileName = "Logger", menuName = "Custom/Managers/Logging/Logger")]
public class Logger : SingletonScriptableObject<Logger>
{
    public bool isLoggingDebug;
    public bool isLoggingError;

    public A_LogTool logTool;

    private void DebugLogInner(string toLog)
    {
        if (isLoggingDebug)
        {
            logTool.DebugLog(toLog);
        }
    }

    private void ErrorLogInner(string toLog)
    {
        if (isLoggingError)
        {
            logTool.ErrorLog(toLog);
        }
    }

    public static void DebugLog(string toLog)
    {
        if (Instance)
        {
            if (Instance.isLoggingDebug)
            {
                Instance.DebugLogInner(toLog);
            }
        }
        else
        {
            Debug.LogError(toLog);
        }
    }

    public static void ErrorLog(string toLog)
    {
        if (Instance)
        {
            if (Instance.isLoggingError)
            {
                Instance.ErrorLogInner(toLog);
            }
        }
        else
        {
            Debug.LogError(toLog);
        }
    }
}
