using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public abstract class A_SelectorPanel<T, E> : SingletonMonoBehaviour<T> where T : A_SelectorPanel<T, E> where E : A_Selector
    {
        public GameObject selectorHolder;
        public GameObject enabler;

        [NonSerialized]
        public int activeAbilities = 0;
        [NonSerialized]
        public List<E> selectors;

        private void Start()
        {
            selectors = new List<E>();
            Initialize();
        }

        protected virtual void Initialize() { }

        protected abstract void RegisterSelector(E selector);

        public E GetFirstSelector()
        {
            foreach (Transform child in selectorHolder.transform)
            {
                E selector = child.GetComponent<E>();
                if (selector != null)
                {
                    return selector;
                }
            }
            return null;
        }

        public virtual void UpdateSelection(E selector) { }
    }
}
