using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    [SerializeField] private float _health;
    private float _maxHealth;
    [SerializeField] private Animator _animator;
    [SerializeField] private Inventory _playerInventory;
    private Vector3 previousPosition;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private Image _healthBar;

    [NonSerialized] public EnemySpawn _enemySpawn;
    private void Start()
    {
        _maxHealth = _health;
        GameObject gameObject = Camera.main.gameObject;
        if(gameObject != null)
            _playerInventory = gameObject.GetComponent<Inventory>();
        
    }
    public float Health 
    { 
        get 
        { 
            return _health; 
        } 
        set 
        { 
            if(name == "Player")
                _healthBar.fillAmount = (Health / (_maxHealth / 100))/100;
            if (value <= 0 && name != "Player")
            {

                if(_health != 0 && _playerInventory != null)
                    _playerInventory.Money += new System.Random().Next(1, 3);
                _health = 0;
                Animator animator;
                if (gameObject.CompareTag("Target"))
                    animator = gameObject.GetComponentInParent<Animator>();
                else
                    animator = gameObject.GetComponent<Animator>();
                if (name != "target")
                {
                    if(_enemySpawn != null)
                        _enemySpawn._enemies.Remove(this.gameObject.transform.parent.gameObject);
                    int dieVariant = new System.Random().Next(1, 3);
                    if (_attack != null)
                        _attack.enabled = false;
                    if (animator != null)
                    {
                        animator.applyRootMotion = true;
                        animator.SetBool("isDead" + $"{dieVariant}", true);
                    }
                    if (_agent != null)
                        _agent.enabled = false;

                }
                else
                    animator.SetBool("isDead", true);
                

                EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
                if (enemyAttack != null)
                    enemyAttack.enabled = false;
                Destroy(gameObject, 10f);
                return;
            }
            else if(value < 0 && name == "Player")
            {
                _health = 0;
                _playerInventory.SaveInventory();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            _health = value;
        } 
    }
    
    private void Update()
    {
        if (_animator == null || _agent == null)
            return;

        if (_agent.velocity.magnitude > 1)
        {
            // Объект движется
            _animator.SetBool("isRun", true);
            
        }
        else
        {
            // Объект не движется
            _animator.SetBool("isRun", false);
        }
    }

}
