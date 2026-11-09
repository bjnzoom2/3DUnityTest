using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private GameInput inputs;

    private Vector2 movementInput;
    private Vector3 movementVector;

    private float playerSpeed = 0.0f;
    private float sprintSpeed = 9.0f;
    private float baseSpeed = 4.5f;

    private Vector2 mouseLookDelta;
    private Vector3 mouseLookVelocity;
    private Vector3 mouseLookEuler;
    private float sensitivity = 70.0f;

    private float gravity = 14.0f;
    private float yVelocity = 0.0f;
    private bool isGrounded = false;

    private float jumpPower = 8.0f;

    private bool sprintActivated = false;

    private float baseFOV = 80.0f;
    private float sprintFOV = 110.0f;
    private float targetFOV = 80.0f;
    private float FOVChangeVelocity = 0.0f;

    private bool crouchActivated = false;

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

        playerSpeed = baseSpeed;
        Camera.main.fieldOfView = baseFOV;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        sprintActivated = inputs.Player.Sprint.IsPressed();
        crouchActivated = inputs.Player.Sprint.IsPressed();

        mouseLookDelta = inputs.Player.MouseLook.ReadValue<Vector2>();
        mouseLookVelocity = new Vector3(-mouseLookDelta.y, mouseLookDelta.x, 0.0f) * sensitivity;
        mouseLookEuler += mouseLookVelocity * Time.deltaTime;

        mouseLookEuler.x = Mathf.Clamp(mouseLookEuler.x, -89.0f, 89.0f);

        movementInput = inputs.Player.GroundMovement.ReadValue<Vector2>();

        if (sprintActivated && movementInput.magnitude != 0.0f)
        {
            targetFOV = sprintFOV;
            playerSpeed = sprintSpeed;
        }
        else
        {
            targetFOV = baseFOV;
            playerSpeed = baseSpeed;
        }

        movementVector = Quaternion.Euler(new Vector3(0.0f, mouseLookEuler.y, 0.0f)) * new Vector3(movementInput.x, 0.0f, movementInput.y) * playerSpeed;

        if (inputs.Player.Jump.WasPressedThisFrame())
        {
            if (isGrounded)
            {
                yVelocity = jumpPower;
            }
        }

        if (isGrounded && yVelocity <= 0.0f)
        {
            yVelocity = -1.0f;
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }

        controller.Move((movementVector + Vector3.up * yVelocity) * Time.deltaTime);
        transform.eulerAngles = new Vector3(0.0f, mouseLookEuler.y, 0.0f);
        Camera.main.transform.eulerAngles = new Vector3(mouseLookEuler.x, mouseLookEuler.y, 0.0f);
        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, targetFOV, ref FOVChangeVelocity, 0.15f);
    }
}
