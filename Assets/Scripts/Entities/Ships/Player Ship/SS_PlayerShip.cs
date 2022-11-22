using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_PlayerShip : SS_Ship
{
    [SerializeField] private GameObject gunLeft;        // ����� ������
    [SerializeField] private GameObject gunRight;       // ������ ������

    private float speedMaxSlow = 30f;                   // ������������ �������� ��� ����������
    private float introTime = 2f;                       // ����� ���� � ����
    private float outroCeil = 120f;                     // "�������", ����� �������� ������ ����� ���������
    private float outroAcc = 0.02f;                     // ��������� ��� �������� ������
    private float repair = 2f / 60f;                    // �������������� ��������
    private float maxHealth = 100f;                     // ������������ ��������

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
