using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MenuControl : MonoBehaviour
{
    public GameObject _weaponCam;
    public GameObject _menuCanvas;
    private PostProcessLayer _postProLayer;
    private bool isActive = false;

    private void Awake()
    {
        _postProLayer = GetComponent<PostProcessLayer>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuSet();
        }
    }
    private void MenuSet()
    {
        if (Time.timeScale == 0.01f && !isActive) return;
        isActive = !isActive;
        Cursor.lockState = isActive ? CursorLockMode.Confined : CursorLockMode.Locked;
        _menuCanvas.SetActive(isActive);
        _weaponCam.SetActive(!isActive);
        if(_postProLayer != null)
            _postProLayer.enabled = !isActive;
        Time.timeScale = Convert.ToInt32(!isActive) == 0 ? 0.01f : 1;

    }
    public void ResumeGame()
    {
        MenuSet();
    }
}
