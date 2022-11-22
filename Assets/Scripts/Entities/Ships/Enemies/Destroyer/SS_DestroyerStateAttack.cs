using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_DestroyerStateAttack : SS_State
{
    private float speedMax;
    private float speedAcc;
    private float targetingArea;
    private float fireRate;
    private float fireTimer = 0f;               // Таймер очереди
    private float attackTimeMin;
    private float attackTimeMax;
    private float attackTimer = 0f;             // Таймер состояния атаки
    private float delayTimer = 0f;              // Таймер состояния перерыва
    private bool bFireDelay = true;              // Перерыв?

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
        // Преследование игрока по оси X:
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

        // Если идёт перерыв - считаем его таймер
        if (bFireDelay)
        {
            delayTimer -= Time.deltaTime;
        }
        // Иначе - считаем таймеры очереди и атаки
        else
        {
            fireTimer -= Time.deltaTime;
            attackTimer -= Time.deltaTime;
        }

        // Если сейчас не перерыв и пора стрелять - стреляем
        if (fireTimer <= 0f && !bFireDelay)
        {
            ((SS_Destroyer)owner).BFireMode = true;
            fireTimer = fireRate;
        }
        // Иначе - не стреляем
        else
        {
            ((SS_Destroyer)owner).BFireMode = false;
        }

        // Если время атаки кончилось - уходим на перерыв
        if (attackTimer <= 0f && !bFireDelay)
        {
            SetDelayTimer();
        }
        
        // Если перерыв кончился - атакуем
        if (delayTimer <= 0f && bFireDelay)
        {
            SetAttackTimer();
        }
    }

    public override void StateExit()
    {
        
    }

    // Установка таймера состояния атаки:
    private void SetAttackTimer()
    {
        attackTimer = Random.Range(attackTimeMin, attackTimeMax);
        fireTimer = 0;
        bFireDelay = false;
    }

    // Установка таймера состояния перерыва:
    private void SetDelayTimer()
    {
        delayTimer = attackTimeMin / 2f;
        bFireDelay = true;
    }
}
