using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour {

    public string nameOwner = "Player";
    public int damage = 3;

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
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add(nameOwner, damage);
            ShotAttack.SimpleShot(transform.position, transform.forward * _shotSettings.bulletSpeed, data);
        }
    }
}