using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] private HealthControl healthControl;
    public void TakeDamage(float damage)
    {
        healthControl.Health -= damage;
    }
}
