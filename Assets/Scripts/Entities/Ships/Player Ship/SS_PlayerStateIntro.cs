using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_PlayerStateIntro : SS_State
{
    private float stateTime;
    private float stateTimer = 0f;
    private Vector3 startVelocity;

    public SS_PlayerStateIntro(SS_PlayerShip newOwner) : base(newOwner)
    {
        startVelocity = newOwner.SpeedMax * Vector3.forward;
        stateTime = newOwner.IntroTime;
    }

    public override void StateEnter()
    {
        if (stateTimer <= 0f)
        {
            ((SS_PlayerShip)owner).BCollision = false;
            stateTimer = stateTime;
        }
    }

    public override void StateUpdate()
    {
        owner.Velocity = Vector3.Lerp(Vector3.zero, startVelocity, stateTimer / stateTime);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            owner.StateSwitch(((SS_PlayerShip)owner).GameState);
        }
    }

    public override void StateExit()
    {
        ((SS_PlayerShip)owner).BCollision = true;
        owner.Velocity = Vector3.zero;
    }
}
