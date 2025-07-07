using UnityEngine;

public class RadialShotPatternVisualizer : MonoBehaviour {

    [SerializeField] private RadialShotPattern _pattern;
    [SerializeField] private float _radius;
    [SerializeField] private Color _color;
    [SerializeField, Range(0f, 5f)] private float _testTime;

    private void OnDrawGizmos()
    {
        if (_pattern == null) return;

        Gizmos.color = _color;
        int lap = 0;

        Vector3 aimDirection = transform.forward;
        Vector3 center = transform.position;
        float timer = _testTime;

        while (timer > 0f && lap < _pattern.repetitions)
        {
            if (lap > 0 && _pattern.angleOffsetBetweenReps != 0f)
                aimDirection = RotateVectorXZ(aimDirection, _pattern.angleOffsetBetweenReps);

            for (int i = 0; i < _pattern.patternSettings.Length; i++)
            {
                if (timer < 0) break;
                DrawRadialShot(_pattern.patternSettings[i], timer, aimDirection);
                timer -= _pattern.patternSettings[i].cooldownAfterShot;
            }
            lap++;
        }
    }
    private void DrawRadialShot(RadialShotSettings settings, float lifeTime, Vector3 aimDirection)
    {
        float angleBetweenBullets = 360f / settings.numberOfBullets;

        if (settings.angleOffset != 0 || settings.phaseOffset != 0f)
            aimDirection = RotateVectorXZ(aimDirection, settings.angleOffset + (settings.phaseOffset * angleBetweenBullets));

        for (int i = 0; i < settings.numberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;
            if (settings.radialMask && bulletDirectionAngle > settings.maskAngle) break;

            Vector3 bulletDirection = RotateVectorXZ(aimDirection, bulletDirectionAngle);

            Vector3 bulletPosition = transform.position + (bulletDirection * settings.bulletSpeed * lifeTime);

            Gizmos.DrawSphere(bulletPosition, _radius);
        }
    }
    private Vector3 RotateVectorXZ(Vector3 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector3(
            vector.x * cos - vector.z * sin,
            vector.y,
            vector.x * sin + vector.z * cos
        );
    }
}