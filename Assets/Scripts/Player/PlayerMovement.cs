using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerStatesController statesController;
    [SerializeField] private Transform playerVisualTransform;
    public Animator animator;
    private Camera mainCameraRef;
    private Vector3 playerVisualDefaultPos;

    //Inputs
    Vector3 currrentMovementInput;

    [Header("Movement")]
    [Range(0f, 100f)]
    [SerializeField] private float walkAcceleration;
    [Range(0f, 100f)]
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


    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldownTime;
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
        GetGravity();
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
        float jumpTimeToPeak = maxJumpTime / 2f;
        jumpVelocity = 2f * jumpHeight / jumpTimeToPeak;
    }
    private void HandleJump()
    {
        if (IsGrounded())
        {
            verticalVelocity = jumpVelocity;
        }
        if (moveDirection != Vector3.zero)
        {
            horizontalVelocity = moveDirection.normalized * walkAcceleration * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = transform.forward * walkAcceleration * Time.deltaTime;
        }
    }
    private void ApplyFinalVelocity()
    {
        newVelocity = new Vector3(horizontalVelocity.x, verticalVelocity , horizontalVelocity.z);
        ApplyFriction();
        characterController.Move(newVelocity * Time.deltaTime);
        GetGravity();
    }
    private void HandleDash()
    {
        if (!dashInCooldown)
        {
            Debug.Log("dashed");
            Vector3 dashDir = moveDirection == Vector3.zero ? transform.forward : moveDirection;
            horizontalVelocity += dashDir.normalized * dashForce;
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
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
           
    }

    private void GetGravity()
    {
        if (!IsGrounded())
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else 
        {
            verticalVelocity = -2f;
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
