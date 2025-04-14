using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : Core
{
    [Header("Componets")]
    public InputReader inputReader;
    [SerializeField] private Transform playerVisualTransform;
    private Camera mainCameraRef;
    private Vector3 playerVisualDefaultPos;

    //Inputs
    public Vector2 CurrrentMovementInput { get; private set; }
    bool isAttacking;

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
    public float jumpHeight;
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
    [SerializeField] private DashState dashState;

    [Header("Rotation")]
    private float targetAngle;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    float angle;

    private void Start()
    {
        SetupInstances();
        mainCameraRef = Camera.main;
        playerVisualDefaultPos = playerVisualTransform.localPosition;
        SetUpJumpVariables();
        machine = new StateMachine();
        machine.Set(idleState);
    }
    private void OnEnable()
    {
        inputReader.MoveEvent += SetUpMoveInput;
        inputReader.JumpEvent += HandleJump;
        inputReader.DashEvent += HandleDash;
    }


    private void OnDisable()
    {
        inputReader.MoveEvent -= SetUpMoveInput;
        inputReader.JumpEvent -= HandleJump;
        inputReader.DashEvent -= HandleDash;
    }


    private void Update()
    {
        playerVisualTransform.localPosition = playerVisualDefaultPos;
        SelectState();
        FaceInput();
        machine.state.Do();
    }
    private void FixedUpdate()
    {
        machine.state.FixedDo();
        ApplyFinalVelocity();
        ApplyGravity();
    }

    private void SelectState()
    {
        if (groundSensor.IsGrounded())
        {
            if(CurrrentMovementInput == Vector2.zero)
            {
              // machine.Set(idleState);
            }
           
        }
        else
        {
            machine.Set(jumpState);
        }
    }
    private void SetUpMoveInput(Vector2 inputDirection)
    {
        if(inputDirection != Vector2.zero || groundSensor.IsGrounded())
        {
            machine.Set(walkState);
        }

        CurrrentMovementInput = inputDirection;
    }
    public void HandleHorizontalMovement()
    {
        cameraFowardXZ = new Vector3(mainCameraRef.transform.forward.x, 0, mainCameraRef.transform.forward.z).normalized;
        cameRightXZ = new Vector3(mainCameraRef.transform.right.x, 0, mainCameraRef.transform.right.z).normalized;
        moveDirection = cameRightXZ * CurrrentMovementInput.x + cameraFowardXZ * CurrrentMovementInput.y;

        movementDelta = moveDirection * walkAcceleration;
        horizontalVelocity += movementDelta;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxWalkSpeed);

    }
    private void SetUpJumpVariables()
    {
        jumpVelocity = MathF.Sqrt(jumpHeight * gravity * -2) * body.mass;
        dashVelocity = MathF.Sqrt(dashDistance * -groundDecay * -2) * body.mass;
    }
    private void HandleJump(bool isJumpButtonPressed)
    {
        isButtonPressed = isJumpButtonPressed;
        if (groundSensor.IsGrounded() && isJumpButtonPressed)
        {
            body.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            buttonPressedTime = 0;
            isJumping = true;
        }

    }
    public void CheckIfStillJumping()
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
        newVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        ApplyFriction();
        body.AddForce(newVelocity, ForceMode.VelocityChange);
    }



    private void HandleDash()
    {
        if (!dashInCooldown && groundSensor.IsGrounded())
        {
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


    private void FaceInput()
    {
        if (CurrrentMovementInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(CurrrentMovementInput.x, CurrrentMovementInput.y) * Mathf.Rad2Deg + mainCameraRef.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerVisualTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerVisualTransform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }

    private void ApplyGravity()
    {
        if (body.linearVelocity.y > 0)
        {
            body.AddForce(Vector3.up * -0.1f, ForceMode.Force);

        }
        else if (!groundSensor.IsGrounded())
        {
            isJumping = false;
            body.AddForce(Vector3.up * gravity, ForceMode.Force);
        }

    }

    private void ApplyFriction()
    {
        if (groundSensor.IsGrounded())
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, groundDecay * Time.deltaTime);
        }
    }
}

