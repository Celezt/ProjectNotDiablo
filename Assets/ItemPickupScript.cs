using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{

    public ItemType itemType;
    public FloatVariable health;


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

                break;
        }
        

        Destroy(gameObject);
    }

    public enum ItemType
    {
        healthPotion
    }

}
