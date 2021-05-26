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
    public System.DateTime created = System.DateTime.Now;
    Collider collider;
    void Awake()
    {
        collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;
        item = new Item();
        item.amount = 1;
        item.itemType = itemType;
        /*
        if(script == null)
        {
            GameObject temp = GameObject.Find("UIHotBar/Inventory");
            script = temp.GetComponent<HotbarScript>();
        }
        inventory = script.GetInventory();
        */
    }


    void OnTriggerEnter()
    {
        System.DateTime currentTime = System.DateTime.Now;
        if((currentTime-created).TotalMilliseconds < 2000)
        {
            return;
        }
        if(inventory == null)
        {
            if(script == null)
            {
                GameObject temp = GameObject.Find("UIHotBar/Inventory");
                script = temp.GetComponent<HotbarScript>();
            }
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
    /*
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
    */



    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }

    public void setItemType(Item.ItemType type)
    {
        this.itemType = type;
    }
}
