using UnityEngine;
using System.Collections;
using Ashen.ToolSystem;

namespace Ashen.ToolSystem
{
    public class ToolReferencer : MonoBehaviour
    {
        public ToolManager reference;

        private void Awake()
        {
            ToolLookUp.Instance.Register(gameObject, reference);
        }
    }
}