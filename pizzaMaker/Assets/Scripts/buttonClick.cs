using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Audio.Google;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_Text tmpText;
    public void texthandler()
    {
        tmpText = input.textComponent;
    }
}
