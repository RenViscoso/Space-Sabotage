using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_PlayerStateGame : SS_State
{
    private float speedMax;
    private float speedMaxSlow;
    private float speedAcc;
    private float fireRate;
    private float fireTimer = 0f;
    private float repair;
    private float maxHealth;

    public SS_PlayerStateGame(SS_PlayerShip newOwner) : base(newOwner)
    {
        speedMax = newOwner.SpeedMax;
        speedMaxSlow = newOwner.SpeedMaxSlow;
        speedAcc = newOwner.SpeedAcc;
        fireRate = newOwner.FireRate;
        repair = newOwner.Repair;
        maxHealth = newOwner.MaxHealth;
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        // Перемещение:
        Vector3 inp = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inp = inp.normalized;
        Vector3 newVelocity = inp * (Input.GetButton("SlowMode") ? speedMaxSlow : speedMax);
        owner.Velocity = Vector3.Lerp(owner.Velocity, newVelocity, speedAcc);

        // Стрельба:
        if (fireTimer <= 0)
        {
            if (Input.GetAxis("Fire") > 0)
            {
                ((SS_PlayerShip)owner).BFireMode = true;
                fireTimer = fireRate;
            }
        }
        else
        {
            fireTimer -= Time.deltaTime;
            ((SS_PlayerShip)owner).BFireMode = false;
        }

        // Восстановление здоровья:
        ((SS_PlayerShip)owner).Health += repair;

        if (((SS_PlayerShip)owner).Health > maxHealth)
        {
            ((SS_PlayerShip)owner).Health = maxHealth;
        }
    }

    public override void StateExit()
    {
        
    }
}
