using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float radius = 20f;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _minDamage;
    [SerializeField] private float _maxDamage;
    [SerializeField] private HealthControl _healthControl;
    private NavMeshAgent _enemyNav;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private Animator _animator;
    private bool isAttacking = false;
    private Animator _playerCameraHolderAnimator;

    private void Awake()
    {
        _enemyNav = GetComponent<NavMeshAgent>();
        _playerCameraHolderAnimator = GameObject.FindGameObjectsWithTag("CameraHolder").First().GetComponent<Animator>();
    }
    private float _counter = 0f;
    void Update()
    {
        if (_counter > 0.5f)
        {
            _counter = 0f;
            DetectMainCharacter();
        }
        else
            _counter += Time.deltaTime;
    }

    private void DetectMainCharacter()
    {
        bool isSearched = false;
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, _attackRadius);
        foreach (Collider collider in playerColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                Quaternion rotTarget = Quaternion.LookRotation(collider.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, 5f);
                if (!isAttacking)
                    StartCoroutine(AttackPlayer());
                isSearched = true;
                break;
            }
        }
        if(!isSearched)
            _animator.SetBool("isAttack", false);
        if (!isAttacking)
        {
            
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    _enemyNav.SetDestination(collider.transform.position);
                    break;
                }
            }
        }
       
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        _animator.applyRootMotion = true;
        _animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
        
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, _attackRadius);
        foreach (Collider collider in playerColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                _playerCameraHolderAnimator.Play("Shake");
                DamageTaker damageTaker = collider.GetComponent<DamageTaker>();
                if (damageTaker != null && _minDamage != 0 && _maxDamage != 0 && _minDamage < _maxDamage)
                {
                    float damage = _minDamage + (new System.Random().Next(0, Convert.ToInt32(Math.Round((_maxDamage - _minDamage) * 10, 0)))) / 10;
                    damageTaker.TakeDamage(damage);
                    Debug.Log(damage.ToString("N1"));
                }
            }
            
        }
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        if(this.enabled && _healthControl.Health != 0)
         _animator.applyRootMotion = false;
    }
}
