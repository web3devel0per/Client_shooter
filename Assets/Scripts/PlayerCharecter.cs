using System;
using UnityEngine;

public class PlayerCharecter : Character
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;
    [SerializeField] private float _jumpNewForce = 5;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDelay = .2f;
    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }
    private void Move()
    {
        //Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed; 

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        velocity.y = _rigidBody.linearVelocity.y;
        _velocity = velocity;
        _rigidBody.linearVelocity = _velocity;
    }

    private void RotateY()
    {
        _rigidBody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidBody.linearVelocity;

        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    internal void Jump()
    {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;

        _jumpTime = Time.time;
        _rigidBody.AddForce(0, _jumpNewForce, 0, ForceMode.VelocityChange);
    }
}
