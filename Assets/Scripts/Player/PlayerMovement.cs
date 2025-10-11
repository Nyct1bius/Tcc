using System;
using System.Collections;
using UnityEngine;

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
    private bool _isGrounded;

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
    [SerializeField] private float _coyoteTime;
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
    private float _timeSinceUnground = 0f;


    [SerializeField] private float _jumpBufferTime = 0.1f;
    public float _timeSinceJumpPressed = Mathf.Infinity;

    [Header("Dash")]
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _dashTime = 1f;
    [SerializeField] private float _dashCooldownTime;
    public AnimationCurve DashSpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public LayerMask DashCollisionMask;
    private bool _isDashButtonPressed;
    private float _dashVelocity;
    private bool _dashInCooldown;

    [Header("Physics")]
    [Range(0f, -180f)]
    [SerializeField] private float _gravity;
    [Range(0f, 100f)]
    [SerializeField] private float groundDecay;
    [SerializeField] private float jumpSustainGravity = 0.4f;

    [Header("Rotation")]
    [SerializeField] private float turnSmoothTime;
    private float targetAngle;
    private float turnSmoothVelocity;
    float angle;

    [Header("Screen Shake Profiles")]
    [SerializeField] private ScreenShakeProfileSO _landProfile;
    [SerializeField] private ScreenShakeProfileSO _dashProfile;
    private bool _ungroudedDueToJump;

    #region Getters and Setters
    public Transform PlayerTransform { get { return transform; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public float Gravity { get { return _gravity; } }

    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float WalkAcceleration { get { return _walkAcceleration; } }
    public float MaxWalkSpeed { get { return _maxWalkSpeed; } }
    public Vector3 HorizontalVelocity { get { return _horizontalVelocity; } set { _horizontalVelocity = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public Vector3 CameraFowardXZ { get { return _cameraFowardXZ; } set { _cameraFowardXZ = value; } }
    public Vector3 CameraRightXZ { get { return _cameraRightXZ; } set { _cameraRightXZ = value; } }
    public Vector3 MovementDelta { get { return _movementDelta; } set { _movementDelta = value; } }

    public float DashTime { get { return _dashTime; } }
    public bool IsDashButtonPressed { get { return _isDashButtonPressed; } }
    public float DashVelocity { get { return _dashVelocity; } }
    public bool DashInCooldown { get { return _dashInCooldown; } set { _dashInCooldown = value; } }

    public bool UngroudedDueToJump { get { return _ungroudedDueToJump; } set { _ungroudedDueToJump = value; } }
    public float CoyoteTime { get { return _coyoteTime; } }
    public float TimeSinceUnground { get { return _timeSinceUnground; } }
    public bool IsJumpButtonPressed { get { return _isJumpButtonPressed; } set { _isJumpButtonPressed = value; } }
    public float ButtonPressedTime { get { return _buttonPressedTime; } set { _buttonPressedTime = value; } }
    public float JumpVelocity { get { return _jumpVelocity; } }
    public float MaxJumpTime { get { return _maxJumpTime; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public float ShortHopMultiplier { get { return _shortHopMultiplier; } }
    public float MaxFallTime { get { return _maxFallTime; } }
    public float FallDeathTimer { get { return _fallDeathTimer; } set { _fallDeathTimer = value; } }

    public ScreenShakeProfileSO LandProfile { get { return _landProfile; } }
    public ScreenShakeProfileSO DashProfile { get { return _dashProfile; } }

    // --- INPUT BUFFER ---
    public bool HasBufferedJump => _timeSinceJumpPressed <= _jumpBufferTime;
    #endregion

    private void Start()
    {
        SetUpJumpVariables();;
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
        if (_machine.GameIsPaused) return;
        _timeSinceJumpPressed += Time.deltaTime; 
        Debug.DrawRay(transform.position, Vector3.down * (transform.localScale.y * 0.5f + 0.3f), Color.red);
        UpdateFrictionMaterial();
        IsGroundAtLandingPoint();
        ApplyGravity();
        HandleGrounded();
    }

    private void FixedUpdate()
    {
        FaceInput();
        ApplyFinalVelocity();
        _timeSinceUnground += Time.deltaTime;
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
        _isJumpButtonPressed = isJumpButtonPressed;
        _requireNewJumpPress = false;

        if (isJumpButtonPressed)
        {
            _timeSinceJumpPressed = 0f;
        }
    }

    private void HandleGrounded()
    {
        if (_machine.GroundSensor.IsGrounded())
        {
            _ungroudedDueToJump = false;
            _timeSinceUnground = 0;
            _isGrounded = true;
            _verticalVelocity = 0f;
        }
        else
        {
            _isGrounded = false;
        }

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
        if (_currentMovementInput != Vector2.zero || _machine.IsBlocking)
        {
            if (_machine.IsBlocking)
            {
                Vector3 cameraForward = _machine.MainCameraRef.transform.forward;
                cameraForward.y = 0f; 
                cameraForward.Normalize();

                if (cameraForward.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime / turnSmoothTime
                    );
                }

                _moveDirection = transform.forward * _currentMovementInput.y +
                                 transform.right * _currentMovementInput.x;
            }
            else
            {
                targetAngle = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg +
                              _machine.MainCameraRef.transform.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
    }


    private void UpdateFrictionMaterial()
    {
        if (OnSlope() && _currentMovementInput == Vector2.zero)
        {
            _playerCollider.material = _highFrictionMaterial;
        }
        else
        {
            _playerCollider.material = _lowFrictionMaterial;
        }
    }

    private void ApplyGravity()
    {
        if (_isGrounded) return;

        var effectiveGravity = _gravity;
        var verticalSpeed = Vector3.Dot(_machine.Body.linearVelocity, transform.up);

        if (_isJumpButtonPressed && verticalSpeed > 0)
        {
            effectiveGravity *= jumpSustainGravity;
        }
        else if (!_isJumpButtonPressed && verticalSpeed > 0)
        {
            effectiveGravity *= 2f;
        }

        _machine.Body.linearVelocity += effectiveGravity * Time.deltaTime * transform.up;
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

    public bool IsGroundAtLandingPoint()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (_playerCollider.center.y - _playerCollider.height / 2 + 0.05f);
        float rayLength = (transform.localScale.y * 0.5f) + _groundDetectionDistance;

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.green);
        return Physics.Raycast(rayOrigin, Vector3.down, out _slopeHit, rayLength);
    }
    #endregion
}
