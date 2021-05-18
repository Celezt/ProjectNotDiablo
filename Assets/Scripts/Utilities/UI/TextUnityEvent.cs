using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public partial class TextUnityEvent : MonoBehaviour
{
    private Text _text;

    public void SetFloat(float value)
    {
        if (_text == null)
            _text = GetComponent<Text>();

        _text.text = value.ToString();
    }

    public void SetInt(int value)
    {
        if (_text == null)
            _text = GetComponent<Text>();

        _text.text = value.ToString();
    }

    public void SetString(string value)
    {
        if (_text == null)
            _text = GetComponent<Text>();

        _text.text = value;
    }
}
