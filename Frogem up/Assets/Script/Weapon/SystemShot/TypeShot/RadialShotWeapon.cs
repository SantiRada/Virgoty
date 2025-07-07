using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialShotWeapon : MonoBehaviour {

    [SerializeField] private RadialShotPattern _shotPattern;
    private bool _onShotPattern = false;

    private EnemyBase _enemy;

    private void Awake() { _enemy = GetComponent<EnemyBase>(); }
    private void Update()
    {
        if (_onShotPattern) return;

        if (_enemy.canAttack) { StartCoroutine(ExecuteRadialShotPattern(_shotPattern)); }
        else { StopCoroutine(ExecuteRadialShotPattern(_shotPattern)); }
    }
    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
    {
        _onShotPattern = true;
        int lap = 0;
        Vector3 aimDirection = transform.forward;

        yield return new WaitForSeconds(pattern.startWait);

        while (lap < pattern.repetitions)
        {
            if (lap > 0 && pattern.angleOffsetBetweenReps != 0f)
                aimDirection = RotateVectorXZ(aimDirection, pattern.angleOffsetBetweenReps);

            for (int i = 0; i < pattern.patternSettings.Length; i++)
            {
                Vector3 currentCenter = transform.position;

                ExecuteRadialShot(currentCenter, aimDirection, pattern.patternSettings[i]);

                yield return new WaitForSeconds(pattern.patternSettings[i].cooldownAfterShot);
            }
            lap++;
        }

        yield return new WaitForSeconds(pattern.endWait);
        _onShotPattern = false;
    }
    private void ExecuteRadialShot(Vector3 center, Vector3 aimDirection, RadialShotSettings settings)
    {
        float angleBetweenBullets = 360f / settings.numberOfBullets;

        Vector3 adjustedAimDirection = aimDirection;
        if (settings.angleOffset != 0 || settings.phaseOffset != 0f)
        {
            float totalOffset = settings.angleOffset + (settings.phaseOffset * angleBetweenBullets);
            adjustedAimDirection = RotateVectorXZ(aimDirection, totalOffset);
        }

        for (int i = 0; i < settings.numberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;

            if (settings.radialMask && bulletDirectionAngle > settings.maskAngle) break;

            Vector3 bulletDirection = RotateVectorXZ(adjustedAimDirection, bulletDirectionAngle);

            CreateBullet(center, bulletDirection, settings);
        }
    }
    private void CreateBullet(Vector3 position, Vector3 direction, RadialShotSettings settings)
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        data.Add(_enemy.nameOwner, _enemy.damage);
        ShotAttack.RadialShot(position, direction, settings, data);
    }
    private Vector3 RotateVectorXZ(Vector3 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector3(
            vector.x * cos - vector.z * sin,
            vector.y + 0.5f,
            vector.x * sin + vector.z * cos
        );
    }
}