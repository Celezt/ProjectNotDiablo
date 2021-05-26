using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour
{
    [SerializeField]
    GameObject blackBox;
    // Start is called before the first frame update
    void Start()
    {
        blackBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
