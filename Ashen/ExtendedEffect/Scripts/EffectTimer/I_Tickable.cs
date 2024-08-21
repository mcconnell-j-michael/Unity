using UnityEngine;
using System.Collections;

public interface I_Tickable
{
    void Tick();
    void End();
    void UpdateTime();
}
