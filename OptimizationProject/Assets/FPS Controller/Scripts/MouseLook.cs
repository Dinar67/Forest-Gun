using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("PlayerRotate Properties")]
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] public float _sensitivity;
    [SerializeField] private float _rotationLimit;
    protected float vertRot;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        if (PlayerPrefs.HasKey("sensitivityMouse"))
            _sensitivity = PlayerPrefs.GetFloat("sensitivityMouse");
    }

    public virtual void Rotate()
    {
        vertRot -= GetVerticalValue();
        vertRot = vertRot <= -_rotationLimit ? -_rotationLimit :
                  vertRot >= _rotationLimit ? _rotationLimit :
                  vertRot;

        RotateVertical();
        RotateHorizontal();
    }

    protected float GetVerticalValue() => Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
    protected float GetHorizontalValue() => Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
    protected virtual void RotateVertical() => _cameraHolder.localRotation = Quaternion.Euler(vertRot, 0f, 0f);
    protected virtual void RotateHorizontal() => transform.Rotate(Vector3.up * GetHorizontalValue());
}
