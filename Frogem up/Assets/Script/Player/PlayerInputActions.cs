using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : System.IDisposable {

    private InputActionAsset inputAsset;
    private InputActionMap playerMap;
    private InputAction moveAction;

    public PlayerInputActions()
    {
        // Crear el Input Action Asset
        inputAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        inputAsset.name = "PlayerInputActions";

        // Crear el Action Map
        playerMap = inputAsset.AddActionMap("Player");

        // Crear la acción de movimiento con composite binding
        moveAction = playerMap.AddAction("Move", InputActionType.Value);

        // Configurar el composite binding para 2D Vector
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }
    public PlayerActions Player => new PlayerActions(this);
    public void Dispose()
    {
        inputAsset?.Disable();
        if (inputAsset != null)
            UnityEngine.Object.DestroyImmediate(inputAsset);
    }
    public struct PlayerActions
    {
        private PlayerInputActions wrapper;

        public PlayerActions(PlayerInputActions wrapper)
        {
            this.wrapper = wrapper;
        }

        public InputAction Move => wrapper.moveAction;

        public void Enable() => wrapper.playerMap.Enable();
        public void Disable() => wrapper.playerMap.Disable();
    }
}