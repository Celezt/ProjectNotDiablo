using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GetPosition : MonoBehaviour
{
    public Vector3 worldPosition;
    public LayerMask layerMask;
    public GameObject myPrefab;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            worldPosition = hit.point;
            Debug.Log(worldPosition);
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Instantiate(myPrefab, worldPosition, Quaternion.identity);
            }
        }
    }
}
