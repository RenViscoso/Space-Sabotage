using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_TridentStateIntro : SS_State
{

    private float speedMin;
    private float speedMax;
    private Vector3 startVelocity;              // Начальная скорость
    private float stateTime;
    private float timer = 0f;

    public SS_TridentStateIntro(SS_Trident newOwner) : base(newOwner)
    {
        speedMin = newOwner.IntroSpeedMin;
        speedMax = newOwner.IntroSpeedMax;
        stateTime = newOwner.IntroTime;
    }

    public override void StateEnter()
    {
        if (timer <= 0f)
        {
            // Установка начальной скорости:
            startVelocity = Vector3.back * Random.Range(speedMin, speedMax);
            timer = stateTime;
        }
    }

    public override void StateUpdate()
    {
        // Торможение:
        owner.Velocity = Vector3.Lerp(Vector3.zero, startVelocity, timer / stateTime);

        // Таймер:
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            owner.StateSwitch(((SS_Trident)owner).FireState);
        }
    }

    public override void StateExit()
    {

    }

}
