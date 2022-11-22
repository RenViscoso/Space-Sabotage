using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_PlayerStateOutro : SS_State
{
    private float speedAcc;
    private float ceil;
    private Vector3 maxVelocity;


    public SS_PlayerStateOutro(SS_PlayerShip newOwner) : base(newOwner)
    {
        speedAcc = newOwner.OutroAcc;
        ceil = newOwner.OutroCeil;
        maxVelocity = newOwner.SpeedMax * Vector3.forward;
    }

    public override void StateEnter()
    {
        ((SS_PlayerShip)owner).BCollision = false;
        ((SS_PlayerShip)owner).BFireMode = false;
    }

    public override void StateUpdate()
    {
        owner.Velocity = Vector3.Lerp(owner.Velocity, maxVelocity, speedAcc);

        if (owner.transform.position.z >= ceil)
        {
            owner.Die();
        }
    }

    public override void StateExit()
    {

    }
}
