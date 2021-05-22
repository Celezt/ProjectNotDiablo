using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThoughFollow : MonoBehaviour
{


    
    public Camera camera;
    public LayerMask mask;
    RaycastHit hit;
    Collider previousHit;


    void Update()
    {
        var dir = camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        
        if (previousHit != hit.collider)
        {
            if (previousHit != null)
            {
                previousHit.gameObject.GetComponent<MeshRenderer>().enabled = true;
                
            }
        }

        if (Physics.Raycast(ray, out hit, 300, mask))
        {
            hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (previousHit != hit.collider && previousHit != null)
            {
                previousHit.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            previousHit = hit.collider;
        }
    }
}
