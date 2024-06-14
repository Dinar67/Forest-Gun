using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public GameObject Hint;
    public int cost;
    public Item()
    {
        Name = "new Item";
    }
    public string Name;
    public Image ItemImage;
    [NonSerialized] public Slot slot;

    private RectTransform _itemRectTransform;
    private CanvasGroup _canvasGroup;
    [SerializeField] private bool useDragAndDrop = true;
    private void Awake()
    {
        _itemRectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!useDragAndDrop)
            return;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.6f;
        slot.ItemInSlot = null;
        slot.EquipWeapon();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!useDragAndDrop)
            return;
        _itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Hint.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!useDragAndDrop)
            return;
        Thread.Sleep(100);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        int isSloted = PlayerPrefs.GetInt("isSloted");
        if (isSloted == 0)
        {
            slot.ItemInSlot = this;
            _itemRectTransform.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        }
        

        PlayerPrefs.SetInt("isSloted", 0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!useDragAndDrop)
            return;
        slot.Drop(eventData.pointerDrag);
    }

    private void OnMouseEnter()
    {
        if (Hint == null)
            return;
        Hint.SetActive(true);
        Hint.GetComponent<Hint>().SetStats(this);
        if(slot != null)
            Hint.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x + (slot.gameObject.name != "SlotWeapon"? 0 : 400),
                GetComponent<RectTransform>().anchoredPosition.y - (slot.gameObject.name != "SlotWeapon" ? 250 : 0));
        else
        {
            RectTransform rect = Hint.GetComponent<RectTransform>();
            RectTransform myRect = transform.parent.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y - 250);
        }

    }


    private void OnMouseExit()
    {
        if (Hint == null)
            return;
        Hint.SetActive(false);
    }
}
