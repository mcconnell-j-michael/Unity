using UnityEngine;
using System.Collections;

namespace Ashen.ToolSystem
{
    public interface I_DamageListener
    {
        void OnDamageEvent(DamageEvent damageEvent);
    }
}