using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;


public class ItemPickupScript : MonoBehaviour
{

    public Item item;
    public FloatVariable health;
    public Item.ItemType itemType;
    [SerializeField] public HotbarScript script;
    /*
    public int healingValue = 25;
    public int speedValue = 5;
    public FloatVariable moveSpeed;
    */
    public Inventory inventory;

    
    void Awake()
    {
        item = new Item();
        item.amount = 1;
        item.itemType = itemType;
        inventory = script.GetInventory();
    }


    void OnTriggerEnter()
    {   
        if(inventory == null)
        {
            inventory = script.GetInventory();
        }
        Debug.Log(inventory);
        Debug.Log(item);
        bool wasAdded = inventory.AddItem(item);

        /*
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
        */
        if (wasAdded)
        {
            Destroy(gameObject);
        }
        
        

    }
    void OnTriggerStay()
    {
        if (inventory == null)
        {
            inventory = script.GetInventory();
        }
        bool wasAdded = inventory.AddItem(item);
        if (wasAdded)
        {
            Destroy(gameObject);
        }

    }



    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }
}
