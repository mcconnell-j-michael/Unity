using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
    public string textValue;

    public TextMeshProUGUI text;

    private void OnEnable()
    {
        text.text = textValue;
    }
}
