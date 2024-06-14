using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    private CharacterController _player;

    public Transform _groundCheck;
    public LayerMask _groundMask;
    public float _gravity = -20f;
    public float _jumpHeight = 2f;
    Vector3 velocity;
    public float _groundDistance = 0.4f;
    bool _isGrounded;

    private void Awake() => _player = GetComponent<CharacterController>();

    public void Move()
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
        _player.Move(move * _speed * Time.deltaTime);


        velocity.y += _gravity * Time.deltaTime; //y = g * t^2 Закон гравитации
        _player.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftControl))
            _player.height = 1f;
        else
        {
            if (_player.height < 2)
                _player.height += 0.1f;
            else
                _player.height = 2f;

        }
    }
}