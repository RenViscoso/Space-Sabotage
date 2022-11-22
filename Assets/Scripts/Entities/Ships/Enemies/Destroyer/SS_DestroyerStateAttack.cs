using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_DestroyerStateAttack : SS_State
{
    private float speedMax;
    private float speedAcc;
    private float targetingArea;
    private float fireRate;
    private float fireTimer = 0f;               // ������ �������
    private float attackTimeMin;
    private float attackTimeMax;
    private float attackTimer = 0f;             // ������ ��������� �����
    private float delayTimer = 0f;              // ������ ��������� ��������
    private bool bFireDelay = true;              // �������?

    public SS_DestroyerStateAttack(SS_Destroyer newOwner) : base(newOwner)
    {
        speedMax = newOwner.SpeedMax;
        speedAcc = newOwner.SpeedAcc;
        targetingArea = newOwner.TargetingArea;
        fireRate = newOwner.FireRate;
        attackTimeMin = newOwner.AttackTimeMin;
        attackTimeMax = newOwner.AttackTimeMax;
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        // ������������� ������ �� ��� X:
        float playerX = ((SS_Destroyer)owner).PlayerPosition.x;
        float difference = playerX - owner.transform.position.x;
        Vector3 newVelocity;

        if (Mathf.Abs(difference) > targetingArea)
        {
            newVelocity = Vector3.right * Mathf.Sign(difference) * speedMax;
        }
        else
        {
            newVelocity = Vector3.zero;
        }

        owner.Velocity = Vector3.Lerp(owner.Velocity, newVelocity, speedAcc);

        // ���� ��� ������� - ������� ��� ������
        if (bFireDelay)
        {
            delayTimer -= Time.deltaTime;
        }
        // ����� - ������� ������� ������� � �����
        else
        {
            fireTimer -= Time.deltaTime;
            attackTimer -= Time.deltaTime;
        }

        // ���� ������ �� ������� � ���� �������� - ��������
        if (fireTimer <= 0f && !bFireDelay)
        {
            ((SS_Destroyer)owner).BFireMode = true;
            fireTimer = fireRate;
        }
        // ����� - �� ��������
        else
        {
            ((SS_Destroyer)owner).BFireMode = false;
        }

        // ���� ����� ����� ��������� - ������ �� �������
        if (attackTimer <= 0f && !bFireDelay)
        {
            SetDelayTimer();
        }
        
        // ���� ������� �������� - �������
        if (delayTimer <= 0f && bFireDelay)
        {
            SetAttackTimer();
        }
    }

    public override void StateExit()
    {
        
    }

    // ��������� ������� ��������� �����:
    private void SetAttackTimer()
    {
        attackTimer = Random.Range(attackTimeMin, attackTimeMax);
        fireTimer = 0;
        bFireDelay = false;
    }

    // ��������� ������� ��������� ��������:
    private void SetDelayTimer()
    {
        delayTimer = attackTimeMin / 2f;
        bFireDelay = true;
    }
}
