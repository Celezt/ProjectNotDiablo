
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
                default:
                    break;
            }
            if(item == null)
            {
                Debug.Log(item);
                return;
            }
            Debug.Log(item.itemType);
            if (item.itemType == Item.ItemType.Sword || item.itemType == Item.ItemType.Spell)
            {
              
            }
            else
            {
                Debug.Log(item);
                bool wasRemoved = inventory.RemoveItem((int)inputKey - 1);
                GameObject slot = GetSlot((int)inputKey);
                
                if (wasRemoved)
                {
                    GameObject itemImage = slot.transform.Find("Item(Clone)").gameObject;
                    Image image = itemImage.GetComponent<Image>();
                    Debug.Log(itemImage);
                    Debug.Log(image);
                    Debug.Log(wasRemoved);
                    Destroy(itemImage);
                }
                
            }

            
        }
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

  
}
