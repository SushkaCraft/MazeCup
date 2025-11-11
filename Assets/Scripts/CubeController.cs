using UnityEngine;

public enum CubeMode
{
    Pushable,
    Synchronized
}

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeMode _mode = CubeMode.Pushable;
    [SerializeField] private float _gridStep = 1f;
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _snapDistanceEpsilon = 0.01f;
    [SerializeField] private float _checkDistance = 1f;
    [SerializeField] private LayerMask _obstacleMask;

    private Vector3 _targetPosition;
    private Vector3 _currentVelocity;
    private bool _isMoving;

    private void Awake()
    {
        _targetPosition = transform.position;
        _currentVelocity = Vector3.zero;
    }

    private void Update()
    {
        MoveToTarget();
    }

    public bool TryMove(Vector3 direction, bool forced = false)
    {
        if (_isMoving && !forced)
            return false;

        if (CanMove(direction))
        {
            _targetPosition = transform.position + direction * _gridStep;
            _isMoving = true;
            return true;
        }

        return false;
    }

    private void MoveToTarget()
    {
        if (!_isMoving)
            return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _smoothTime, _maxSpeed, Time.deltaTime);

        if (Vector3.SqrMagnitude(_targetPosition - transform.position) <= _snapDistanceEpsilon * _snapDistanceEpsilon)
        {
            transform.position = _targetPosition;
            _currentVelocity = Vector3.zero;
            
            SnapToGrid();

            _isMoving = false;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3 origin = transform.position;
        origin.y += 0.5f;

        bool hit = Physics.Raycast(origin, direction, _checkDistance, _obstacleMask);
        Debug.DrawRay(origin, direction * _checkDistance, hit ? Color.red : Color.green, 0.1f);

        return !hit;
    }

    private void SnapToGrid()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Round(position.x * 2) / 2;
        position.z = Mathf.Round(position.z * 2) / 2;

        transform.position = position;
    }

    public CubeMode Mode => _mode;
}
