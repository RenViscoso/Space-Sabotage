using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_PlayerShip : SS_Ship
{
    [SerializeField] private GameObject gunLeft;        // Левое орудие
    [SerializeField] private GameObject gunRight;       // Правое орудие

    private float speedMaxSlow = 30f;                   // Макисмальная скорость при замедлении
    private float introTime = 2f;                       // Время влёта в кадр
    private float outroCeil = 120f;                     // "Потолок", после которого объект будет уничтожен
    private float outroAcc = 0.02f;                     // Ускорение для катсцены вылета
    private float repair = 2f / 60f;                    // Восстановление здоровья
    private float maxHealth = 100f;                     // Макисмальное здоровье

    private SS_PlayerStateIntro introState;
    private SS_PlayerStateGame gameState;
    private SS_PlayerStateOutro outroState;

    public SS_PlayerStateGame GameState
    {
        get
        {
            return gameState;
        }
    }

    public SS_PlayerStateOutro OutroState
    {
        get
        {
            return outroState;
        }
    }

    public float SpeedMaxSlow
    {
        get
        {
            return speedMaxSlow;
        }
    }

    public float IntroTime
    {
        get
        {
            return introTime;
        }
    }

    public float Repair
    {
        get
        {
            return repair;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float OutroCeil
    {
        get
        {
            return outroCeil;
        }
    }
    
    public float OutroAcc
    {
        get
        {
            return outroAcc;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        speedMax = 50f;
        speedAcc = 0.2f;
        rollMax = 30f;
        fireRate = 0.1f;
        fireAngle = 2f;
        health = MaxHealth;
        bEnemy = false;

        introState = new SS_PlayerStateIntro(this);
        gameState = new SS_PlayerStateGame(this);
        outroState = new SS_PlayerStateOutro(this);

        StateSwitch(introState);
    }

    protected override void Fire()
    {
        base.Fire();
        Shoot(gunLeft, Vector3.forward);
        Shoot(gunRight, Vector3.forward);
    }
}
