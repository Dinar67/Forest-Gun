
using System;
using UnityEngine;
using Random = System.Random;

public class Test : MonoBehaviour
{
    public float _spread = 0.2f;
    private float _nextFire;
    public float _fireRate = 1f;
    private Random random = new Random();
    Vector3 vector3 = new Vector3();

    void Update()
    {
        
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + 1f / _fireRate;
            vector3 = transform.forward;
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

            
        }
        Debug.DrawRay(transform.position, vector3, Color.red);
    }
}
