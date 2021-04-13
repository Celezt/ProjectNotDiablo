using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{

    public ItemType itemType;
    public FloatVariable health;
    public int healingValue = 25;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        switch (itemType)
        {
           
            case ItemType.healthPotion:
                Debug.Log(health.Value);
                if(health.Value + healingValue > 100)
                {
                    health.Value = health.InitialValue;
                }
                else
                {
                    health.Value += healingValue;
                }
                break;
          
        }
        Debug.Log(health.Value);
        Destroy(gameObject);
    }

    public enum ItemType
    {
        healthPotion,
        speedPotion
    }

}
