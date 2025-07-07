using System.Collections.Generic;
using UnityEngine;

public static class ShotAttack {

    public static void SimpleShot(Vector3 origin, Vector3 velocity, Dictionary<string, int> data)
    {
        Bullet bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.velocity = velocity;

        bullet.data = data;
    }
    public static void RadialShot (Vector3 origin, Vector3 aimDirection, RadialShotSettings settings, Dictionary<string, int> data)
    {
        float angleBetweenBullets = 360f / settings.numberOfBullets;
        
        if (settings.angleOffset != 0 || settings.phaseOffset != 0f)
            aimDirection = aimDirection.Rotate(settings.angleOffset + (settings.phaseOffset * angleBetweenBullets));

        for (int i = 0; i < settings.numberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;

            if (settings.radialMask && bulletDirectionAngle > settings.maskAngle) break;

            Vector3 bulletDirection = RotateVectorXZ(aimDirection, bulletDirectionAngle);
            SimpleShot(origin, bulletDirection * settings.bulletSpeed, data);
        }
    }
    private static Vector3 RotateVectorXZ(Vector3 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector3(
            vector.x * cos - vector.z * sin,
            0,
            vector.x * sin + vector.z * cos
        );
    }
}