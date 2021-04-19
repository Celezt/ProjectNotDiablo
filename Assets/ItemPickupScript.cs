using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{

    public ItemType itemType;
    public FloatVariable health;
    public int healingValue = 25;
    public int speedValue = 5;
    public FloatVariable moveSpeed;


    void OnTriggerEnter()
    {
        switch (itemType)
        {
           
            case ItemType.HealthPotion:
                Debug.Log(health.Value);
                if(health.Value + healingValue > health.InitialValue)
                {
                    health.Value = health.InitialValue;
                }
                else
                {
                    health.Value += healingValue;
                }
                break;
            case ItemType.SpeedPotion:
                Debug.Log(moveSpeed.Value);
                moveSpeed.Value += speedValue;
                break;
        }
        Destroy(gameObject);
    }

    public enum ItemType
    {
        HealthPotion,
        SpeedPotion
    }

}
