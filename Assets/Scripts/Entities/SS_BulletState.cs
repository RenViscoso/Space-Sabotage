using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_BulletState : SS_State
{
    private float deathDistance;

    public SS_BulletState(SS_Bullet newOwner) : base(newOwner)
    {
        deathDistance = newOwner.DeathDistance;
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        // ”ничтожение снар€да, если он слишком далеко
        if (owner.gameObject.transform.position.magnitude >= deathDistance)
        {
            owner.Die();
        }
    }

    public override void StateExit()
    {
        
    }
}
