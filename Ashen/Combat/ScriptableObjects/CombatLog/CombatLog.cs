using System;
using System.Collections.Generic;

public class CombatLog : SingletonScriptableObject<CombatLog>
{
    private List<I_LogMessageChangedListener> listeners;
    private List<I_LogMessageChangedListener> Listeners
    {
        get
        {
            if (listeners == null)
            {
                listeners = new List<I_LogMessageChangedListener>();
            }
            return listeners;
        }
    }

    [NonSerialized]
    private List<string> logMessages;
    private List<string> LogMessages
    {
        get
        {
            if (logMessages == null)
            {
                logMessages = new List<string>();
            }
            return logMessages;
        }
    }

    public void RegisterListener(I_LogMessageChangedListener listener)
    {
        Listeners.Add(listener);
    }

    public void RemoveListener(I_LogMessageChangedListener listener)
    {
        Listeners.Remove(listener);
    }

    public void AddMessage(string message)
    {
        LogMessages.Add(message);
        OnMessageChanged();
    }

    public void ClearMessages()
    {
        LogMessages.Clear();
        OnMessageChanged();
    }

    public string GetMessage(int index)
    {
        if (index < 0)
        {
            index = LogMessages.Count + index;
        }
        if (index < 0 || index >= LogMessages.Count)
        {
            return null;
        }
        return LogMessages[index];
    }

    public string GetLastMessage()
    {
        return GetMessage(-1);
    }

    private void OnMessageChanged()
    {
        foreach (I_LogMessageChangedListener listener in Listeners)
        {
            listener.OnLogMessageChanged();
        }
    }
}
