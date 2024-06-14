using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDoor : MonoBehaviour, IActiveObject
{
    private Animator _animator;
    private bool isOpen = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Activate(Inventory inventory) 
    {
        if (_animator == null)
            return;

        isOpen = !isOpen;
        _animator.SetBool("isOpen", isOpen);
    }

}
