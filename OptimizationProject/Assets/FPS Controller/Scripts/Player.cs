using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(PlayerMove), typeof(PlayerRotate))]
public class Player : MonoBehaviour
{
    private PlayerMove _move;
    private PlayerRotate _rotate;
    private PlayerRotate _rotateSmooth;
    private PlayerRotate _currentRotate;
    private Inventory _playerInventory;

    private void Awake()
    {
        _move = GetComponent<PlayerMove>();
        _rotate = GetComponents<PlayerRotate>()[0];
        _rotateSmooth = GetComponents<PlayerRotate>()[1];
        Cursor.lockState = CursorLockMode.Locked;

//#if UNITY_EDITOR
//        _currentRotate = _rotate;
//#else
        _currentRotate = _rotateSmooth;
//#endif
    }
    private void Start()
    {
        Time.timeScale = 1;
        GameObject gameObject = Camera.main.gameObject;
        if (gameObject != null)
            _playerInventory = gameObject.GetComponent<Inventory>();
    }

    private void Update()
    {
        _move.Move();
        _currentRotate.Rotate();
        if(gameObject.transform.position.y < -20)
        {
            _playerInventory.SaveInventory();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}