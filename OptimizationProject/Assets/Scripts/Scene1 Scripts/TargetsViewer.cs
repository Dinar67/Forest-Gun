using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsViewer : MonoBehaviour
{
    [SerializeField] private List<HealthControl> targets;
    [SerializeField] private GameObject canvas;
    [SerializeField] private SceneLoader loader;
    [SerializeField] private Inventory inventory;
    private void Start()
    {
        StartCoroutine(targetsView());
    }
    private IEnumerator targetsView()
    {
        yield return new WaitForSeconds(3f);

        bool defeatedEnemy = targets.Count == targets.Where(x => x.Health == 0).Count();
        if (!defeatedEnemy)
            StartCoroutine(targetsView());
        else
        {
            inventory.SaveInventory();
            canvas.SetActive(true);
            loader.LoadScene("Terrain Bake");
        }

    }
}
