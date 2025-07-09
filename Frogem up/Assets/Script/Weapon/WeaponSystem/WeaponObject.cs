using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponObject : MonoBehaviour {

    public Weapon obj;

    // Private Content
    private PlayerMovement _player;

    [Header("Copy to Weapons")]
    [HideInInspector] public bool isPressed;
    [HideInInspector] public float timeBetweenShot;
    [HideInInspector] public float timeBetweenBurst;
    [HideInInspector] public float timeToDisappearDispersion;
    [HideInInspector] public int totalBullets;
    [HideInInspector] public bool inHold = false;
    [HideInInspector] public bool canShot = true;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public bool hasWaitTime = false;

    private Dictionary<string, int> data = new Dictionary<string, int>();

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>();
        timeBetweenBurst = obj.timeBetweenBurst;
        timeBetweenShot = obj.timeBetweenShot;
        totalBullets = obj.bulletAmount;
    }
    private void Start()
    {
        canShot = true;
        timeBetweenShot = 0;

        _player.inputActions.Player.Shot.performed += OnShot;
        _player.inputActions.Player.Shot.canceled += OnUpShot;

        data.Add("Player", obj.damage);
    }
    private void OnShot(InputAction.CallbackContext context) { isPressed = true; }
    private void OnUpShot(InputAction.CallbackContext context)
    {
        if (!obj.hasHold) inHold = false;

        isPressed = false;
    }
    private void Update()
    {
        // Disminuye el tiempo de perdida de dispersión hasta 0
        if(obj.dispersion > 0f)
        {
            if (timeToDisappearDispersion > 0f) timeToDisappearDispersion -= Time.deltaTime;
            else timeToDisappearDispersion = 0f;
        }

        // Si existe, reducir el tiempo entre ráfagas siempre que sea mayor a 0
        if (timeBetweenBurst > 0f) timeBetweenBurst -= Time.deltaTime;
        else timeBetweenBurst = 0f;

        // Revisa el tiempo entre balas para poder o no disparar
        if (obj.timeBetweenShot > 0)
        {
            hasWaitTime = true;
            canShot = false;
            timeBetweenShot -= Time.deltaTime;
            if (timeBetweenShot < 0f && timeBetweenBurst <= 0f)
            {
                hasWaitTime = false;
                canShot = true;
            }
        }

        if (isPressed && canShot)
        {
            // Revisa si tiene balas antes de avanzar, en caso de que no, recarga
            if(totalBullets <= 0 && !isReloading)
            {
                StartCoroutine(Reload());
                return;
            }

            // Revisa si puede disparar antes de hacerlo
            if (canShot && !isReloading && !hasWaitTime)
            {
                if (!obj.hasHold && inHold) return;

                if (obj.bulletPerClic > 1)
                {
                    if(obj.timeBetweenBurst > 0f)
                    {
                        // Disparo de ráfaga => Varias balas con tiempo entre sí => Y tiempo entre ráfagas
                        timeBetweenBurst = obj.timeBetweenBurst;
                        StartCoroutine(MultipleShot());
                    }
                    else
                    {
                        // Disparo de escopeta => Varias balas sin tiempo entre sí
                        for (int i = 0; i < obj.bulletPerClic; i++) { Shot(i, true); }
                        totalBullets--;
                    }
                }
                else { Shot(); }
            }

            // Detiene el disparo en spray si el arma no lo permite
            if (!obj.hasHold) canShot = false;
        }
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        canShot = false;

        yield return new WaitForSeconds(obj.timeToReload);

        totalBullets = obj.bulletAmount;
        isReloading = false;
        canShot = true;
    }
    private Vector3 CalculateDirBullet(int numBullet)
    {
        Vector3 direction = (transform.forward * obj.bulletSpeed);

        if(obj.dispersion > 0f)
        {
            if(timeToDisappearDispersion > 0f)
            {
                float dispersion = obj.dispersion;
                if(numBullet > 0) { dispersion *= numBullet; }

                float rnd = Random.Range(-dispersion, dispersion);

                direction = new Vector3(direction.x + rnd, direction.y, direction.z + rnd);
            }
        }

        return direction;
    }
    private IEnumerator MultipleShot()
    {
        for(int i = 0; i < obj.bulletPerClic; i++)
        {
            Shot(i / 10);
            yield return new WaitForSeconds(obj.timeBetweenShot);
        }
    }
    private void Shot(int numBullet = 0, bool isShotgun = false)
    {
        ShotAttack.SimpleShot(transform.position, CalculateDirBullet(numBullet), data);
        inHold = true;

        if (!isShotgun) totalBullets--;

        timeBetweenShot = obj.timeBetweenShot;

        // Modificar el tiempo de dispersión si existe en el arma
        if(obj.dispersion > 0f) timeToDisappearDispersion = obj.timeToDisappearDispersion;
    }
}