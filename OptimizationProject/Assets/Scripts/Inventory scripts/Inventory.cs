using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject _weaponCam;
    public GameObject _inventoryCanvas;
    private Canvas _inventoryCanvasCanva;
    private PostProcessLayer _postProLayer;
    public Item[] itemsInGame;
    public List<Item> items;
    public Slot[] _slots;
    public Slot _weaponSlot;
    private bool isActive = false;
    public GameObject _weaponPanel;
    public GameObject Hint;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _successBuying;
    private int _money;
    [SerializeField] private Text _moneyText;
    public int Money{
        get { return _money; }
        set 
        { 
            _money = value;
            _moneyText.text = value.ToString("0");
        }
    }

    [SerializeField] private PlayerRotate playerRotate;
    [SerializeField] private PlayerRotateSmooth playerRotateSmooth;


    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _notEnoughMoney;

    private void Awake()
    {
        _postProLayer = GetComponent<PostProcessLayer>();
        _inventoryCanvasCanva = _inventoryCanvas.GetComponent<Canvas>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadInventory();
    }

    public void BuyItem(Item item)
    {
        int cost = item.cost;
        if (_notEnoughMoney == null)
        {
            return;
        }
        if (Money < cost)
        {
            StartCoroutine(ShowWarning("Недостаточно монет!"));
            return;
        }
        Slot slot = new Slot();
        bool isSearched = false;
        foreach(Slot a in _slots)
        {
            if(a.ItemInSlot == null)
            {
                slot = a; 
                isSearched = true;
                break;
            }
        }

        if (!isSearched)
        {
            StartCoroutine(ShowWarning("Инвентарь заполнен!"));
            return;
        }

        items.Add(item);
        int index = items.Count() - 1;
        Debug.Log($"{index}");
        Item item1 = Instantiate(items[index], _weaponPanel.transform);
        item1.slot = slot;
        item1.canvas = _inventoryCanvasCanva;
        item1.Hint = Hint;
        slot.ItemInSlot = item1;
        Money -= cost;
        _audioSource.PlayOneShot(_successBuying);
    }

    public bool GetItem(Item item)
    {
        foreach (Slot slot in _slots)
        {
            if (slot.ItemInSlot == null)
            {
                Item item1 = Instantiate(item, _weaponPanel.transform);
                item1.slot = slot;
                item1.canvas = _inventoryCanvasCanva;
                item1.Hint = Hint;
                slot.ItemInSlot = item1;
                items.Add(item1);
                return true;
            }
        }
        return false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventorySet();
        }
    }
    private void InventorySet()
    {

        if (Time.timeScale == 0.01f && !isActive) return;

        isActive = !isActive;
        if (isActive)
        {
            playerRotate.enabled = !isActive;
            playerRotateSmooth.enabled = !isActive;
        }
        
        _inventoryCanvas.SetActive(isActive);
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        _weaponCam.SetActive(!isActive);
        if (_postProLayer != null)
            _postProLayer.enabled = !isActive;
        Time.timeScale = Convert.ToInt32(!isActive) == 0? 0.01f : 1;
        if (!isActive)
        {
            playerRotate.enabled = !isActive;
            playerRotateSmooth.enabled = !isActive;
        }

    }

    private bool isShopOpened = false;
    public void OpenCloseShop()
    {
        isShopOpened = !isShopOpened;
        _inventoryPanel.SetActive(!isShopOpened);
        _shopPanel.SetActive(isShopOpened);
    }
    public void ResumeGame()
    {
        InventorySet();
    }

    public void SaveInventory()
    {
        if (items.Count == 0 && _weaponSlot.ItemInSlot == null)
            return;
        string weaponText = _weaponSlot.ItemInSlot == null? "None;;;" : $"{_weaponSlot.ItemInSlot.Name};;;";
        foreach(Slot slot in _slots)
        {
            weaponText += slot.ItemInSlot == null? "None;;;" : $"{slot.ItemInSlot.Name};;;";
        }
        weaponText += Money.ToString("0");
        File.WriteAllText(@"InventoryItems.txt", weaponText);
    }

    

    public void LoadInventory()
    {
        if (!File.Exists(@"InventoryItems.txt"))
            return;
        string text = File.ReadAllText(@"InventoryItems.txt");
        string[] stringItems = text.Split(";;;");

        int i = 0;
        int counterItems = 0;
        while(i < stringItems.Count())
        {
            if(stringItems.Length - 1 == i)
            {
                Money = Convert.ToInt32(stringItems[i]);
            }
            if (stringItems[i] == "None")
            {
                i++;
                continue;
            }
            
            foreach (var a in itemsInGame)
            {
                if (a.Name == stringItems[i] && i == 0)
                {
                    items.Add(a);
                    items[counterItems] = a;
                    items[counterItems].slot = _weaponSlot;
                    items[counterItems].canvas = _inventoryCanvasCanva;
                    Item item = Instantiate(items[counterItems], _weaponPanel.transform);
                    item.Hint = Hint;
                    _weaponSlot.ItemInSlot = item;
                    _weaponSlot.EquipWeapon();
                    counterItems++;
                    break;
                }
                else if(a.Name == stringItems[i])
                {
                    items.Add(a);
                    items[counterItems].slot = _slots[i - 1];
                    items[counterItems].canvas = _inventoryCanvasCanva;
                    Item item = Instantiate(items[counterItems], _weaponPanel.transform);
                    item.Hint = Hint;
                    _slots[i - 1].ItemInSlot = item;
                    counterItems++;
                }
            }
            i++;
        }

    }

    private IEnumerator ShowWarning(string text)
    {
        _notEnoughMoney.transform.GetChild(1).GetComponent<Text>().text = text;
        _notEnoughMoney.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        _notEnoughMoney.SetActive(false);
    }
}
