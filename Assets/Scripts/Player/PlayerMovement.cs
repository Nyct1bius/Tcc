using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerStatesController statesController;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform mainCameraRef;


    //Inputs
    float xInput;
    float yInput;

    [Header("Movement States")]
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

    private void Start()
    {
        idleState.Setup(body, animator,inputManager);
        walkState.Setup(body, animator, inputManager);
        jumpState.Setup(body, animator, inputManager);
    }
    private void OnEnable()
    {
        PlayerEvents.Jump += HandleJump;
    }
    private void OnDisable()
    {
        PlayerEvents.Jump -= HandleJump;
    }

    private void Update()
    {
        CheckInputs();
        SelectState();
    }
    private void FixedUpdate()
    {
        FaceInput();
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
    private void HandleJump()
    {
        if (Grounded())
        {
            statesController.ChangePlayerState(jumpState);
        }
    }

    private bool Grounded()
    {
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundMask);
    }

    private void FaceInput()
    {
        if(inputManager.InputDirection() != Vector3.zero)
        {
            targetAngle = Mathf.Atan2(xInput, yInput) * Mathf.Rad2Deg + mainCameraRef.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
