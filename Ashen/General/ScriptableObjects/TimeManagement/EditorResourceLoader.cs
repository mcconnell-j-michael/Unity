using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.General
{
    public class EditorResourceLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        public TimeManager timeManager;

        public int timeInSecondsBetweenUpdates = 20;
        private float currentTime;

        [ExecuteInEditMode]
        public void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime > timeInSecondsBetweenUpdates)
            {
                currentTime = 0;
                AddAllSingletonScriptableObjects();
            }
        }

        [Button]
        public void AddAllSingletonScriptableObjects()
        {
            List<SerializedScriptableObject> objects = StaticUtilities.FindAssetsByType<SerializedScriptableObject>();
            foreach (SerializedScriptableObject scriptableObject in objects)
            {
                Type type = scriptableObject.GetType();
                while (type != null)
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SingletonScriptableObject<>))
                    {
                        if (timeManager.preLoad == null)
                        {
                            timeManager.preLoad = new List<SerializedScriptableObject>();
                        }
                        if (!timeManager.preLoad.Contains(scriptableObject))
                        {
                            timeManager.preLoad.Add(scriptableObject);
                        }
                        break;
                    }
                    type = type.BaseType;
                }
            }
            for (int x = 0; x < timeManager.preLoad.Count; x++)
            {
                if (timeManager.preLoad[x] == null)
                {
                    timeManager.preLoad.RemoveAt(x);
                    x--;
                }
            }
        }
#endif
    }
}