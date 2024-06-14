using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ActivateGun : MonoBehaviour, IActiveObject
{
    public Item item;
    [SerializeField] private GameObject _cutSceneObject;
    public void Activate(Inventory inventory) 
    {
        bool isGeted = inventory.GetItem(item);

        if (_cutSceneObject != null)
            _cutSceneObject.SetActive(true);

        if (isGeted)
            Destroy(this.gameObject);

        
    }


    
}
