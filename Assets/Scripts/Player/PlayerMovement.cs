using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] PlayerStateMachine _machine;
    [SerializeField] private Transform playerVisualTransform;

    //Inputs
    private Vector2 _currentMovementInput;

    [Header("Movement")]
    [Range(0f, 50f)]
    [SerializeField] private float _walkAcceleration;
    [Range(0f, 10f)]
    [SerializeField] private float _maxWalkSpeed;
    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;
    private Vector3 _cameraFowardXZ;
    private Vector3 _cameraRightXZ;
    private Vector3 _moveDirection;
    private Vector3 _movementDelta;
    private Vector3 _newVelocity;

    [Header("Slope")]
    [Range(0f, 90f)]
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private PhysicsMaterial _highFrictionMaterial;
    [SerializeField] private PhysicsMaterial _lowFrictionMaterial;
    [SerializeField] private CapsuleCollider _playerCollider;
    private RaycastHit _slopeHit;



    [Header("Jump")]
    public float jumpHeight;
    [SerializeField] private float _maxJumpTime;
    [Range(0, 0.7f)]
    [SerializeField] private float _shortHopMultiplier;
    [SerializeField] private float _maxFallTime = 5f;
    [Range(0, 500f)]
    [SerializeField] private float _groundDetectionDistance;
    private float _fallDeathTimer;
    private float _jumpVelocity;
    private bool _isJumpButtonPressed;
    private float _buttonPressedTime;
    private bool _requireNewJumpPress;


    [Header("Dash")]
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _dashTime = 1f;
    [SerializeField] private float _dashCooldownTime;
    private bool _isDashButtonPressed;
    private float _dashVelocity;
    private bool _dashInCooldown;

    [Header("Physics")]
    [Range(0f, -50f)]
    [SerializeField] private float _gravity;
    [Range(0f, 100f)]
    [SerializeField] private float groundDecay;


    [Header("Rotation")]
    [SerializeField] private float turnSmoothTime;
    private float targetAngle;
    private float turnSmoothVelocity;
    float angle;

    #region Getters and Setters
    //COMPONENTS
    public Transform PlayerTransform { get { return transform; } }

    //Phisycs

    public float Gravity { get { return _gravity; } }


    //MOVEMENT
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float WalkAcceleration { get { return _walkAcceleration; } }
    public float MaxWalkSpeed { get { return _maxWalkSpeed; } }
    public Vector3 HorizontalVelocity { get { return _horizontalVelocity; } set { _horizontalVelocity = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public Vector3 CameraFowardXZ { get { return _cameraFowardXZ; } set { _cameraFowardXZ = value; } }
    public Vector3 CameraRightXZ { get { return _cameraRightXZ; } set { _cameraRightXZ = value; } }
    public Vector3 MovementDelta { get { return _movementDelta; } set { _movementDelta = value; } }

    //DASH
    public float DashTime { get { return _dashTime; } }
    public bool IsDashButtonPressed { get { return _isDashButtonPressed; } }
    public float DashVelocity { get { return _dashVelocity; } }
    public bool DashInCooldown { get { return _dashInCooldown; } set { _dashInCooldown = value; } }


    //JUMP
    public bool IsJumpButtonPressed { get { return _isJumpButtonPressed; } }
    public float ButtonPressedTime { get { return _buttonPressedTime; } set { _buttonPressedTime = value; } }
    public float JumpVelocity { get { return _jumpVelocity; } }
    public float MaxJumpTime { get { return _maxJumpTime; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public float ShortHopMultiplier { get { return _shortHopMultiplier; } }
    public float MaxFallTime { get { return _maxFallTime; } }
    public float FallDeathTimer { get { return _fallDeathTimer; } set { _fallDeathTimer = value; } }


    #endregion

    private void Start()
    {
        SetUpJumpVariables();
    }
    private void OnEnable()
    {
        _machine.inputReader.MoveEvent += SetUpMoveInput;
        _machine.inputReader.JumpEvent += OnjumpButton;
        _machine.inputReader.DashEvent += HandleDash;
    }

    private void OnDisable()
    {
        _machine.inputReader.MoveEvent -= SetUpMoveInput;
        _machine.inputReader.JumpEvent -= OnjumpButton;
        _machine.inputReader.DashEvent -= HandleDash;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * (transform.localScale.y * 0.5f + 0.3f), Color.red);
        UpdateFrictionMaterial();
        HasGround();
    }
    private void FixedUpdate()
    {
        if (!_machine.GameIsPaused)
        {
            FaceInput();
            ApplyGravity();
            ApplyFinalVelocity();
        }
    }

    #region Movement
    private void SetUpJumpVariables()
    {
        _jumpVelocity = MathF.Sqrt(jumpHeight * _gravity * -2) * _machine.Body.mass;
        _dashVelocity = MathF.Sqrt(_dashDistance * _gravity * -2) * _machine.Body.mass;

    }

    private void SetUpMoveInput(Vector2 inputDirection)
    {
        _currentMovementInput = inputDirection;
    }


    private void OnjumpButton(bool isJumpButtonPressed)
    {
        this._isJumpButtonPressed = isJumpButtonPressed;
        _requireNewJumpPress = false;

    }

    private void HandleDash(bool isDashing)
    {
        _isDashButtonPressed = isDashing;
    }
    public void ResetDash()
    {
        StartCoroutine(DashCoolingdown());
    }
    private IEnumerator DashCoolingdown()
    {
        yield return new WaitForSeconds(_dashCooldownTime);
        _dashInCooldown = false;
    }

    private void ApplyFinalVelocity()
    {
        _newVelocity = new Vector3(_horizontalVelocity.x, _verticalVelocity, _horizontalVelocity.z);
        ApplyFriction();
        _machine.Body.AddForce(_newVelocity, ForceMode.VelocityChange);
    }

    private void ApplyFriction()
    {
        _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, groundDecay * Time.deltaTime);
    }

    private void FaceInput()
    {
        if (_currentMovementInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg + _machine.MainCameraRef.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }
    private void UpdateFrictionMaterial()
    {
        if (OnSlope() && _currentMovementInput == Vector2.zero)
        {
            Debug.Log("On a Slope");
            _playerCollider.material = _highFrictionMaterial;
        }
        else
        {
            _playerCollider.material = _lowFrictionMaterial;
        }
    }
    private void ApplyGravity()
    {
        //_machine.Body.useGravity = OnSlope();
        //if (OnSlope()) return;

        if (_machine.Body.linearVelocity.y > 0)
        {
            _machine.Body.AddForce(Vector3.up * -0.1f, ForceMode.Force);

        }
        else
        {
            _machine.Body.AddForce(Vector3.up * _gravity, ForceMode.Force);
        }
    }

    public bool OnSlope()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (_playerCollider.center.y - _playerCollider.height / 2 + 0.05f);
        float rayLength = (transform.localScale.y * 0.5f) + 0.3f;

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

        if (Physics.Raycast(rayOrigin, Vector3.down, out _slopeHit, rayLength))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }

    public bool HasGround()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (_playerCollider.center.y - _playerCollider.height / 2 + 0.05f);
        float rayLength = (transform.localScale.y * 0.5f) + _groundDetectionDistance;

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.green);
        return Physics.Raycast(rayOrigin, Vector3.down, out _slopeHit, rayLength);
        

    }
    #endregion
}

