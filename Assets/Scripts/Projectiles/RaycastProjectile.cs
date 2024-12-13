using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : Projectile
{
    public override void Launch()
    {
        base.Launch();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            ITakeDamage[] damageTaker = hit.collider.GetComponentsInChildren<ITakeDamage>();
            foreach (var taker in damageTaker)
            { 
                taker.TakeDamage(weapon, this, hit.point);
            }
        }
    }
}
