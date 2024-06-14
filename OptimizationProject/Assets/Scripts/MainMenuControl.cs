using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenuControl : MonoBehaviour
{
    public GameObject _mainUi;
    public GameObject _settingsUi;
    public GameObject _infoUi;

    public void OpenSettings()
    {
        _mainUi.SetActive(false);
        _settingsUi.SetActive(true);
        if(_infoUi != null)
            _infoUi.SetActive(false);
    }

    public void OpenMain()
    {
        _mainUi.SetActive(true);
        _settingsUi.SetActive(false);
        if (_infoUi != null)
            _infoUi.SetActive(false);
        
    }
    public void OpenInfo()
    {
        _mainUi.SetActive(false);
        _settingsUi.SetActive(false);
        if (_infoUi != null)
            _infoUi.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
