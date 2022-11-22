using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Trident : SS_EnemyShip
{

    private float introSpeedMin = 30f;              // ����������� ��������� �������� � ��������� Intro
    private float introSpeedMax = 50f;              // ������������ ��������� �������� � ��������� Intro
    private float introTime = 2f;                   // ����� ������������ ��������� Intro
    private float fireTimeMin = 1f;                 // ����������� ������������ ������� ���������
    private float fireTimeMax = 2f;                 // ������������ ������������ ������� ���������
    private float targetingArea = 1f;               // ������ ������� ������ ������, � ������� Trident ������ ������� � ��������� �����
    private float rammingChance = 0.1f;             // ���� �������� � ��������� ������ ����� ��������� �����
    private float deathFloor = -60f;                // �������� �� Z, ����� �������� Trident ����� ���������

    private SS_TridentStateIntro introState;
    private SS_TridentStateFire fireState;
    private SS_TridentStateFlight flightState;
    private SS_TridentStateRamming rammingState;

    public float IntroSpeedMin
    {
        get
        {
            return introSpeedMin;
        }
    }

    public float IntroSpeedMax
    {
        get
        {
            return introSpeedMax;
        }
    }

    public float IntroTime
    {
        get
        {
            return introTime;
        }
    }

    public float FireTimeMin
    {
        get
        {
            return fireTimeMin;
        }
    }

    public float FireTimeMax
    {
        get
        {
            return fireTimeMax;
        }
    }

    public float TargetingArea
    {
        get
        {
            return targetingArea;
        }
    }

    public float RammingChance
    {
        get
        {
            return rammingChance;
        }
    }

    public float DeathFloor
    {
        get
        {
            return deathFloor;
        }
    }

    public SS_TridentStateFire FireState
    {
        get
        {
            return fireState;
        }
    }

    public SS_TridentStateFlight FlightState
    {
        get
        {
            return flightState;
        }
    }

    public SS_TridentStateRamming RammingState
    {
        get
        {
            return rammingState;
        }
    }

    protected override void Awake()
    {
        rb.transform.eulerAngles = Vector3.up * 180;
        base.Awake();
        bKamikaze = true;
        health = 150f;
        damage = 25f;
        fireRate = 0.2f;
        fireAngle = 2f;
        speedMax = 30f;
        speedAcc = 0.1f;
        rollMax = 30f;
        introState = new SS_TridentStateIntro(this);
        fireState = new SS_TridentStateFire(this);
        flightState = new SS_TridentStateFlight(this);
        rammingState = new SS_TridentStateRamming(this);
        StateSwitch(introState);
    }

    protected override void Fire()
    {
        base.Fire();
        Shoot(gameObject, PlayerPosition - transform.position);
    }
}
