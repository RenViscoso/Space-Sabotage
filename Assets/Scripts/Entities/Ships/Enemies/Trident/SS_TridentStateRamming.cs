using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_TridentStateRamming : SS_State
{

    private float speedMax;
    private float speedAcc;
    private float deathFloor;
    private Vector3 maxVelocity;

    public SS_TridentStateRamming(SS_Trident newOwner) : base(newOwner)
    {
        speedMax = newOwner.SpeedMax * 2;
        speedAcc = newOwner.SpeedAcc * 2;
        deathFloor = newOwner.DeathFloor;
    }

    public override void StateEnter()
    {
        // ���������� ������������ ��������:
        maxVelocity = Vector3.back * speedMax;
    }

    public override void StateUpdate()
    {
        // ������:
        owner.Velocity = Vector3.Lerp(owner.Velocity, maxVelocity, speedAcc);

        // ���� Trident ������� ������ - ���������� ���:
        if (owner.transform.position.x < deathFloor)
        {
            owner.Die();
        }
    }

    public override void StateExit()
    {

    }
}
