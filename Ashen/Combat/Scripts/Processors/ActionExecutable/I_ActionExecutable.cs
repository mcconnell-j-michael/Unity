using UnityEngine;
using System.Collections;

public interface I_ActionExecutable
{
    IEnumerator Execute(MonoBehaviour runner);
    bool IsFinished();
}
