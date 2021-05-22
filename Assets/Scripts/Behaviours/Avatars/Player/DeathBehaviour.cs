using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerDataReference _data;
    [SerializeField] private AnimatorModifier _animationData;

    private bool _isDead;


    public void OnDeath(float health)
    {
        if (!_isDead && health <= 0)
        {
            _isDead = true;

            _data.AnimatorModifierEvent.Raise(new AnimatorModifier(_animationData.Clip, speedMultiplier: _animationData.SpeedMultiplier, exitPercent: 0.7f,
            enterAction: info =>
            {
                info.AnimatorBehaviour.EnableCustomAnimation = false;
                _data.StunMoveList.Add(Duration.Infinity);
                _data.StunAttackList.Add(Duration.Infinity);
                _data.StunDodgeList.Add(Duration.Infinity);
            },
            exitAction: info =>
            {
                info.AnimatorBehaviour.SetMotionSpeed(0);
                _data.DieEvent.Raise();
            }));
        }
    }

    public void OnEnable()
    {
        _data.Health.Changed.Register(OnDeath);
    }

    public void OnDisable()
    {
        _data.Health.Changed.Unregister(OnDeath);
    }
}
