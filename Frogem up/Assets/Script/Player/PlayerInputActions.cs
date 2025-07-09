using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : System.IDisposable {

    private InputActionAsset inputAsset;
    private InputActionMap playerMap;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shotAction;

    public PlayerInputActions()
    {
        // Crear el Input Action Asset
        inputAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        inputAsset.name = "PlayerInputActions";

        // Crear el Action Map
        playerMap = inputAsset.AddActionMap("Player");

        // Crear la acción de movimiento con composite binding
        moveAction = playerMap.AddAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Agregar soporte para gamepad
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Gamepad>/leftStick/up")
            .With("Down", "<Gamepad>/leftStick/down")
            .With("Left", "<Gamepad>/leftStick/left")
            .With("Right", "<Gamepad>/leftStick/right");

        // Crear la acción de salto
        jumpAction = playerMap.AddAction("Jump", InputActionType.Button);
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/buttonSouth");

        // Crear la acción de disparar
        shotAction = playerMap.AddAction("Shot", InputActionType.Button);
        shotAction.AddBinding("<Mouse>/leftButton");
        shotAction.AddBinding("<Gamepad>/rightTrigger");
        shotAction.AddBinding("<Gamepad>/rightShoulder");

        // Habilitar el action map
        playerMap.Enable();
    }
    public PlayerActions Player => new PlayerActions(this);
    public void Dispose()
    {
        playerMap?.Disable();
        if (inputAsset != null)
        {
            UnityEngine.Object.DestroyImmediate(inputAsset);
        }
    }
    public struct PlayerActions
    {
        private PlayerInputActions wrapper;

        public PlayerActions(PlayerInputActions wrapper)
        {
            this.wrapper = wrapper;
        }

        public InputAction Move => wrapper.moveAction;
        public InputAction Jump => wrapper.jumpAction;
        public InputAction Shot => wrapper.shotAction;

        public void Enable() => wrapper.playerMap.Enable();
        public void Disable() => wrapper.playerMap.Disable();
    }
}