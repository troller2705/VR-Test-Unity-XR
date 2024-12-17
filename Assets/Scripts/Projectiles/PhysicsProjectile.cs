using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PhysicsProjectile : Projectile
{
    [SerializeField] private float lifeTime;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        Destroy(gameObject, lifeTime);
    }

    public override void Launch()
    {
        base.Launch();
        rigidBody.AddRelativeForce(Vector3.forward * weapon.GetShootingForce(), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Weapon"))
        {
            //Debug.Log($"Bullet collision detected with {other.gameObject.name}");
            ITakeDamage[] damageTakers = other.gameObject.GetComponentsInChildren<ITakeDamage>();

            foreach (var taker in damageTakers)
            {
                taker.TakeDamage(weapon, this, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
