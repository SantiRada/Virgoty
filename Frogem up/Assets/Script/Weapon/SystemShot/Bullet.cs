using UnityEngine;

public class Bullet : MonoBehaviour {

    private const float MAX_LIFE_TIME = 3f;
    private float _lifeTime = 0f;

    public Vector2 velocity;

    private void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
        _lifeTime += Time.deltaTime;

        if (_lifeTime > MAX_LIFE_TIME) Disable();
    }
    private void Disable()
    {
        _lifeTime = 0f;
        gameObject.SetActive(false);
    }
}
