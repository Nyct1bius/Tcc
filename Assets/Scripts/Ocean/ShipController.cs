using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private ShipSettings _shipSettings;
    [SerializeField] private LayerMask islandLayer;

    private Rigidbody _body;
    private Vector2 _moveDir;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += GetInputDirection;
        _inputReader.InteractEvent += Interact;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= GetInputDirection;
        _inputReader.InteractEvent -= Interact;
    }

    private void FixedUpdate()
    {
        HandleShipMovement();
        LimitRotation();
    }

    private void GetInputDirection(Vector2 inputDir)
    {
        _moveDir = inputDir.normalized;
    }

    private void HandleShipMovement()
    {
        HandleRotation();

        if (_moveDir == Vector2.zero) return;
        if (_body.linearVelocity.z <= 0 && _moveDir.y < 0) return;

        Vector3 forwardMovement = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        _body.AddForce(_moveDir.y * forwardMovement * _shipSettings.moveSpeed, ForceMode.Acceleration);

        if (_body.linearVelocity.magnitude > _shipSettings.maxSpeed)
            _body.linearVelocity = Vector3.ClampMagnitude(_body.linearVelocity, _shipSettings.maxSpeed);
    }

    private void HandleRotation()
    {
        float inputX = _moveDir.x;

        if (Mathf.Abs(inputX) > 0.01f)
        {
            transform.Rotate(Vector3.up, inputX * _shipSettings.turnSpeed * Time.fixedDeltaTime);
            if (Mathf.Abs(_moveDir.y) < 0.01f) return;
            float targetZ = -inputX * _shipSettings.tiltAngle;
            Vector3 currentRot = transform.localEulerAngles;
            currentRot.z = Mathf.LerpAngle(NormalizeAngle(currentRot.z), targetZ, Time.fixedDeltaTime * 2f);
            transform.localEulerAngles = new Vector3(currentRot.x, currentRot.y, currentRot.z);
        }
        else
        {
            Quaternion targetRot = Quaternion.Euler(0f, NormalizeAngle(transform.localEulerAngles.y), 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.fixedDeltaTime * _shipSettings.autoCenterSpeed);
        }
    }

    private void LimitRotation()
    {
        Vector3 rot = transform.localEulerAngles;
        rot.z = Mathf.Clamp(NormalizeAngle(rot.z), -_shipSettings.maxTiltAngle, _shipSettings.maxTiltAngle);
        rot.x = Mathf.Clamp(NormalizeAngle(rot.x), -_shipSettings.maxTiltAngle, _shipSettings.maxTiltAngle);

        transform.localEulerAngles = rot;
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
    private void Interact()
    {
       foreach(Collider collider in checkForIslands())
        {
            collider.GetComponent<IslandManager>()?.LoadLevel();
        }
    }   

    private Collider[] checkForIslands()
    {
        float checkRadius = 0.5f;   
        return Physics.OverlapSphere(
            transform.position,
            checkRadius,
            islandLayer
        );
    }
}

[Serializable]
public class ShipSettings
{
    public float moveSpeed = 5f;
    public float turnSpeed = 60f;
    public float maxSpeed = 10f;
    public float maxTiltAngle = 20f;     
    public float tiltAngle = 10f;        
    public float autoCenterSpeed = 2f;   
}
