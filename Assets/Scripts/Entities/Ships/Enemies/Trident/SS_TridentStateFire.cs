using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_TridentStateFire : SS_State
{

    private float fireRate;
    private float fireTimer = 0f;
    private float stateTimeMin;
    private float stateTimeMax;
    private float stateTimer = 0f;
    private float speedDec;

    public SS_TridentStateFire(SS_Trident newOwner) : base(newOwner)
    {
        fireRate = newOwner.FireRate;
        stateTimeMin = newOwner.FireTimeMin;
        stateTimeMax = newOwner.FireTimeMax;
        speedDec = newOwner.SpeedAcc;
    }

    public override void StateEnter()
    {
        if (stateTimer <= 0f)
        {
            stateTimer = Random.Range(stateTimeMin, stateTimeMax);
            fireTimer = 0;
        }
    }

    public override void StateUpdate()
    {
        // Замедление:
        owner.Velocity = Vector3.Lerp(owner.Velocity, Vector3.zero, speedDec);

        // Таймер стрельбы:
        if (fireTimer <= 0f)
        {
            ((SS_Trident)owner).BFireMode = true;
            fireTimer = fireRate;
        }
        else
        {
            ((SS_Trident)owner).BFireMode = false;
            fireTimer -= Time.deltaTime;
        }

        // Таймер состояния:
        if (stateTimer <= 0f)
        {
            fireTimer = 0f;
            owner.StateSwitch(((SS_Trident)owner).FlightState);
            ((SS_Trident)owner).BFireMode = false;
        }
        else
        {
            stateTimer -= Time.deltaTime;
        }
    }

    public override void StateExit()
    {

    }
}
