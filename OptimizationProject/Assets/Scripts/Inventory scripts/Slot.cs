using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    private Item itemInSlot;
    public Item ItemInSlot
    {
        get { return itemInSlot; }
        set
        {
            itemInSlot = value;
            if (value != null)
                SetItemToSlot(value.gameObject);
        }
    }
    private void SetItemToSlot(GameObject item)
    {
        RectTransform rect = item.GetComponent<RectTransform>();
        if (rect != null)
            rect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }
    public Weapon[] _weapons;


    private void Start()
    {
        if (ItemInSlot != null)
            ItemInSlot.slot = this;
        EquipWeapon();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Drop(eventData.pointerDrag);
    }

    public void Drop(GameObject eventData)
    {
        if (eventData != null)
        {
            if (ItemInSlot == null)
            {
                ItemInSlot = eventData.GetComponent<Item>();
                ItemInSlot.slot = this;
                PlayerPrefs.SetInt("isSloted", 1);
                EquipWeapon();
            }
            else
            {
                Item newItem = eventData.GetComponent<Item>();
                Item oldItem = ItemInSlot;
                ItemInSlot = newItem;
                newItem.slot.Drop(oldItem.gameObject);
                ItemInSlot.slot = this;
                PlayerPrefs.SetInt("isSloted", 1);
                EquipWeapon();
            }

        }
    }

    public void EquipWeapon()
    {
        if (gameObject.name != "SlotWeapon") return;
        foreach (Weapon weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        if (ItemInSlot == null) return;
        foreach (Weapon weapon in _weapons)
        {
            if (weapon.gameObject.name == ItemInSlot.Name)
                weapon.gameObject.SetActive(true);

        }
    }

}
