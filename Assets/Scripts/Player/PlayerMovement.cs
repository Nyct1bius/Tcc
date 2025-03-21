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
    [SerializeField] private PlayerStatesController statesController;
    public Animator animator;
    [SerializeField] private Transform mainCameraRef;


    //Inputs
    float xInput;
    float yInput;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    Vector3 moveDirection;
    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldownTime;
    private bool dashInCooldown;

    [Header("Physics")]
    [Range(0f, 1f)]
    [SerializeField] private float gravity;
    [Range(0f, 1f)]
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
        idleState.Setup(body, animator,this);
        walkState.Setup(body, animator, this);
        jumpState.Setup(body, animator, this);
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
        ApplyGravity();
    }

    private void CheckInputs()
    {
        xInput = inputManager.InputDirection().x;
        yInput = inputManager.InputDirection().z;
    }
    private void SelectState()
    {
        if (Grounded())
        {
            if (inputManager.InputDirection() == Vector3.zero)
            {
                statesController.ChangePlayerState(idleState);
            }
            else
            {
                statesController.ChangePlayerState(walkState);
            }
        }    
    }
    private void HandleMovement()
    {
        if (inputManager.InputDirection() != Vector3.zero)
        {
            moveDirection = mainCameraRef.transform.forward * yInput + mainCameraRef.transform.right * xInput;
            moveDirection = new Vector3(moveDirection.x * acceleration, body.linearVelocity.y, moveDirection.z * acceleration);
            body.linearVelocity = moveDirection;
        }
    }
    private void HandleJump()
    {
        if (Grounded())
        {
            body.linearVelocity = Vector3.up * jumpForce;
            statesController.ChangePlayerState(jumpState);
        }
    }

    private void HandleDash()
    {
        if (!dashInCooldown)
        {
            Debug.Log("dashed");
            body.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            dashInCooldown = true;
            StartCoroutine(DashCoolingdown());
        }
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
        if(inputManager.InputDirection() != Vector3.zero)
        {
            targetAngle = Mathf.Atan2(xInput, yInput) * Mathf.Rad2Deg + mainCameraRef.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
           
    }

    private void ApplyGravity()
    {
        if (!Grounded())
        {
            body.linearVelocity -= Vector3.up * gravity;    
        }
    }

    private void ApplyFriction()
    {
        if (Grounded() && xInput == 0 && yInput == 0 && body.linearVelocity.y <= 0)
        {
            body.linearVelocity *= groundDecay;
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
