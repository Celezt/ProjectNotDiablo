using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Popup : MonoBehaviour
{
    [SerializeField] private float _duration = 3.0f;
    [SerializeField] private bool _enableLifespan = true;
    [Space(10)]
    [SerializeField] private bool _enableDynamicScale;
    [SerializeField, ConditionalField(nameof(_enableDynamicScale))] private float _scaleMultiplier = 1.0f;
    [SerializeField, ConditionalField(nameof(_enableDynamicScale))] private AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 1, 1, 0);
    [Space(10)]
    [SerializeField] private bool _enableDynamicTranslation;
    [SerializeField, ConditionalField(nameof(_enableDynamicTranslation))] private Vector3 _translationMultiplier = new Vector3(1, 1, 1);
    [SerializeField, ConditionalField(nameof(_enableDynamicTranslation))] private AnimationCurve _translationXCurve = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField, ConditionalField(nameof(_enableDynamicTranslation))] private AnimationCurve _translationYCurve = AnimationCurve.Linear(0, -0.5f, 1, 0.5f);
    [SerializeField, ConditionalField(nameof(_enableDynamicTranslation))] private AnimationCurve _translationZCurve = AnimationCurve.Linear(0, 0, 1, 0);

    private Duration _lifespan;

    private Vector3 _initScale;

    private void Start()
    {
        _initScale = transform.localScale;

        if (_enableDynamicScale)
            StartCoroutine(CoroutineScale());

        if (_enableDynamicTranslation)
            StartCoroutine(CoroutineTranslation());

        if (_enableLifespan)
            _lifespan = new Duration(_duration);
    }

    private void FixedUpdate()
    {
        if (_enableLifespan)
            if (!_lifespan.IsActive)
            {
                Destroy(gameObject);
            }
    }

    private IEnumerator CoroutineScale()
    {
        do
        {
            Duration duration = new Duration(_duration);

            while (duration.IsActive)
            {
                float scale = _scaleCurve.Evaluate(1 - duration.UnitIntervalTimeLeft) * _scaleMultiplier;
                transform.localScale = _initScale * scale;

                yield return new WaitForFixedUpdate();
            }

        } while (!_enableLifespan);
    }

    private IEnumerator CoroutineTranslation()
    {
        do
        {
            Duration duration = new Duration(_duration);
            Vector3 originPositon = transform.position;

            while (duration.IsActive)
            {
                transform.position = new Vector3
                {
                    x = originPositon.x + _translationXCurve.Evaluate(1 - duration.UnitIntervalTimeLeft) * _translationMultiplier.x,
                    y = originPositon.y + _translationYCurve.Evaluate(1 - duration.UnitIntervalTimeLeft) * _translationMultiplier.y,
                    z = originPositon.z + _translationZCurve.Evaluate(1 - duration.UnitIntervalTimeLeft) * _translationMultiplier.z,
                };

                yield return new WaitForFixedUpdate();
            }

            transform.position = originPositon;

        } while (!_enableLifespan);
    }
}
