using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_DestroyerStateIntro : SS_State
{

    private float stateTime;
    private float timer = 0f;
    private Vector3 startVelocity;

    public SS_DestroyerStateIntro(SS_Destroyer newOwner) : base(newOwner)
    {
        stateTime = newOwner.IntroTime;
        startVelocity = Vector3.back * newOwner.SpeedMax;
    }

    public override void StateEnter()
    {
        if (timer <= 0f)
        {
            timer = stateTime;
        }
    }

    public override void StateUpdate()
    {
        // Замедление:
        owner.Velocity = Vector3.Lerp(Vector3.zero, startVelocity, timer / stateTime);

        // Таймер:
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            owner.StateSwitch(((SS_Destroyer)owner).AttackState);
        }
    }

    public override void StateExit()
    {

    }

}
