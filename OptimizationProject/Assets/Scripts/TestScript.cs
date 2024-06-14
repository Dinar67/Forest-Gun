using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, 2);
        foreach (Collider collider in playerColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Quaternion rotTarget = Quaternion.LookRotation(collider.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, 5f);
                break;
            }
        }
    }
}
