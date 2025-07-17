using System.Collections;
using UnityEngine;

public class Spawners : MonoBehaviour {

    [Header("Load Stats")]
    public SpawnerEnemy spawnEnemy;
    public bool canLoadEnemies = true;

    private CameraController _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<CameraController>();
    }
    private void Update()
    {
        if (canLoadEnemies)
        {
            StartCoroutine(LoadEnemies());
            canLoadEnemies = false;
        }
    }
    private IEnumerator LoadEnemies()
    {
        yield return new WaitForSeconds(spawnEnemy.timerToInitial);

        while (spawnEnemy.totalEnemies > 0)
        {
            int count = (int)Random.Range(spawnEnemy.minMaxPerWaves.x, spawnEnemy.minMaxPerWaves.y);
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(CreateEnemy());
                spawnEnemy.totalEnemies--;
                if (spawnEnemy.totalEnemies <= 0) break;
                yield return new WaitForSeconds(spawnEnemy.timeBetweenEnemy);
            }

            yield return new WaitForSeconds(spawnEnemy.timeBetweenWaves);
        }
    }
    private IEnumerator CreateEnemy()
    {
        float valueX = Random.Range(_camera.minPos.x + 1, _camera.maxPos.x - 1);
        float valueZ = Random.Range(_camera.minPos.y + 1, _camera.maxPos.y - 1);
        Vector3 newPos = new Vector3(valueX, 0.5f, valueZ);

        GameObject baseView = Instantiate(spawnEnemy.basePrefab, newPos, Quaternion.identity);
        yield return new WaitForSeconds(spawnEnemy.timeToAppear);

        GameObject enemy = Instantiate(spawnEnemy.enemyPrefab.gameObject, newPos, Quaternion.identity);
        Destroy(baseView.gameObject);
    }
}