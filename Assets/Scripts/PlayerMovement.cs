using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private GameInput inputs;

    private Vector2 movementInput;
    private Vector3 movementVector;
    private float playerSpeed = 7.5f;

    private Vector2 mouseLookDelta;
    private Vector3 mouseLookVelocity;
    private Vector3 mouseLookEuler;
    private float sensitivity = 40.0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputs = new();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseLookDelta = inputs.Player.MouseLook.ReadValue<Vector2>();
        mouseLookVelocity = new Vector3(0.0f, mouseLookDelta.x, 0.0f) * sensitivity;
        mouseLookEuler += mouseLookVelocity * Time.deltaTime;

        movementInput = inputs.Player.GroundMovement.ReadValue<Vector2>();
        movementVector = Quaternion.Euler(mouseLookEuler) * new Vector3(movementInput.x, 0.0f, movementInput.y) * playerSpeed;

        controller.Move(movementVector * Time.deltaTime);
        transform.eulerAngles = mouseLookEuler;
    }
}
