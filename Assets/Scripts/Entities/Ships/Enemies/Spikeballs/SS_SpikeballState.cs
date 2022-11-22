using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_SpikeballState : SS_State
{
    private float deathFloor;
    private Vector3 rotationSpeed;
    private float fireRate;
    private float fireTimer = 0;
    private bool bBig = false;              // �������� �� ���� �������

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
        // ������ ���, ����� ���� �� �������� ������������:
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
        // ����������� ����, ���� ��� ������� �����
        if (owner.transform.position.z <= deathFloor)
        {
            owner.Die();
        }

        // �������� ����
        ((SS_SpikeballSmall)owner).Rotation += rotationSpeed;

        // ���� ���� ������� - ��������
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
