
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class HotbarScript : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerControls controller;
    private Inventory inventory;

    [SerializeField] public GameObject slot1;
    [SerializeField] public GameObject slot2;
    [SerializeField] public GameObject slot3;
    [SerializeField] public GameObject slot4;
    [SerializeField] public GameObject slot5;
    [SerializeField] public GameObject baseItemObject;
    [SerializeField] public FloatVariable health;
    [SerializeField] public FloatVariable speed;
    [SerializeField] public GameObject baseGroundObject;
    [SerializeField] public GameObject sword;
    [SerializeField] public GameObject spellbok;
    private float lastUsedSlot = -1;
    private bool speedBoosted = false;
    private System.DateTime speedBostedTime;


    void Update()
    {
        if (speedBoosted)
        {
            if((System.DateTime.Now - speedBostedTime).TotalMilliseconds > 5000)
            {
                speedBoosted = false;
                speed.Value = speed.InitialValue;
            }
        }
    }

    private void Awake()
    {
        controller = new PlayerControls();
        inventory = new Inventory();
        /*
        inventory.AddItem(new Item
        {
            itemType = Item.ItemType.HealthPotion,
            amount = 1
        });
        
        GameObject temp = Instantiate(baseItemObject, slot1.transform);
        temp.transform.SetParent(slot1.transform);
        inventory.AddItem(new Item
        {
            itemType = Item.ItemType.SpeedPotion,
            amount = 1
        });
        Image img = temp.GetComponent<Image>();
        Sprite newSprite = Resources.Load<Sprite>("Items/Potion");
        Debug.Log(newSprite);
        img.sprite = newSprite;
        Debug.Log(img);
        GameObject temp2 = Instantiate(baseItemObject, slot2.transform);
        temp2.transform.SetParent(slot2.transform);
        */
        inventory.SetHotbardScript(this);
        inventory.SetHealth(health);
        inventory.SetSpeed(speed);
        inventory.SetSword(sword);
        inventory.SetSpellBook(spellbok);
         
    }

    private void OnEnable()
    {
        controller.Enable();
        controller.Ground.Hotbar.performed += HotbarListener;
        
        
    }

    private void OnDisable()
    {
        controller.Ground.Hotbar.performed -= HotbarListener;
       
        controller.Disable();
    }
    public void HotbarListener(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float inputKey = context.ReadValue<float>();

            Item item = null;
            
            switch (inputKey)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    item = inventory.GetInventory()[(int)inputKey - 1];
                    UseItemHotbar(inputKey, item);
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    if(lastUsedSlot != -1)
                    {
                        //Debug.Log("deepp");
                        item = inventory.GetInventory()[(int)lastUsedSlot-1];
                        DropItem(lastUsedSlot, item);
                    }
                    break;
                default:
                    break;
            }
            

            
        }
    }

    public void UseItemHotbar(float inputKey, Item item)
    {
        if (item == null)
        {
            //Debug.Log(item);
            return;
        }
        //Debug.Log(item.itemType);
        if (item.itemType == Item.ItemType.Sword)
        {

            bool success = inventory.UseItem(item, inputKey);
            if (success)
            {
                lastUsedSlot = inputKey;
            }

            //CHANGE WEAPON TO SWORD?
        }
        else if (item.itemType == Item.ItemType.Spell)
        {

            bool success = inventory.UseItem(item, inputKey);
            if (success)
            {
                lastUsedSlot = inputKey;
            }
            //CHANGE WEAPON TO SPELL?
        }
        else
        {
            //ONLY CONSUMABLE ITEMS LEFT USE ITEM AND REMOVE.
            bool wasUsed = inventory.UseItem(item, inputKey);
            //Debug.Log(wasUsed);
            bool wasRemoved = false;
            if (wasUsed)
            {
                wasRemoved = inventory.RemoveItem((int)inputKey - 1);
            }

            GameObject slot = GetSlot((int)inputKey);

            if (wasRemoved)
            {
                GameObject itemImage = slot.transform.Find("Item(Clone)").gameObject;
                Image image = itemImage.GetComponent<Image>();
                //Debug.Log(itemImage);
                //Debug.Log(image);
                //Debug.Log(wasRemoved);
                Destroy(itemImage);
            }

        }
    }

    public void DropItem(float inputkey, Item item)
    {
        GameObject newObject = baseGroundObject;
        ItemPickupScript sc = baseGroundObject.GetComponent<ItemPickupScript>();
        Debug.Log(sc);
        sc.setItemType(item.itemType);
        inventory.RemoveItem((int)inputkey - 1);
        GameObject player = GameObject.Find("Player");
        var pos = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y+0.5f, player.transform.localPosition.z);
        Quaternion q = new Quaternion();
        Instantiate(baseGroundObject,pos,q);
        GameObject slot = GetSlot((int)lastUsedSlot);
        GameObject itemImage = slot.transform.Find("Item(Clone)").gameObject;
        Image image = itemImage.GetComponent<Image>();
        //Debug.Log(itemImage);
        //Debug.Log(image);
        //Debug.Log(wasRemoved);
        lastUsedSlot = -1;
        Destroy(itemImage);
    }

    public GameObject GetSlot(int i)
    {
        switch (i)
        {
            case 1:
                return slot1;
            case 2:
                return slot2;
            case 3:
                return slot3;
            case 4:
                return slot4;
            case 5:
                return slot5;
            default:
                return slot1;

        }
    }

    public GameObject InstaniateObject(GameObject main, GameObject parent)
    {
        return Instantiate(main, parent.transform);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public void setBoostedTime(System.DateTime time)
    {
        this.speedBostedTime = time;
    }
    
    public void setBoosted(bool b)
    {
        this.speedBoosted = b;
    }
    public bool getBoosted()
    {
        return speedBoosted;
    }
}
