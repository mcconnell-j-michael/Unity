using UnityEngine;

public class TestRunner : MonoBehaviour
{
    public void Start()
    {
        string[] myStrings = new string[]
        {
            "string 1",
            "string 2",
            "string 3",
            "string 4"
        };
        //myStrings[0] = "string 1";
        //myStrings[1] = "string 2";
        //myStrings[2] = "string 3";
        //myStrings[3] = "string 4";

        for (int i = 0; i < myStrings.Length; i++)
        {
        }
        //bool keepGoing = true;
        //while (keepGoing)
        //{
        //    if (something triggesrs me to want to continue) {
        //        keepGoing = false;
        //    }
        //}

        foreach (string oneString in myStrings)
        {
            Debug.Log(oneString);
        }
    }
}
