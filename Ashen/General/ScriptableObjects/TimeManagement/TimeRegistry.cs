using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * The TimeRegistry is used to manage anything that needs to be timed
 * it will send out regular time updates to each I_Timed class registered to it
 **/
[CreateAssetMenu(fileName = "TimeRegistry", menuName = "Custom/Managers/TimeRegistry")]
public class TimeRegistry : SingletonScriptableObject<TimeRegistry>
{
    public float tickTime = 0.1f;
    public bool turnBased = false;

    private float currentTime = 0f;

    private void Awake()
    {
        Instance.Init();
    }

    public void UpdateTime()
    {
        if (!paused)
        {
            float time = Time.deltaTime;
            currentTime += time;
            if (currentTime >= tickTime)
            {
                currentTime -= tickTime;
                FireTick();
            }
        }
    }

    private void FireTick()
    {
        for (int x = 0; x < listeners.Count; x++)
        {
            I_Timed timed = listeners[x];
            if (timed == null)
            {
                UnorderedListUtility<I_Timed>.RemoveAt(listeners, x);
                x--;
            }
            else if (!timed.IsEnabled())
            {
                timed.StopTracking();
                UnorderedListUtility<I_Timed>.RemoveAt(listeners, x);
                x--;
            }
            else
            {
                timed.UpdateTime();
            }
        }
    }

    public static float GetDeltaTime()
    {
        if (Instance.paused)
        {
            return 0f;
        }
        return Time.deltaTime;
    }

    public static bool IsPaused()
    {
        return Instance.paused;
    }

    [OdinSerialize]
    private List<I_Timed> listeners;
    private bool paused;

    void Init()
    {
        if (listeners == null)
        {
            listeners = new List<I_Timed>();
        }
    }

    public void Clear()
    {
        if (listeners != null)
        {
            listeners.Clear();
        }
    }

    public static void AddListener(I_Timed timed)
    {
        if (!timed.IsTracked())
        {
            timed.StartTracking();
            Instance.listeners.Add(timed);
        }
    }

    public static void RemoveListener(I_Timed timed)
    {
        Instance.listeners.Remove(timed);
    }

    public void InstancePause()
    {
        Pause();
    }

    public static void Pause()
    {
        Instance.paused = !Instance.paused;
    }

    public static int GetTotal()
    {
        return Instance.listeners.Count;
    }
}
