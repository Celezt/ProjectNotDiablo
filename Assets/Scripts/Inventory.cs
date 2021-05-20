using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using TMPro;

public class Inventory
{
    private Item[] items;
    public HotbarScript scriptMain;
    public int healingValue = 25;
    public int speedBoostValue = 5;
    public FloatVariable health;
    public FloatVariable speed;
    private int maxNumberOfItems = 3;
    public GameObject sword;
    public GameObject spellbook;
    public GameObject hands = GameObject.Find("HandsWeapon");

    public Inventory()
    {
        items = new Item[5];
    }

    public bool AddItem(Item item)
    {
        int indexFirstNull = -1;
        bool sameItem = false;
        Debug.Log(item.amount);
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
            if(items[indexFirstNull].amount >= maxNumberOfItems)
            {
                return false;
            } 
            else
            {
                if(item.itemType == Item.ItemType.Spell || item.itemType == Item.ItemType.Sword)
                {
                    return false;
                }
                items[indexFirstNull].amount += 1;
                Transform amountText = scriptMain.GetSlot(indexFirstNull + 1).transform.Find("Border").transform.Find("Amount");
                Debug.Log(amountText);
                if(amountText != null)
                {
                    TextMeshProUGUI uiText = amountText.GetComponent<TextMeshProUGUI>();
                    uiText.SetText(items[indexFirstNull].amount.ToString());
                }
                Debug.Log(items[indexFirstNull].amount);
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
            switch (item.itemType)
            {
                case Item.ItemType.HealthPotion:
                    break;
                case Item.ItemType.SpeedPotion:
                    newSprite = Resources.Load<Sprite>("Items/SpeedPotion");
                    break;
                case Item.ItemType.Spell:
                    newSprite = Resources.Load<Sprite>("Items/Spell");
                    break;
                case Item.ItemType.Sword:
                    newSprite = Resources.Load<Sprite>("Items/Sword");
                    break;

            }
            
            //Debug.Log(newSprite);
            img.sprite = newSprite;

            return true;
        }
        return false;
    }

    public bool RemoveItem(int index)
    {
        Item item = items[index];
        if(item.itemType != Item.ItemType.Spell && item.itemType != Item.ItemType.Sword)
        {

            if (items[index].amount == 1)
            {
                items[index] = null;
                return true;

            }
            else
            {

                int newAmount = items[index].amount -= 1;
                Transform amountText = scriptMain.GetSlot(index + 1).transform.Find("Border").transform.Find("Amount");
                Debug.Log(amountText);
                if (amountText != null)
                {

                    TextMeshProUGUI uiText = amountText.GetComponent<TextMeshProUGUI>();
                    if (newAmount == 1)
                    {
                        uiText.SetText("");
                    }
                    else uiText.SetText(newAmount.ToString());
                }
                
            }
        }
        else
        {
            Debug.Log("Turn white");
            GameObject tempSlot = scriptMain.GetSlot(index + 1) ;
            GameObject tempBorder = tempSlot.transform.Find("Border").gameObject;
            Image tempImg = tempBorder.GetComponent<Image>();
            tempImg.color = Color.white;
            items[index] = null;
            AttackBehaviour abScript = GameObject.Find("Player").GetComponent<AttackBehaviour>();
            Debug.Log(hands);
            abScript.SelectedWeapon = hands;

            return true;
        }
        //Debug.Log(items[index].itemType);
        Debug.Log(items[index].amount);
        return false;
    }

    public bool UseItem(Item item, float inputkey)
    {
        if (item.itemType == Item.ItemType.Spell || item.itemType == Item.ItemType.Sword)
        {


            AttackBehaviour abScript = GameObject.Find("Player").GetComponent<AttackBehaviour>();
            GameObject currentWeapon = abScript.SelectedWeapon;
            if(currentWeapon == null)
            {
                return false;
            }
            Melee melee = currentWeapon.GetComponent<Melee>();
            if ( melee != null)
            {
                if(melee.cooldownTimer > 0)
                {
                    return false;
                }
            }
            Ranged ranged = currentWeapon.GetComponent<Ranged>();
            if(ranged != null)
            {
                if (ranged.cooldownTimer > 0)
                {
                    return false;
                }
            }
                

            for (int i = 1; i < 6; i++)
            {
                GameObject tempSlot = scriptMain.GetSlot((int)i);
                GameObject tempBorder = tempSlot.transform.Find("Border").gameObject;
                Image tempImg = tempBorder.GetComponent<Image>();
                tempImg.color = Color.white;
            }
            GameObject slot = scriptMain.GetSlot((int)inputkey);
            GameObject border = slot.transform.Find("Border").gameObject;
            Image img = border.GetComponent<Image>();
            img.color = Color.red;
            
            if(item.itemType == Item.ItemType.Spell)
            {
                abScript.SelectedWeapon = spellbook;
            }
            else if(item.itemType == Item.ItemType.Sword)
            {
                abScript.SelectedWeapon = sword;
            }
            return true;

            
        }
        else
        {
            switch (item.itemType)
            {
                case Item.ItemType.HealthPotion:
                    if (health.Value + healingValue > health.InitialValue)
                    {
                        health.Value = health.InitialValue;
                    }
                    else
                    {
                        health.Value += healingValue;
                    }
                    return true;
                case Item.ItemType.SpeedPotion:
                    float val = speed.Value;
                    if (val != speed.InitialValue && val + speedBoostValue >= (speed.InitialValue + speedBoostValue))
                    {
                        return false;
                    }
                    else
                    {
                        //Debug.Log("beep");
                        speed.Value += speedBoostValue;
                        scriptMain.setBoosted(true);
                        scriptMain.setBoostedTime(System.DateTime.Now);
                        return true;
                    }
                    break;
            }
        }
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
    
    public void SetHealth(FloatVariable health)
    {
        this.health = health;
    }
    public void SetSpeed(FloatVariable speed)
    {
        this.speed = speed;
    }
    public void SetSword(GameObject obj)
    {
        this.sword = obj;
    }
    public void SetSpellBook(GameObject obj)
    {
        this.spellbook = obj;
    }
}
