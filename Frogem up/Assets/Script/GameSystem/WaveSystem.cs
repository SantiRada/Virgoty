using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour {

    public Wave[] waves;
    private int currentWave = 0;

    public GameObject spawners;
    private ImprovementSystem improvements;

    private List<Spawners> listSpawners = new List<Spawners>();

    // Eventos
    private int enemyCount;
    private float timer;
    private bool inWait = false;

    private void Start()
    {
        listSpawners = new List<Spawners>();
        listSpawners.AddRange(FindObjectsOfType<Spawners>());

        improvements = GetComponentInChildren<ImprovementSystem>();
        improvements.gameObject.SetActive(false);

        LaunchWave();
    }
    private void Update()
    {
        if (inWait) return;
        if (currentWave < waves.Length) if (!waves[currentWave].isForTime) return;

        if(timer > 0) { timer -= Time.deltaTime; }
        else { TestNewWave(); }
    }
    public void LaunchWave()
    {
        improvements.gameObject.SetActive(false);
        currentWave++;
        timer = waves[currentWave].timeToWave;

        // Crear enemigos de la siguiente Wave
        for (int j = 0; j < waves[currentWave].enemies.Length; j++)
        {
            enemyCount += waves[currentWave].enemies[j].totalEnemies;

            if (j >= listSpawners.Count)
            {
                Spawners newSpawn = spawners.AddComponent<Spawners>();
                newSpawn.spawnEnemy = waves[currentWave].enemies[j];
                listSpawners.Add(newSpawn);
            }
            else
            {
                listSpawners[j].spawnEnemy = waves[currentWave].enemies[j];
            }
        }

        inWait = false;
    }
    private void TestNewWave()
    {
        inWait = true;

        if (currentWave >= waves.Length) FinishLevel();
        else LaunchImprovements();
    }
    private void FinishLevel()
    {
        // Mostrar pantalla de "SIGUIENTE NIVEL"
    }
    private void LaunchImprovements()
    {
        improvements.gameObject.SetActive(true);
        improvements.Roll();
    }
    public void KillEnemy()
    {
        enemyCount--;

        if (enemyCount <= 0) TestNewWave();
    }
}