using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private float _seconds;
    [SerializeField] private GameObject[] _cutSceneGameObjects;
    [SerializeField] private GameObject _cameraBrain;
    void Start()
    {
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        if (_cutSceneGameObjects.Length == 0)
            yield break;

        foreach (GameObject gameObject in _cutSceneGameObjects)
            gameObject.SetActive(true);
        yield return new WaitForSeconds(_seconds);
        foreach (GameObject gameObject in _cutSceneGameObjects)
            gameObject.SetActive(false);
        
        gameObject.SetActive(false);
    }
}
