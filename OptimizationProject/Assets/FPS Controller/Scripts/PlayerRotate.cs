using System;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("PlayerRotate Properties")]
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] public float _sensitivity;
    [SerializeField] private float _rotationLimit;

    protected float vertRot;

    public virtual void Rotate()
    {
        vertRot -= GetVerticalValue();
        vertRot = Mathf.Clamp(vertRot, -_rotationLimit, _rotationLimit);

        RotateVertical();
        RotateHorizontal();
    }

    protected float GetVerticalValue() => Input.GetAxis("Mouse Y") * (_sensitivity + 100) * 1.5f * Time.deltaTime;
    protected float GetHorizontalValue() => Input.GetAxis("Mouse X") * _sensitivity * 1.5f * Time.deltaTime;
    protected virtual void RotateVertical() => _cameraHolder.localRotation = Quaternion.Euler(Mathf.Clamp(vertRot * 1.5f, -_rotationLimit, _rotationLimit), 0f, 0f);
    protected virtual void RotateHorizontal() => transform.Rotate(Vector3.up * GetHorizontalValue());
}
