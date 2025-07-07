using System.Collections;
using UnityEngine;

public class Spawners : MonoBehaviour {

    [Header("Appear Data")]
    public EnemyBase enemyPrefab;
    [Tooltip("Sprite de X en el suelo")] public GameObject basePrefab;
    public float timeToAppear;

    [Header("Load Stats")]
    public bool canLoadEnemies = true;
    [Space]
    public float timerToInitial;
    public float timeBetweenEnemy;
    public float timeBetweenWave;
    [Space]
    public int totalEnemies;
    public Vector2 minMaxPerWaves;

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
        yield return new WaitForSeconds(timerToInitial);

        while (totalEnemies > 0)
        {
            int count = (int)Random.Range(minMaxPerWaves.x, minMaxPerWaves.y);
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(CreateEnemy());
                totalEnemies--;
                if (totalEnemies <= 0) break;
                yield return new WaitForSeconds(timeBetweenEnemy);
            }

            yield return new WaitForSeconds(timeBetweenWave);
        }
    }
    private IEnumerator CreateEnemy()
    {
        float valueX = Random.Range(_camera.minPos.x + 1, _camera.maxPos.x - 1);
        float valueZ = Random.Range(_camera.minPos.y + 1, _camera.maxPos.y - 1);
        Vector3 newPos = new Vector3(valueX, 0.5f, valueZ);

        GameObject baseView = Instantiate(basePrefab, newPos, Quaternion.identity);
        yield return new WaitForSeconds(timeToAppear);

        GameObject enemy = Instantiate(enemyPrefab.gameObject, newPos, Quaternion.identity);
        Destroy(baseView.gameObject);
    }
}