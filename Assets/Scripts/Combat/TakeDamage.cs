using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;
using MyBox;

public class TakeDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private Damageable _damageable;
    [SerializeField, ConditionalField(nameof(_damageable), true, Damageable.None)] AnimatorModifier _takeDamageAnimation;
    [SerializeField, ConditionalField(nameof(_damageable), false, Damageable.Player), Min(0)] float _invisibilityFrame = 0.5f;
    [SerializeField, ConditionalField(nameof(_damageable), false, Damageable.Player), Min(0)] float _stunAttack = 0.3f;

    [Header("Popup")]
    [SerializeField] private bool _hasPopup;
    [SerializeField, ConditionalField(nameof(_hasPopup))] private GameObject _popup;
    [SerializeField, ConditionalField(nameof(_hasPopup))] private float _randomPopupOffset = 20;
    [SerializeField, ConditionalField(nameof(_hasPopup))] private Vector3 _offsetPopup;

    private enum Damageable
    {
        None,
        Player,
        AI
    }

    public void ReciveDamage(float damage)
    {

        if (gameObject.layer == LayerMask.NameToLayer("Player"))    // Player hit logic.
        {
            PlayerDataReference data = gameObject.GetComponent<PlayerDataReference>();
            data.Health.Value -= damage;

            if (data.Health.Value > 0 && data.InvisibilityFrameList.Count <= 0)
            {
                if (_takeDamageAnimation.Clip != null)
                    data.AnimatorModifierEvent.Raise(new AnimatorModifier(_takeDamageAnimation.Clip, _takeDamageAnimation.SpeedMultiplier, _takeDamageAnimation.Exitpercent));
                data.StunAttackList.Add(new Duration(_stunAttack));
                data.InvisibilityFrameList.Add(new Duration(_invisibilityFrame));
                SpawnPopup(damage);
            }
        }
        else if (gameObject.layer == LayerMask.NameToLayer("AI"))   // AI hit logic.
        {
            AI ai = gameObject.GetComponent<AI>();
            AudioSource source = gameObject.GetComponent<AudioSource>();
            AudioClip clip = gameObject.GetComponent<AI>().hitSoundClip;
            source.Play();
            ai.health.Value -= damage;

            if (ai.health.Value > 0)
            {
                AnimatorBehaviour animatorBehaviour = gameObject.GetComponent<AnimatorBehaviour>();
                if (_takeDamageAnimation.Clip != null)
                    animatorBehaviour?.OnAnimationModifierRaised(new AnimatorModifier(_takeDamageAnimation.Clip, _takeDamageAnimation.SpeedMultiplier, _takeDamageAnimation.Exitpercent));

                SpawnPopup(damage);
            }
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Damageble"))
        {
            SpawnPopup(damage);
        }
    }

    private void SpawnPopup(float value)
    {
        if (_hasPopup && _popup != null)
        {
            Vector3 position = transform.position;
            GameObject instance = Instantiate(_popup, new Vector3(
                _offsetPopup.x + Random.Range(position.x - _randomPopupOffset, position.x + _randomPopupOffset),
                _offsetPopup.y + Random.Range(position.y - _randomPopupOffset, position.y + _randomPopupOffset),
                _offsetPopup.z + Random.Range(position.z - _randomPopupOffset, position.z + _randomPopupOffset)),
                Quaternion.identity);
            Text text = instance.GetComponentInChildren<Text>();
            text.text = value.ToString();
        }
    }
}
