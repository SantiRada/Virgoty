using UnityEngine;

[System.Serializable]
public class RadialShotSettings {

    [Header("Base Settings")]
    public int numberOfBullets = 5;
    public float bulletSpeed = 10f;
    public float cooldownAfterShot;

    [Header("Offsets")]
    [Range(-1f, 1f)] public float phaseOffset = 0f;
    [Range(-180f, 180f)] public float angleOffset = 0f;

    [Header("Mask")]
    public bool radialMask;
    [Range(0f, 360f)] public float maskAngle = 360f;
}