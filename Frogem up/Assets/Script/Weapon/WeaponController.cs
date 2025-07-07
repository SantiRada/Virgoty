using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour {

    [Header("Weapon Settings")]
    [SerializeField] private float distanceFromPlayer = 1f;
    [SerializeField] private float rotationSmooth = 20f;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private LayerMask groundLayer = -1;

    private Transform playerT;
    private Renderer weaponRenderer;
    private Camera cam;

    private void Awake()
    {
        playerT = transform.parent;
        weaponRenderer = GetComponent<Renderer>();
        cam = Camera.main;
    }
    private void Update()
    {
        if (playerT == null || cam == null) return;

        Vector3 targetPoint = GetMouseWorldPos3D();

        Vector3 dirFromPlayer = (targetPoint - playerT.position).normalized;
        transform.position = playerT.position + dirFromPlayer * distanceFromPlayer;

        Vector3 dirFromWeapon = targetPoint - transform.position;
        dirFromWeapon.y = 0f;

        if (dirFromWeapon.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirFromWeapon, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
        }
    }
    private Vector3 GetMouseWorldPos3D()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
            return hit.point;

        Plane playerPlane = new Plane(Vector3.up, playerT.position);
        if (playerPlane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return ray.GetPoint(10f);
    }
}