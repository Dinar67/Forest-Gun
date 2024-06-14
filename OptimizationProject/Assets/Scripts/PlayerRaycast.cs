using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
            {
                IActiveObject activeObject = hit.collider.gameObject.GetComponent<IActiveObject>();
                if (activeObject != null)
                    activeObject.Activate(GetComponent<Inventory>());
            }
        }
    }
}
