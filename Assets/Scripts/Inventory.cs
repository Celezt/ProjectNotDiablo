using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    private Item[] items;
    public HotbarScript scriptMain;

    public Inventory()
    {
        items = new Item[5];
    }

    public bool AddItem(Item item)
    {
        int indexFirstNull = -1;
        bool sameItem = false;
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                if(indexFirstNull == -1)
                {
                    indexFirstNull = i;
                }
            }
            else if(items[i].itemType == item.itemType)
            {
                sameItem = true;
                indexFirstNull = i;
                break;
            }
            
        }
        if (sameItem)
        {
            if(items[indexFirstNull].amount > 0)
            {
                return false;
            } 
            else
            {
                items[indexFirstNull].amount += 1;
                return true;
            }
        }
        else if(indexFirstNull != -1)
        {
            //add item to array
            items[indexFirstNull] = item;
            GameObject slot = scriptMain.GetSlot(indexFirstNull+1);
            GameObject baseItem = scriptMain.baseItemObject;
            GameObject temp = scriptMain.InstaniateObject(baseItem, slot);
            temp.transform.SetParent(slot.transform);
            Image img = temp.GetComponent<Image>();
            Sprite newSprite = Resources.Load<Sprite>("Items/Potion");
            Debug.Log(newSprite);
            img.sprite = newSprite;

            return true;
        }
        return false;
    }

    public bool RemoveItem(int index)
    {
        Item item = items[index];
        if(item.itemType != Item.ItemType.Spell || item.itemType != Item.ItemType.Sword)
        {
            
            if (items[index].amount == 1)
            {
                items[index] = null;
                return true;
            }
            else items[index].amount -= 1;
        }
        Debug.Log(items[index].itemType);
        Debug.Log(items[index].amount);
        return false;
    }


    public Item[] GetInventory()
    {
        return items;
    }

    public void SetHotbardScript(HotbarScript script)
    {
        scriptMain = script;
    }
}
