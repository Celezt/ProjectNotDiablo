using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class FloatBar : MonoBehaviour
{
    /// <summary>
    /// Bar progress value. Clamp between 0 and max value.
    /// </summary>
    public float Value
    {
        get => _valueReference.Value;
        set => _valueReference.Value = Mathf.Clamp(value, 0, MaxValue);
    }

    /// <summary>
    /// Bar progress max value. Min value 0.
    /// </summary>
    public float MaxValue
    {
        get => _maxValueReference.Value;
        set => _maxValueReference.Value = Math.Max(0, value);
    }

    [SerializeField] private FloatReference _valueReference = new FloatReference();
    [SerializeField] private FloatReference _maxValueReference = new FloatReference();

    private Image _valueBarImage;

    public void OnValueChange()
    {
        if (_valueBarImage == null)
            return;

        _valueBarImage.fillAmount = Value / MaxValue;
    }

    private void Awake()
    {
        _valueBarImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (_valueReference.Usage >= 2)
            _valueReference.GetEvent<FloatEvent>().Register(OnValueChange);

        if (_maxValueReference.Usage >= 2)
            _maxValueReference.GetEvent<FloatEvent>().Register(OnValueChange);
    }

    private void OnDisable()
    {
        if (_valueReference.Usage >= 2)
            _valueReference.GetEvent<FloatEvent>().Unregister(OnValueChange);

        if (_maxValueReference.Usage >= 2)
            _maxValueReference.GetEvent<FloatEvent>().Unregister(OnValueChange);
    }
}
