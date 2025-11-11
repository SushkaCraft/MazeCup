using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _restartAction;

    [SerializeField] private float _gridStep = 1f;
    [SerializeField] private float _inputDeadzone = 0.5f;
    [SerializeField] private float _smoothTime = 0.075f;
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _snapDistanceEpsilon = 0.01f;
    [SerializeField] private float _checkDistance = 1f;
    [SerializeField] private LayerMask _obstacleMask;

    [SerializeField] private SceneLoader _sceneLoader;

    private Vector3 _targetPosition;
    private Vector3 _currentVelocity;
    private bool _isMoving;

    private void OnEnable()
    {
        _moveAction?.action.Enable();
        _restartAction?.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.action.Disable();
        _restartAction?.action.Disable();
    }

    private void Awake()
    {
        _targetPosition = transform.position;
        _currentVelocity = Vector3.zero;
        _isMoving = false;
    }

    private void Update()
    {
        ProcessInput();
        MoveToTarget();
        SceneReloadHandler();
    }

    private void ProcessInput()
    {
        if (_isMoving || _moveAction == null) return;

        Vector2 raw = _moveAction.action.ReadValue<Vector2>();
        float x = Mathf.Abs(raw.x) > _inputDeadzone ? Mathf.Sign(raw.x) : 0f;
        float y = Mathf.Abs(raw.y) > _inputDeadzone ? Mathf.Sign(raw.y) : 0f;

        if (x == 0f && y == 0f) return;

        Vector3 direction;
        if (Mathf.Abs(x) > Mathf.Abs(y))
            direction = new Vector3(x, 0f, 0f);
        else
            direction = new Vector3(0f, 0f, y);

        if (CanMove(direction))
        {
            _targetPosition = transform.position + direction * _gridStep;
            _isMoving = true;
            TryMoveSynchronizedObjects(direction);
        }
    }

    private void MoveToTarget()
    {
        if (!_isMoving) return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _smoothTime, _maxSpeed, Time.deltaTime);

        if (Vector3.SqrMagnitude(_targetPosition - transform.position) <= _snapDistanceEpsilon * _snapDistanceEpsilon)
        {
            transform.position = _targetPosition;
            _currentVelocity = Vector3.zero;
            _isMoving = false;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3 origin = transform.position;
        origin.y += 0.5f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, _checkDistance, _obstacleMask))
        {
            if (hit.collider.TryGetComponent<CubeController>(out var cube))
            {
                if (cube.Mode == CubeMode.Pushable)
                    return cube.TryMove(direction);
                else if (cube.Mode == CubeMode.Synchronized)
                    return cube.TryMove(direction, true);
            }
            return false;
        }
        return true;
    }

    private void TryMoveSynchronizedObjects(Vector3 direction)
    {
        CubeController[] cubes = FindObjectsByType<CubeController>(FindObjectsSortMode.None);
        foreach (var cube in cubes)
        {
            if (cube.Mode == CubeMode.Synchronized)
                cube.TryMove(direction, true);
        }
    }

    private void SceneReloadHandler()
    {
        if (_restartAction != null && _restartAction.action.WasPressedThisFrame())
        {
            _sceneLoader.RestartScene();
        }
        
    }
}
