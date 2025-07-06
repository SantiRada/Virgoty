using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponController : MonoBehaviour {

    [Header("Weapon Settings")]
    [SerializeField] private float distanceFromPlayer = 1f;
    [SerializeField] private float rotationSmooth = 20f;

    private Transform playerT;
    private SpriteRenderer sprite;
    private Camera cam;

    private void Awake()
    {
        playerT = transform.parent;
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }
    private void Update()
    {
        if (playerT == null || cam == null) return;

        Vector3 mouse = GetMouseWorldPos();

        // Position to Player
        Vector3 dirFromPlayer = (mouse - playerT.position).normalized;
        transform.position = playerT.position + dirFromPlayer * distanceFromPlayer;

        // Rotation to Mouse
        Vector3 dirFromWeapon = mouse - transform.position;
        float angle = Mathf.Atan2(dirFromWeapon.y, dirFromWeapon.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);

        // Flip to correct angle
        sprite.flipY = angle > 90f || angle < -90f;
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 screen = Mouse.current.position.ReadValue();
        screen.z = cam.WorldToScreenPoint(playerT.position).z;

        return cam.ScreenToWorldPoint(screen);
    }
}