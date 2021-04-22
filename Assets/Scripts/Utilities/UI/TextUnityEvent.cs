using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public partial class TextUnityEvent : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetFloat(float value)
    {
        _text.text = value.ToString();
    }

    public void SetInt(int value)
    {
        _text.text = value.ToString();
    }

    public void SetString(string value)
    {
        _text.text = value;
    }
}
