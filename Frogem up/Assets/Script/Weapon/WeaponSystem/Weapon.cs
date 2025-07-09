using UnityEngine;

[CreateAssetMenu(menuName = "Weapon System/Weapon")]
public class Weapon : ScriptableObject {

    [Header("Specs")]
    [Tooltip("Cantidad total de balas del arma")] public int bulletAmount;
    [Tooltip("Tiempo entre balas")] public float timeBetweenShot;
    [Tooltip("Tiempo entre ráfagas")] public float timeBetweenBurst;
    [Tooltip("Tiempo desde que se cliquea el disparo hasta que sale la primera bala")] public float delayShot;
    [Tooltip("Tiempo que tarda en recargar")] public float timeToReload;

    [Header("Stats")]
    public int damage;
    [Tooltip("Velocidad de cada bala")] public float bulletSpeed;
    [Tooltip("Tiempo para quitar el debuff de dispersión")] public float timeToDisappearDispersion;
    [Tooltip("Cuantas balas se lanzan en un solo clic")] public int bulletPerClic;
    [Tooltip("De ser 0, las balas iran directo al mouse")] public float dispersion;

    [Header("Characteristics")]
    [Tooltip("Dispara manteniendo el clic o hay que cliquear para cada bala")] public bool hasHold = false;
    [Tooltip("Si permite crear una mira desde el arma al mouse")] public bool hasCrosshair = false;

    /*
    
    [Header("Content")]
    public GameObject model;
    public AudioClip audioBullet;
    public AudioClip audioReload;


    */
}