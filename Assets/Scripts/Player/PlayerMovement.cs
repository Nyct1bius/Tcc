using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private InputManager inputManager;
    private CharacterController characterController;
    [SerializeField] private PlayerStatesController statesController;
    public Animator animator;
    private Camera mainCameraRef;


    //Inputs
    Vector3 currrentMovementInput;

    [Header("Movement")]
    [Range(0f, 100f)]
    [SerializeField] private float walkAcceleration;
    [Range(0f, 100f)]
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float jumpForce;
    Vector3 cameraFowardXZ;
    Vector3 cameRightXZ;
    Vector3 moveDirection;
    Vector3 movementDelta;
    Vector3 newVelocity;





    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldownTime;
    private bool dashInCooldown;

    [Header("Physics")]
    [Range(-0f, -10f)]
    [SerializeField] private float gravity;
    [Range(-0f, -10f)]
    [SerializeField] private float groundGravaty;
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
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        idleState.Setup( animator,this);
        walkState.Setup(animator, this);
        jumpState.Setup(animator, this);
        mainCameraRef = Camera.main;
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
        CheckInputs();
        SelectState();
        FaceInput();
    }
    private void FixedUpdate()
    {
        HandleMovement();
        ApplyFriction();
        //ApplyGravity();

        if (inputManager.InputDirection() != Vector2.zero)
        {
            characterController.Move(newVelocity * Time.deltaTime);
        }
       
    }

    private void CheckInputs()
    {
        currrentMovementInput.x = inputManager.InputDirection().x;
        currrentMovementInput.z = inputManager.InputDirection().y;
    }
    private void SelectState()
    {
        if (characterController.isGrounded)
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
    private void HandleMovement()
    {
        if (inputManager.InputDirection() != Vector2.zero)
        {
            cameraFowardXZ = new Vector3(mainCameraRef.transform.forward.x, 0, mainCameraRef.transform.forward.z).normalized;
            cameRightXZ = new Vector3(mainCameraRef.transform.right.x, 0, mainCameraRef.transform.right.z).normalized;
            moveDirection = cameRightXZ * currrentMovementInput.x + cameraFowardXZ * currrentMovementInput.z;

            // Move delta is the amount that the player walks in that frame
            movementDelta = moveDirection * walkAcceleration * Time.deltaTime;
            // Its aplied twice the acceleration due to the Kinematic movement formula 
            newVelocity = characterController.velocity + movementDelta;

            ApplyFriction();

            newVelocity = Vector3.ClampMagnitude(newVelocity, maxWalkSpeed);

        }
    }
    private void HandleJump()
    {
        if (Grounded())
        {
            //body.linearVelocity = Vector3.up * jumpForce;
            //statesController.ChangePlayerState(jumpState);
        }
    }

    private void HandleDash()
    {
        //if (!dashInCooldown)
        //{
        //    Debug.Log("dashed");
        //    body.AddForce(moveDirection * dashForce, ForceMode.Impulse);
        //    dashInCooldown = true;
        //    StartCoroutine(DashCoolingdown());
        //}
    }

    IEnumerator DashCoolingdown()
    {
        yield return new WaitForSeconds(dashCooldownTime);
        dashInCooldown = false;
    }



    public bool Grounded()
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

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            moveDirection.y = groundGravaty;    
        }
        else
        {
            moveDirection.y += gravity;
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
