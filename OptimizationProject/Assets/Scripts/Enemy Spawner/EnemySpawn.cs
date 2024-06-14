using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnerPoints;
    [SerializeField] private GameObject _panelText;
    [SerializeField] public List<GameObject> _enemies = new List<GameObject>();
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _radiusSpawn;


    private int _enemyCount;
    private int _currentStage;
    private bool _stageStarted = false;
    private System.Random random = new System.Random();
    private bool _isLockedUpdate = true;
    private void Start()
    {
        _currentStage = 1;
        _enemyCount = 15;
        _stageStarted = true;

        if (File.Exists(@"StageData.txt"))
        {
            _currentStage = Convert.ToInt32(File.ReadAllText(@"StageData.txt"));
            if (_currentStage != 1 && _currentStage < 19)
                for (int i = 1; i < _currentStage; i++)
                    _enemyCount += 5;
            else if (_currentStage >= 19)
                _enemyCount = 100;
        }
        else
            File.WriteAllText(@"StageData.txt", "1");
        StartCoroutine(StartStage());
    }
    private float _counter = 0f;
    
    private void Update()
    {
        if (_counter < 2f)
        {
            _counter += Time.deltaTime;
            return;
        }
        else
            _counter = 0;

        if(_enemies.Count == 0 && _stageStarted && !_isLockedUpdate)
            _stageStarted = false;

        if(_enemies.Count == 0 && !_stageStarted)
        {
            _isLockedUpdate = true;
            _currentStage += 1;
            if (_currentStage < 19)
                _enemyCount += 5;
            _stageStarted = true;
            StartCoroutine(StartStage());
        }
    }

    private IEnumerator StartStage()
    {
        yield return new WaitForSeconds(10f);
        ViewStage();
        for (int i = 0; i < _enemyCount; i++)
        {
            float x = _spawnerPoints[i% _spawnerPoints.Length].position.x 
                + random.Next(Convert.ToInt32(-_radiusSpawn * 10), Convert.ToInt32(_radiusSpawn * 10)) / 10;
            float z = _spawnerPoints[i % _spawnerPoints.Length].position.z
                + random.Next(Convert.ToInt32(-_radiusSpawn * 10), Convert.ToInt32(_radiusSpawn * 10)) / 10;

            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.GetComponentInChildren<HealthControl>()._enemySpawn = this;
            enemy.gameObject.transform.position 
                = new Vector3(x, _spawnerPoints[i% _spawnerPoints.Length].gameObject.transform.position.y, z);
            _enemies.Add(enemy);
        }
        _isLockedUpdate = false;
    }

    private void ViewStage()
    {
        StartCoroutine(FadeAndUnFade());
    }

    private IEnumerator FadeAndUnFade()
    {
        Image panelImage = _panelText.GetComponent<Image>();
        Text panelText = _panelText.GetComponentInChildren<Text>();
        panelText.text = $"Stage {_currentStage}";
        while(panelImage.color.a < 1f)
        {
            yield return null;
            Color color = panelImage.color;
            Color color1 = panelText.color;
            color.a += 0.6f * Time.deltaTime;
            color1.a += 0.6f * Time.deltaTime;
            panelText.color = color1;
            panelImage.color = color;
        }
        yield return new WaitForSeconds(3f);
        while (panelImage.color.a > 0f)
        {
            yield return null;
            Color color = panelImage.color;
            Color color1 = panelText.color;
            color.a -= 0.6f * Time.deltaTime;
            color1.a -= 0.6f * Time.deltaTime;
            panelText.color = color1;
            panelImage.color = color;
        }
    }

    public void SaveData()
    {
        File.WriteAllText(@"StageData.txt", $"{_currentStage}");
    }
    
}
