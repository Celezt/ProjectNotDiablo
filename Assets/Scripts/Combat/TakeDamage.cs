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
    [SerializeField, ConditionalField(nameof(_damageable), true, Damageable.None)] AnimationClip _takeDamageClip;
    [SerializeField, ConditionalField(nameof(_damageable), false, Damageable.Player)] float _invisibilityFrame = 0.5f;


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
            data.AnimatorModifierEvent.Raise(new AnimatorModifier(_takeDamageClip));
            data.InvisibilityFrameList.Add(new Duration(_invisibilityFrame));
        }
        else if (gameObject.layer == LayerMask.NameToLayer("AI"))   // AI hit logic.
        {
            AI ai = gameObject.GetComponent<AI>();
            ai.health.Value -= damage;

            if (ai.health.Value > 0)
            {
                AnimatorBehaviour animatorBehaviour = gameObject.GetComponent<AnimatorBehaviour>();
                animatorBehaviour?.OnAnimationModifierRaised(new AnimatorModifier(_takeDamageClip));
            }
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Damageble"))
        {

        }

        if (_hasPopup && _popup != null)
        {
            Vector3 position = transform.position;
            GameObject instance = Instantiate(_popup, new Vector3(
                _offsetPopup.x + Random.Range(position.x - _randomPopupOffset, position.x + _randomPopupOffset),
                _offsetPopup.y + Random.Range(position.y - _randomPopupOffset, position.y + _randomPopupOffset),
                _offsetPopup.z + Random.Range(position.z - _randomPopupOffset, position.z + _randomPopupOffset)),
                Quaternion.identity);
            Text text = instance.GetComponentInChildren<Text>();
            text.text = damage.ToString();
        }
    }
}
