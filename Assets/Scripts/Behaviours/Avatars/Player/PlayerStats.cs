using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerStats : MonoBehaviour
{
    #region Inspector
    [Header("Atoms")]
    [SerializeField] private FloatVariable _healthVariable;
    [SerializeField] private CooldownValueList _invisibleFrameVariable;
    #endregion

    #region Events
    public void OnEventChange(float health)
    {
        for (int i = 0; i < _invisibleFrameVariable.Count; i++)
        {
            Cooldown invisibleFrame = _invisibleFrameVariable[i];

            if (!invisibleFrame.IsActive)
                _invisibleFrameVariable.Remove(invisibleFrame);
        }
        Debug.Log(_invisibleFrameVariable.Count);
        if (_invisibleFrameVariable.Count != 0)
            _healthVariable.Value = _healthVariable.OldValue;
    }
    #endregion

    #region Unity Message
    private void OnEnable()
    {
        _healthVariable.Changed.Register(OnEventChange);
    }

    private void OnDisable()
    {
        _healthVariable.Changed.Unregister(OnEventChange);
    }
    #endregion
}
