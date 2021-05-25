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

            _data.StunMoveList.Add(Duration.Infinity);
            _data.StunAttackList.Add(Duration.Infinity);
            _data.StunDodgeList.Add(Duration.Infinity);
            _data.DieEvent.Raise();
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
