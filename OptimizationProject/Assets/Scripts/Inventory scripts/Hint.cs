using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public Text damageText;
    public Text firerateText;
    public Text nameText;
    public Text countBulletsText;
    [SerializeField] private Slot weaponSlot;

    public void SetStats(Item item)
    {
        
        foreach(Weapon a in weaponSlot._weapons)
        {
            if(a.gameObject.name == item.Name)
            {
                nameText.text = a._name;
                firerateText.text = a._fireRate.ToString("N1");
                damageText.text = a._damage.ToString("N1");
                countBulletsText.text = a._bulletsCount.ToString("0");
            }
        }
        
    }
}
