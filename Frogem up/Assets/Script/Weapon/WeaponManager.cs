using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour {

    [SerializeField] private float _shootCooldown;
    [SerializeField] private RadialShotSettings _shotSettings;

    private float _shootCooldownTimer = 0f;
    private bool canShot = false;

    private void Awake()
    {
        _shootCooldownTimer = _shootCooldown;
    }
    private void Update()
    {
        _shootCooldownTimer -= Time.deltaTime;
        if(_shootCooldownTimer < 0f) canShot = true;

        bool isPressed = Mouse.current.leftButton.isPressed;

        if (isPressed && canShot)
        {
            canShot = false;
            _shootCooldownTimer = _shootCooldown;
            ShotAttack.SimpleShot(transform.position, transform.right * _shotSettings.bulletSpeed);
        }
    }
}