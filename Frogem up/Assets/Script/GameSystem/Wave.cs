using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Wave")]
public class Wave : ScriptableObject {

    [Header("Time")]
    public bool isForTime = false;
    public float timeToWave = 0;

    [Header("Enemies")]
    public SpawnerEnemy[] enemies;
}

[Serializable]
public class SpawnerEnemy : MonoBehaviour {
    
    [Header("Appear Data")]
    public EnemyBase enemyPrefab;
    [Tooltip("Sprite de X en el suelo")] public GameObject basePrefab;
    public float timeToAppear;

    [Header("Load Stats")]
    public float timerToInitial;
    public float timeBetweenEnemy;
    public float timeBetweenWaves;
    [Space]
    public int totalEnemies;
    public Vector2 minMaxPerWaves;
}