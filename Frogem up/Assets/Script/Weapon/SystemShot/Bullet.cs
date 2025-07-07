using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Dictionary<string, int> data;

    private const float MAX_LIFE_TIME = 3f;
    private float _lifeTime = 0f;

    [HideInInspector] public Vector3 velocity;

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        _lifeTime += Time.deltaTime;

        if (_lifeTime > MAX_LIFE_TIME) Disable();
    }
    private void Disable()
    {
        _lifeTime = 0f;
        data = null;

        gameObject.SetActive(false);
    }
}
