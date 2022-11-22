using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_SpikeballBig : SS_SpikeballSmall
{
    [SerializeField] private GameObject firePoint1;
    [SerializeField] private GameObject firePoint2;
    [SerializeField] private GameObject firePoint3;
    [SerializeField] private GameObject firePoint4;

    protected override void Awake()
    {
        fireRate = 2f;
        base.Awake();
        fireAngle = 2f;
        rotationSpeed = 0.5f;
        speedMax = 5f;
        health = 300f;
        damage = 50f;
        Velocity = speedMax * Vector3.back;
    }

    protected override void Fire()
    {
        base.Fire();
        Shoot(firePoint1, firePoint1.transform.position - transform.position);
        Shoot(firePoint2, firePoint2.transform.position - transform.position);
        Shoot(firePoint3, firePoint3.transform.position - transform.position);
        Shoot(firePoint4, firePoint4.transform.position - transform.position);
    }
}
