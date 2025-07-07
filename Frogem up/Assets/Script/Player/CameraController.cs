using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Limits")]
    public Vector2 minPos;
    public Vector2 maxPos;

    [Header("Settings")]
    public float smoothSpeed = 5f;
    public float mouseInfluenceDistance = 3f;
    public float cameraOffset = 2f;

    private PlayerMovement _player;

    private void Awake() { _player = FindObjectOfType<PlayerMovement>(); }
    private void LateUpdate()
    {
        if (_player == null) return;

        Vector3 playerPos = _player.transform.position;
        Vector3 mouseWorld = _player.GetMouseWorldPosition();

        Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.z);
        Vector2 mousePos2D = new Vector2(mouseWorld.x, mouseWorld.z);
        float distanceToMouse = Vector2.Distance(playerPos2D, mousePos2D);

        Vector3 desiredPos;

        if (distanceToMouse <= mouseInfluenceDistance) { desiredPos = playerPos; }
        else
        {
            Vector2 directionToMouse = (mousePos2D - playerPos2D).normalized;
            Vector2 offsetPos = playerPos2D + directionToMouse * cameraOffset;
            desiredPos = new Vector3(offsetPos.x, playerPos.y, offsetPos.y);
        }

        float clampedX = Mathf.Clamp(desiredPos.x, minPos.x, maxPos.x);
        float clampedZ = Mathf.Clamp(desiredPos.z, minPos.y, maxPos.y);

        Vector3 finalTarget = new Vector3(clampedX, transform.position.y, clampedZ - 3);

        transform.position = Vector3.Lerp(transform.position, finalTarget, smoothSpeed * Time.deltaTime);
    }
}