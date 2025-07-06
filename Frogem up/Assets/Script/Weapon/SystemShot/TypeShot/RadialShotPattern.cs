using UnityEngine;

[CreateAssetMenu(menuName = "BulletHell System/Radial Shot Pattern")]
public class RadialShotPattern : ScriptableObject {

    public int repetitions;
    [Range(-180f, 180f)] public float angleOffsetBetweenReps;
    public float startWait = 0f;
    public float endWait = 0f;
    public RadialShotSettings[] patternSettings;
}