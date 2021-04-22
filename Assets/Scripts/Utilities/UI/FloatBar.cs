using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class FloatBar : MonoBehaviour
{
    [SerializeField] private FloatVariable _valueVariable;
    [SerializeField] private FloatVariable _maxValueVariable;

    private Image _valueBarImage;

    public void OnValueChange()
    {
        if (_valueBarImage == null)
            return;

        _valueBarImage.fillAmount = _valueVariable.Value / _maxValueVariable.Value;
    }

    private void Awake()
    {
        _valueBarImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _valueVariable?.Changed.Register(OnValueChange);
        _maxValueVariable?.Changed.Register(OnValueChange);
    }

    private void OnDisable()
    {
        _valueVariable?.Changed.Unregister(OnValueChange);
        _maxValueVariable?.Changed.Unregister(OnValueChange);
    }
}
