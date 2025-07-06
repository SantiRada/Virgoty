using System.Collections;
using UnityEngine;

public class RadialShotWeapon : MonoBehaviour
{

    [SerializeField] private RadialShotPattern _shotPattern;

    private bool _onShotPattern = false;

    private void Update()
    {
        if (_onShotPattern) return;

        StartCoroutine(ExecuteRadialShotPattern(_shotPattern));
    }
    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
    {
        _onShotPattern = true;
        int lap = 0;
        Vector2 aimDirection = transform.up;
        Vector2 center = transform.position;

        yield return new WaitForSeconds(pattern.startWait);

        while (lap < pattern.repetitions)
        {
            if (lap > 0 && pattern.angleOffsetBetweenReps != 0f) aimDirection = aimDirection.Rotate(pattern.angleOffsetBetweenReps);

            for (int i = 0; i < pattern.patternSettings.Length; i++)
            {
                ShotAttack.RadialShot(center, aimDirection, pattern.patternSettings[i]);
                yield return new WaitForSeconds(pattern.patternSettings[i].cooldownAfterShot);
            }
            lap++;
        }

        yield return new WaitForSeconds(pattern.endWait);

        _onShotPattern = false;
    }
}