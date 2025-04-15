using System;
using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] Rigidbody _body;
    [SerializeField] Animator animator;
    [SerializeField] GroundSensor _groundSensor;
    public InputReader inputReader;
    [SerializeField] private Transform playerVisualTransform;
    private Camera _mainCameraRef;
    private Vector3 playerVisualDefaultPos;

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

    //State variables
    State _currentState;
    PlayerStateFactory _states;



    [Header("Jump")]
    public float jumpHeight;
    [SerializeField] private float _maxJumpTime;
    private float _jumpVelocity;
    private bool _isJumpButtonPressed;
    private float _buttonPressedTime;
    private bool _requireNewJumpPress;


    [Header("Dash")]
    private bool _isDashButtonPressed;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldownTime;
    private float dashVelocity;
    private bool dashInCooldown;

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
    
    
    [Header("Weapon")]
    [SerializeField] private LayerMask _damageableLayer;
    [SerializeField] private Transform _attackCollisionCheck;
    [SerializeField] private Transform _weaponPos;
    [SerializeField] private WeaponSO _currentWeaponData;
    private bool _hasWeapon;
    private GameObject _currentWeaponVisual;


    [Header("Combat")]
    [SerializeField] private bool _attackIncooldown;
    [SerializeField] private float _timeBetweenAttacks = 0.5f;
    private int _attackCount;
    private bool _isAttacking;

    [Header("Animations")]
    [SerializeField] private AnimationClip _jumpAnim;
    [SerializeField] private AnimationClip _idleAnim;
    [SerializeField] private AnimationClip _walkAnim;
    private int _dashAnimHash;

    #region Getters/Setters

    public State CurrentState { get { return _currentState; } set { _currentState = value; } }


    //COMPONENTS
    public Animator Animator { get { return animator; } }
    public Rigidbody Body { get { return _body; } }
    public GroundSensor GroundSensor { get { return _groundSensor; } }
    public Camera MainCameraRef {  get { return _mainCameraRef; } }
    
    public Transform PlayerTransform { get { return transform; } }

    //Phisycs

    public float Gravity { get {  return _gravity; } }

    //IDLE
    public AnimationClip IdleAnim { get { return _idleAnim; } }

    //MOVEMENT
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public AnimationClip WalkAnim { get { return _walkAnim; } }
    public float WalkAcceleration {  get { return _walkAcceleration; } }
    public float MaxWalkSpeed {  get { return _maxWalkSpeed; } }
    public Vector3 HorizontalVelocity {  get { return _horizontalVelocity; } set { _horizontalVelocity = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public Vector3 CameraFowardXZ { get { return _cameraFowardXZ; } set { _cameraFowardXZ = value; } }
    public Vector3 CameraRightXZ{ get { return _cameraRightXZ; } set { _cameraRightXZ = value; } }
    public Vector3 MovementDelta{ get { return _movementDelta; } set { _movementDelta = value; } }

    //JUMP
    public bool IsJumpButtonPressed { get { return _isJumpButtonPressed; }}

    public AnimationClip JumpAnim { get { return _jumpAnim; }}

    public float ButtonPressedTime { get { return _buttonPressedTime; } set { _buttonPressedTime = value; } }
    public float JumpVelocity {  get { return _jumpVelocity; }}
    public float MaxJumpTime {  get { return _maxJumpTime; }}
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; }}


    //COMBAT
    public int AttackCount {  get { return _attackCount; } set { _attackCount = value; } }
    public bool AttackIncooldown {  get { return _attackIncooldown; } set { _attackIncooldown = value; } }
    public float TimeBetweenAttacks {  get { return _timeBetweenAttacks; }}
    public bool IsAttacking {  get { return _isAttacking; }}
    public LayerMask DamageableLayer {  get { return _damageableLayer; }}
    public WeaponSO CurrentWeaponData {  get { return _currentWeaponData; }}


    #endregion

    private void Start()
    {

        //Handle States Start
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.Enter();
        
        //Get Components
        _mainCameraRef = Camera.main;
        playerVisualDefaultPos = playerVisualTransform.localPosition;
        
        SetUpJumpVariables();
        
    }


    private void OnEnable()
    {
        inputReader.MoveEvent += SetUpMoveInput;
        inputReader.JumpEvent += OnjumpButton;
        inputReader.DashEvent += HandleDash;
        inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
    }


    private void OnDisable()
    {
        inputReader.MoveEvent -= SetUpMoveInput;
        inputReader.JumpEvent -= OnjumpButton;
        inputReader.DashEvent -= HandleDash;
        inputReader.AttackEvent -= CheckAttackButton;
        PlayerEvents.SwordPickUp -= AddSword;
        PlayerEvents.AttackFinished -= HandleResetAttack;
    }
    private void Update()
    {     
        FaceInput();
        _currentState.UpdateStates();
    }
    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
        ApplyGravity();
        ApplyFinalVelocity();
    }
    #region Combat

    private void AddSword(WeaponSO currentWeapon)
    {
        if (_currentWeaponVisual != null)
        {
            Destroy(_currentWeaponVisual);
        }
        _currentWeaponData = currentWeapon;
        _hasWeapon = true;
        _currentWeaponVisual = Instantiate(_currentWeaponData.weaponVisual, _weaponPos);


    }

    private void CheckAttackButton(bool attacking)
    {
        _isAttacking = attacking;
    }

    public void HandleResetAttack()
    {
        StartCoroutine(WaitToResetAttack());
    }
    private IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(_timeBetweenAttacks);
        _attackIncooldown = false;
    }
    private void OnDrawGizmos()
    {
        if (_currentWeaponData != null)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(_attackCollisionCheck.position, _currentWeaponData.attackRange);
        }
    }

    #endregion

    #region Movement
    private void SetUpJumpVariables()
    {
        _jumpVelocity = MathF.Sqrt(jumpHeight * _gravity * -2) * _body.mass;
        dashVelocity = MathF.Sqrt(dashDistance * -groundDecay * -2) * _body.mass;
        
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

    private void HandleDash( bool isDashing)
    {
        _isDashButtonPressed = isDashing;
        if (!dashInCooldown && _groundSensor.IsGrounded())
        {
            Vector3 dashDir = _moveDirection == Vector3.zero ? transform.forward : _moveDirection;
            _body.AddForce(dashDir.normalized * dashVelocity, ForceMode.Impulse);
            dashInCooldown = true;
            StartCoroutine(DashCoolingdown());
        }
    }

    IEnumerator DashCoolingdown()
    {
        yield return new WaitForSeconds(dashCooldownTime);
        dashInCooldown = false;
    }

    private void ApplyFinalVelocity()
    {
        _newVelocity = new Vector3(_horizontalVelocity.x, _verticalVelocity, _horizontalVelocity.z);
        ApplyFriction();;
        _body.AddForce(_newVelocity, ForceMode.VelocityChange);
    }

    private void ApplyFriction()
    {
        _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, groundDecay * Time.deltaTime);
    }

    private void FaceInput()
    {
        if (_currentMovementInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg + _mainCameraRef.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

    }

    private void ApplyGravity()
    {
        if (_body.linearVelocity.y > 0)
        {
            _body.AddForce(Vector3.up * -0.1f, ForceMode.Force);

        }
        else
        {
            _body.AddForce(Vector3.up * _gravity, ForceMode.Force);
        }

    }
    #endregion
}
