using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private GameObject _popup;
    [SerializeField] private float _randomPopupOffset = 20;
    [SerializeField] private Vector3 _offsetPopup;

    public void ReciveDamage(float damage)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))    // Player hit logic.
        {
            gameObject.GetComponent<PlayerDataReference>().Health.Value -= damage;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("AI"))   // AI hit logic.
        {
            gameObject.GetComponent<AI>().health.Value -= damage;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Damageble"))
        {

        }

        if (_popup != null)
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
