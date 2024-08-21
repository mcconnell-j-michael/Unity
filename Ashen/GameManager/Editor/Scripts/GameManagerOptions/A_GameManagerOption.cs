using UnityEngine;
using System.Collections.Generic;

namespace Ashen.GameManagerWindow
{
    public abstract class A_GameManagerOption
    {
        protected GameManagerState state;

        public virtual void OnDeselected(){}
        public virtual void OnSelected(){}
        public virtual void OnNew(ScriptableObject scriptableObject){}
        public virtual void OnDelete(ScriptableObject scriptableObject){}
        public virtual void Initialize(){}
        public virtual void AddTarget(List<object> targets)
        {
            targets.Add(null);
        }
        public virtual string GetName()
        {
            return null;
        }

        public GameManagerState GetState()
        {
            return state;
        }

        public abstract A_GameManagerOption Clone(GameManagerState state);
        public abstract GameManagerOptionType GetGameManagerOptionType();
    }
}