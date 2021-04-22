using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerStats : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private FloatVariable _healthVariable;
    [SerializeField] private DurationValueList _invisibleFrameVariable;
    #endregion

    private float _oldValue;

    #region Events
    public void OnHealthChange(float health)
    {
        for (int i = 0; i < _invisibleFrameVariable.Count; i++)
        {
            Duration invisibleFrame = _invisibleFrameVariable[i];

            if (!invisibleFrame.IsActive)
                _invisibleFrameVariable.Remove(invisibleFrame);
        }

        if (_invisibleFrameVariable.Count != 0)
            _healthVariable.Value = _oldValue;
        else
            _oldValue = _healthVariable.Value;
    }
    #endregion

    #region Unity Message
    private void OnEnable()
    {
        _oldValue = _healthVariable.Value;
        _healthVariable.Changed.Register(OnHealthChange);
    }

    private void OnDisable()
    {
        _healthVariable.Changed.Unregister(OnHealthChange);
    }
    #endregion
}
