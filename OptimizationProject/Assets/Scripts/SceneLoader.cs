using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    public GameObject _blackImage;
    public GameObject _progressBar;
    public Image progress_bar_image;
    public Text _progressText;
    AsyncOperation async_operation;

    string scene;
    public void LoadScene(string scene)
    {
        this.scene = scene;
        StartCoroutine(Async_load());
    }

    public void NewGamePlay()
    {
        scene = "Scene1";
        File.Delete(@"InventoryItems.txt");
        File.Delete(@"StageData.txt");
        StartCoroutine(Async_load());
    }
    public void ContinuePlay()
    {
        if (File.Exists(@"InventoryItems.txt") && File.Exists(@"StageData.txt"))
            scene = "Terrain Bake";
        else
            scene = "Scene1";
        StartCoroutine(Async_load());
    }
    IEnumerator Async_load()
    {
        float loading_progress;
        async_operation = SceneManager.LoadSceneAsync(scene);
        if(_progressBar != null)
            _progressBar.SetActive(true);
        if (_blackImage != null)
            _blackImage.SetActive(true);
        if(_progressText != null)
            _progressText.gameObject.SetActive(true);


        while (!async_operation.isDone)
        {
            loading_progress = Mathf.Clamp01(async_operation.progress / 0.9f);
            if (_progressText != null)
                _progressText.text = $"Загрузка... {(loading_progress * 100).ToString("0")}%";
            if (_progressBar != null)
                progress_bar_image.fillAmount = loading_progress;
            yield return null;
        }
    }
    
}
