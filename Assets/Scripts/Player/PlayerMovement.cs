using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Rigidbody body;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerStatesController statesController;
    [SerializeField] private Transform playerVisualTransform;
    public Animator animator;
    private Camera mainCameraRef;
    private Vector3 playerVisualDefaultPos;

    //Inputs
    Vector3 currrentMovementInput;

    [Header("Movement")]
    [Range(0f, 50f)]
    [SerializeField] private float walkAcceleration;
    [Range(0f, 10f)]
    [SerializeField] private float maxWalkSpeed;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private Vector3 cameraFowardXZ;
    private Vector3 cameRightXZ;
    private Vector3 moveDirection;
    private Vector3 movementDelta;
    private Vector3 newVelocity;


    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float maxJumpTime;
    private float jumpVelocity;
    private bool isJumping;
    private bool isButtonPressed;
    private float buttonPressedTime;


    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldownTime;
    private float dashVelocity;
    private bool dashInCooldown;

    [Header("Physics")]
    [Range(0f, -50f)]
    [SerializeField] private float gravity;
    [Range(0f, 100f)]
    [SerializeField] private float groundDecay;


    [Header("States")]
    [SerializeField] private IdleState idleState;
    [SerializeField] private WalkState walkState;
    [SerializeField] private JumpState jumpState;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    [Header("Rotation")]
    private float targetAngle;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    float angle;

    private void Start()
    {
        idleState.Setup(animator,this);
        walkState.Setup(animator, this);
        jumpState.Setup(animator, this);
        mainCameraRef = Camera.main;
        playerVisualDefaultPos = playerVisualTransform.localPosition;
        SetUpJumpVariables();
    }
    private void OnEnable()
    {
        PlayerEvents.Jump += HandleJump;
        PlayerEvents.Dash += HandleDash;
    }


    private void OnDisable()
    {
        PlayerEvents.Jump -= HandleJump;
        PlayerEvents.Dash -= HandleDash;
    }
  
    private void Update()
    {
        playerVisualTransform.localPosition = playerVisualDefaultPos;
        CheckInputs();
        SelectState();
        FaceInput();
    }
    private void FixedUpdate()
    {
        HandleHorizontalMovement();
        ApplyFinalVelocity();
        CheckIfStillJumping();
        ApplyGravity();
    }

    private void CheckInputs()
    {
        currrentMovementInput.x = inputManager.InputDirection().x;
        currrentMovementInput.z = inputManager.InputDirection().y;
    }
    private void SelectState()
    {
        if (IsGrounded())
        {
            if (inputManager.InputDirection() == Vector2.zero)
            {
                statesController.ChangePlayerState(idleState);
            }
            else
            {
                statesController.ChangePlayerState(walkState);
            }
        }
        else
        {
            statesController.ChangePlayerState(jumpState);
        }
    }
    private void HandleHorizontalMovement()
    {
        if (inputManager.InputDirection() != Vector2.zero)
        {
            cameraFowardXZ = new Vector3(mainCameraRef.transform.forward.x, 0, mainCameraRef.transform.forward.z).normalized;
            cameRightXZ = new Vector3(mainCameraRef.transform.right.x, 0, mainCameraRef.transform.right.z).normalized;
            moveDirection = cameRightXZ * currrentMovementInput.x + cameraFowardXZ * currrentMovementInput.z;

            movementDelta = moveDirection * walkAcceleration * Time.deltaTime;
            horizontalVelocity += movementDelta;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxWalkSpeed);
        }
        else if (IsGrounded())
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, groundDecay * Time.deltaTime);
        }
    }
    private void SetUpJumpVariables()
    {
        jumpVelocity = MathF.Sqrt(jumpHeight * gravity * -2) * body.mass;
        dashVelocity = MathF.Sqrt(dashDistance * -groundDecay * -2) * body.mass;
    }
    private void HandleJump(bool isJumpButtonPressed)
    {
        Debug.Log(isJumpButtonPressed);
        isButtonPressed = isJumpButtonPressed;
        if (IsGrounded() && isJumpButtonPressed)
        {
            body.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            buttonPressedTime = 0;
            isJumping = true;
        }

    }
    private void CheckIfStillJumping()
    {
        if (isJumping)
        {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime < maxJumpTime && !isButtonPressed)
            {
                CancelJump();
            }

        }

    }
    private void CancelJump()
    {
        body.AddForce(Vector3.up * gravity, ForceMode.Force);
    }
    private void ApplyFinalVelocity()
    {
        newVelocity = new Vector3(horizontalVelocity.x, verticalVelocity , horizontalVelocity.z);
        ApplyFriction();
        body.AddForce(newVelocity,ForceMode.VelocityChange);
        ApplyGravity();
    }
    private void HandleDash()
    {
        if (!dashInCooldown && IsGrounded())
        {
            Debug.Log(dashVelocity);
            Vector3 dashDir = moveDirection == Vector3.zero ? transform.forward : moveDirection;
            body.AddForce(dashDir.normalized * dashVelocity, ForceMode.Impulse);
            dashInCooldown = true;
            StartCoroutine(DashCoolingdown());
        }
    }

    IEnumerator DashCoolingdown()
    {
        yield return new WaitForSeconds(dashCooldownTime);
        dashInCooldown = false;
    }



    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundMask);
    }

    private void FaceInput()
    {
        if(inputManager.InputDirection() != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(currrentMovementInput.x, currrentMovementInput.z) * Mathf.Rad2Deg + mainCameraRef.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerVisualTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerVisualTransform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
           
    }

    public void MovePlayerToAPoint(Vector3 movePosition)
    {
        horizontalVelocity = movePosition;
        verticalVelocity = movePosition.y;
    }

    private void ApplyGravity()
    {
        if (body.linearVelocity.y > 0)
        {
            body.AddForce(Vector3.up * -0.1f, ForceMode.Force);

        }
        else if(!IsGrounded())
        {
            isJumping = false;
            body.AddForce(Vector3.up * gravity, ForceMode.Force);
        }

    }

    private void ApplyFriction()
    {
       Vector3 currentDrag = newVelocity.normalized * groundDecay * Time.deltaTime;

        if(newVelocity.magnitude > groundDecay * Time.deltaTime)
        {
            newVelocity -= currentDrag;
        }
        else
        {
            newVelocity = Vector3.zero;
        }
    }

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
    #endregion
}
