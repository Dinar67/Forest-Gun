using System;
using UnityEngine;
using Random = System.Random;


public class Weapon : MonoBehaviour
{


    public float _damage = 5f;
    public string _name;
    public int _bulletsCount = 1;
    public float _spread = 0.05f;
    public float _fireRate = 2f;
    public float _range = 15f;
    public float _force = 155f;
    public ParticleSystem _muzzleFlash;
    public GameObject _bloodSystem;
    public AudioClip _shotSFX;
    public AudioSource _audioSource;
    public Transform _bulletSpawn;
    public GameObject hitEffect;
    public int _enemyLayerId;
    private Random random;



    public Camera _cam;
    private float _nextFire = 0f;
    private Animator _shootAnim;
    private void Awake()
    {
        _shootAnim = GetComponent<Animator>();
        random = new Random();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && Time.time > _nextFire && Time.timeScale != 0.01f)
        {
            _nextFire = Time.time + 1f / _fireRate;
            Shoot();
        }

    }

    void Shoot()
    {
        _audioSource.PlayOneShot(_shotSFX);
        //Instantiate(_muzzleFlash, _bulletSpawn.position, _bulletSpawn.rotation);
        _muzzleFlash.Play();
        

        RaycastHit hit;
        for (int i = 0; i < _bulletsCount; i++)
        {
            _shootAnim.Play("Shoot");
            Vector3 vector3 = _cam.transform.forward;
            if (random.Next(0, 2) == 0)
                vector3.x -= (float)Math.Round(random.NextDouble() * (double)_spread, 1);
            else
                vector3.x += (float)Math.Round(random.NextDouble() * (double)_spread, 1);

            if (random.Next(0, 2) == 0)
                vector3.y -= (float)Math.Round(random.NextDouble() * (double)_spread, 1);
            else
                vector3.y += (float)Math.Round(random.NextDouble() * (double)_spread, 1);

            if (random.Next(0, 2) == 0)
                vector3.z -= (float)Math.Round(random.NextDouble() * (double)_spread, 1);
            else
                vector3.z += (float)Math.Round(random.NextDouble() * (double)_spread, 1);
            if (Physics.Raycast(_cam.transform.position, vector3, out hit, _range))
            {

                GameObject impact = Instantiate(hitEffect, hit.collider.gameObject.transform);
                impact.transform.position = hit.point;
                impact.transform.rotation = Quaternion.LookRotation(hit.normal);
                float x = impact.gameObject.transform.lossyScale.x / hitEffect.gameObject.transform.lossyScale.x;
                float y = impact.gameObject.transform.lossyScale.y / hitEffect.gameObject.transform.lossyScale.y;
                float z = impact.gameObject.transform.lossyScale.z / hitEffect.gameObject.transform.lossyScale.z;
                x *= 7;
                y *= 7;
                z *= 7;
                impact.gameObject.transform.localScale = new Vector3(1/x, 1/y, 1/z);
                Destroy(impact, 6f);


                if (hit.collider.gameObject.layer == _enemyLayerId)
                {
                    DamageTaker damageTaker = hit.collider.gameObject.GetComponent<DamageTaker>();
                    if (damageTaker != null)
                        damageTaker.TakeDamage(_damage);

                    if (i != 0)
                        continue;
                    GameObject blood = Instantiate(_bloodSystem, hit.point, Quaternion.LookRotation(hit.normal));
                    blood.GetComponent<ParticleSystem>().Play();
                    Destroy(blood, 1f);
                }

                if (hit.rigidbody != null)
                    hit.rigidbody.AddForce(-hit.normal * _force);

            }
        }
    }
}
