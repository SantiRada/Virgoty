using UnityEngine;

public static class Vector2Extensions {

    public static Vector3 Rotate(this Vector3 originalVector, float rotateAngleInDegrees)
    {
        Quaternion rotation = Quaternion.AngleAxis(rotateAngleInDegrees, Vector3.right);
        return rotation * originalVector;
    }
}