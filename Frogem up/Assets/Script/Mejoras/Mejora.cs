using UnityEngine;

public enum Stats { life, speed, shield }
[CreateAssetMenu(menuName = "Improvement System/Improvement")]
public class Improvement : ScriptableObject {

    [Header("Content")]
    public string nameItem;
    public string descItem;
    public Sprite icon;

    [Header("Weapon")]
    public bool isWeapon = false;
    public Weapon weaponView;

    [Header("Stats")]
    public Stats modifyStats;
    public float newValue;
}