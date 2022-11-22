using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_SpikeballSmall : SS_EnemyShip
{
    protected float deathFloor = -60f;          // ѕозици€ по оси Z, после которой мина будет уничтожена
    protected float rotationSpeed = -1f;

    protected SS_SpikeballState state;

    public float DeathFloor
    {
        get
        {
            return deathFloor;
        }
    }

    public float RotationSpeed
    { 
        get
        {
            return rotationSpeed;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        bKamikaze = true;
        speedMax = 15f;
        health = 100f;
        damage = 25f;
        Velocity = Vector3.back * speedMax;
        state = new SS_SpikeballState(this);
        StateSwitch(state);
    }
}
