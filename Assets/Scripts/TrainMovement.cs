using Fusion;
using UnityEngine;

public class TrainMovement : NetworkBehaviour
{
   public Vector2 moveDirection = Vector2.right; // Only forward!
    public float maxSpeed = 2f;
    public float acceleration = 2f;
    public float deceleration = 3f;
    public float distanceToTravel = 9f;

    private float _currentSpeed = 0f;
    private float _distanceTravelled = 0f;
    private bool _isMoving = true;
    private bool _isDecelerating = false;
    private Vector3 _lastPosition;
    public Vector2 CurrentVelocity => moveDirection * _currentSpeed;


    private void Start()
    {
        moveDirection = moveDirection.normalized;
        _lastPosition = transform.position;
    }

    public override void FixedUpdateNetwork()
    {
        if (!_isMoving) return;

        float remainingDistance = distanceToTravel - _distanceTravelled;
        float stopDistance = (deceleration > 0)
            ? (_currentSpeed * _currentSpeed) / (2f * deceleration)
            : 0f;

        // Start decelerating if needed
        if (!_isDecelerating && remainingDistance <= stopDistance)
            _isDecelerating = true;

        // Accelerate or decelerate
        if (_isDecelerating && deceleration > 0)
            _currentSpeed -= deceleration * Runner.DeltaTime;
        else if (_currentSpeed < maxSpeed)
            _currentSpeed += acceleration * Runner.DeltaTime;

        // Clamp speed
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);

        // Move
        Vector3 moveStep = (Vector3)(moveDirection * _currentSpeed * Runner.DeltaTime);
        transform.position += moveStep;

        // Update distance travelled
        _distanceTravelled += (transform.position - _lastPosition).magnitude;
        _lastPosition = transform.position;

        // Stop if reached distance or speed is zero
        if (_distanceTravelled >= distanceToTravel || _currentSpeed <= 0.01f)
            _isMoving = false;
    }
}
