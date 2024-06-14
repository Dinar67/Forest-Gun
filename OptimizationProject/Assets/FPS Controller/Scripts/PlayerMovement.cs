using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _characterController;
    public Transform _groundCheck;
    public LayerMask _groundMask;
    public float _speed = 6f;
    public float _gravity = -20f;
    public float _jumpHeight = 2f;
    Vector3 velocity;
    public float _groundDistance = 0.4f;
    bool _isGrounded;

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (_isGrounded && velocity.y < 0)
            velocity.y = -2f; 

        if (_isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        //Формула прыжка = Корень(сила прыжка * -2 * гравитация)

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _speed *= 2;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _speed /= 2;
        Vector3 move = transform.right * x + transform.forward * z;
        _characterController.Move(move * _speed * Time.deltaTime);


        velocity.y += _gravity * Time.deltaTime; //y = g * t^2 Закон гравитации
        _characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftControl))
            _characterController.height = 1f;
        else
        {
            if (_characterController.height < 2)
                _characterController.height += 0.1f;
            else
                _characterController.height = 2f;

        }
    }
}
