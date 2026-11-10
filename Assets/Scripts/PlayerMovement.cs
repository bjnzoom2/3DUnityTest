using UnityEngine;

public enum PlayerMovementStates
{
    Grounded = 0,
    Airborne = 1
}

public enum SpecialPlayerMovementStates
{
    None = 0,
    Sprinting = 1,
    Crouching = 2
}

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private GameInput inputs;

    public PlayerMovementStates playerMovementState = new();
    public SpecialPlayerMovementStates specialPlayerMovementState = new();

    private Vector2 movementInput;
    private Vector3 movementVector;

    private float playerSpeed = 0.0f;
    private float sprintSpeed = 7.5f;
    private float baseSpeed = 4.0f;
    private float crouchSpeed = 1.75f;

    private Vector2 mouseLookDelta;
    private Vector3 mouseLookVelocity;
    private Vector3 mouseLookEuler;
    private float sensitivity = 70.0f;

    private float gravity = 14.0f;
    private float yVelocity = 0.0f;

    private float jumpPower = 8.0f;

    private float baseFOV = 80.0f;
    private float sprintFOV = 110.0f;
    private float targetFOV = 80.0f;
    private float FOVChangeVelocity = 0.0f;

    private float baseHeight = 2.0f;
    private float crouchHeight = 1.4f;
    private float targetCameraHeight = 2.0f;
    private float cameraHeight = 0.0f;
    private float cameraHeightChangeVelo = 0.0f;

    private float headBobTimer = 0.0f;
    private float headBobMagnitude = 0.05f;
    private float headBobRate = 2.5f;
    private Vector3 currentHeadBobOffset = new();
    private Vector3 targetHeadBobOffset = new();
    private Vector3 headBobChangeVelo = new();

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
        cameraHeight = baseHeight - 0.5f;
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            playerMovementState = PlayerMovementStates.Grounded;
        } else
        {
            playerMovementState = PlayerMovementStates.Airborne;
        }
        
        if (inputs.Player.Sprint.IsPressed())
        {
            specialPlayerMovementState = SpecialPlayerMovementStates.Sprinting;
        } else if (inputs.Player.Crouch.IsPressed())
        {
            specialPlayerMovementState = SpecialPlayerMovementStates.Crouching;
        } else
        {
            specialPlayerMovementState = SpecialPlayerMovementStates.None;
        }

        mouseLookDelta = inputs.Player.MouseLook.ReadValue<Vector2>();
        mouseLookVelocity = new Vector3(-mouseLookDelta.y, mouseLookDelta.x, 0.0f) * sensitivity;
        mouseLookEuler += mouseLookVelocity * Time.deltaTime;

        mouseLookEuler.x = Mathf.Clamp(mouseLookEuler.x, -89.0f, 89.0f);

        movementInput = inputs.Player.GroundMovement.ReadValue<Vector2>();

        if (specialPlayerMovementState == SpecialPlayerMovementStates.Sprinting && movementInput.magnitude != 0.0f)
        {
            targetFOV = sprintFOV;
        }
        else
        {
            targetFOV = baseFOV;
        }

        if (specialPlayerMovementState == SpecialPlayerMovementStates.Crouching)
        {
            controller.height = crouchHeight;
            controller.center = Vector3.up * (crouchHeight / 2.0f);
            targetCameraHeight = crouchHeight - 0.3f;
        } else
        {
            controller.height = baseHeight;
            controller.center = Vector3.up * (baseHeight / 2.0f);
            targetCameraHeight = baseHeight - 0.5f;
        }

        if (specialPlayerMovementState == SpecialPlayerMovementStates.Crouching)
        {
            playerSpeed = crouchSpeed;
            headBobMagnitude = 0.025f;
        } else if (specialPlayerMovementState == SpecialPlayerMovementStates.Sprinting)
        {
            playerSpeed = sprintSpeed;
            headBobMagnitude = 0.07f;
        } else
        {
            playerSpeed = baseSpeed;
            headBobMagnitude = 0.05f;
        }

        movementVector = Quaternion.Euler(new Vector3(0.0f, mouseLookEuler.y, 0.0f)) * new Vector3(movementInput.x, 0.0f, movementInput.y) * playerSpeed;

        if (playerMovementState == PlayerMovementStates.Grounded && movementVector.magnitude != 0.0f)
        {
            headBobTimer += Time.deltaTime * playerSpeed * headBobRate;
        } else
        {
            headBobTimer = 0.0f;
        }

        targetHeadBobOffset.x = Mathf.Sin(headBobTimer / 2.0f) * headBobMagnitude;
        targetHeadBobOffset.y = Mathf.Sin(headBobTimer) * headBobMagnitude;

        if (inputs.Player.Jump.WasPressedThisFrame())
        {
            if (playerMovementState == PlayerMovementStates.Grounded)
            {
                yVelocity = jumpPower;
                playerMovementState = PlayerMovementStates.Airborne;
            }
        }

        if (inputs.Player.Jump.WasReleasedThisFrame() && playerMovementState == PlayerMovementStates.Airborne && yVelocity > 0.0f)
        {
            yVelocity *= 0.6f;
        }

        if (playerMovementState == PlayerMovementStates.Grounded && yVelocity <= 0.0f)
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

        currentHeadBobOffset = Vector3.SmoothDamp(currentHeadBobOffset, targetHeadBobOffset, ref headBobChangeVelo, 0.1f);

        cameraHeight = Mathf.SmoothDamp(cameraHeight, targetCameraHeight, ref cameraHeightChangeVelo, 0.1f);
        Camera.main.transform.localPosition = Vector3.up * cameraHeight + currentHeadBobOffset;
    }
}
