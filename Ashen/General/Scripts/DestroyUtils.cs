using UnityEngine;

public class DestroyUtils
{
    public static void Destroy(Object toDestroy)
    {
        if (Application.isPlaying)
        {
            Object.Destroy(toDestroy);
        }
        else
        {
            Object.DestroyImmediate(toDestroy);
        }
    }
}
