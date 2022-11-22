using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_SpikeballState : SS_State
{
    private float deathFloor;
    private Vector3 rotationSpeed;
    private float fireRate;
    private float fireTimer = 0;
    private bool bBig = false;              // явл€етс€ ли мина большой

    public SS_SpikeballState(SS_SpikeballSmall newOwner) : base(newOwner)
    {
        deathFloor = newOwner.DeathFloor;
        rotationSpeed = newOwner.RotationSpeed * Vector3.up;

        if (owner is SS_SpikeballBig)
        {
            bBig = true;
            fireRate = ((SS_SpikeballBig)owner).FireRate;
        }
    }

    public override void StateEnter()
    {
        // ƒелаем так, чтобы мины не стрел€ли одновременно:
        if (bBig)
        {
            if (fireTimer <= 0)
            {
                fireTimer = Random.Range(0, fireRate);
            }
        }
    }

    public override void StateUpdate()
    {
        // ”ничтожение мины, если она слишком низко
        if (owner.transform.position.z <= deathFloor)
        {
            owner.Die();
        }

        // ¬ращение мины
        ((SS_SpikeballSmall)owner).Rotation += rotationSpeed;

        // ≈сли мина больша€ - стрел€ем
        if (bBig)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                ((SS_SpikeballBig)owner).BFireMode = true;
                fireTimer = fireRate;
            }
            else
            {
                ((SS_SpikeballBig)owner).BFireMode = false;
            }
        }    
    }

    public override void StateExit()
    {

    }
}
