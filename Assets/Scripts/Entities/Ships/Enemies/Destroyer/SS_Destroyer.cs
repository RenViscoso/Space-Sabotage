using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Destroyer : SS_EnemyShip
{

    [SerializeField] private GameObject firePoint1;                           // Компонент-огневая точка 1
    [SerializeField] private GameObject firePoint2;                           // Компонент-огневая точка 2
    [SerializeField] private GameObject firePoint3;                           // Компонент-огневая точка 3
    [SerializeField] private GameObject firePoint4;                           // Компонент-огневая точка 4

    private float introTime = 4f;
    private float targetingArea = 1f;
    private int phase = 1;
    private float attackTimeMin = 3f;
    private float attackTimeMax = 5f;
    private float explosionDistance = 4f;
    private float explosionRateMin = 0.25f;
    private float explosionRateMax = 0.75f;
    private int explosionCount = 8;
    private float maxHealth = 3000f;

    private SS_DestroyerStateIntro introState;
    private SS_DestroyerStateAttack attackState;

    public float IntroTime
    {
        get
        {
            return introTime;
        }
    }

    public float TargetingArea
    {
        get
        {
            return targetingArea;
        }
    }

    public float AttackTimeMin
    {
        get
        {
            return attackTimeMin;
        }
    }

    public float AttackTimeMax
    {
        get
        {
            return attackTimeMax;
        }
    }

    public SS_DestroyerStateAttack AttackState
    {
        get
        {
            return attackState;
        }
    }

    protected override void Awake()
    {
        rb.transform.eulerAngles = Vector3.up * 180;
        base.Awake();
        health = maxHealth;
        damage = 100f;
        speedMax = 10f;
        rollMax = 15f;
        speedAcc = 0.05f;
        fireRate = 0.1f;
        fireAngle = 3f;
        introState = new SS_DestroyerStateIntro(this);
        attackState = new SS_DestroyerStateAttack(this);
        StateSwitch(introState);
    }

    protected override void Update()
    {
        base.Update();

        // Вычисление фазы боя:
        phase = (int)((maxHealth - health) / 1000) + 1;
    }

    protected override void Fire()
    {
        base.Fire();

        // Позиция игрока и направление стрельбы:
        Vector3 playerPos = PlayerPosition;
        Vector3 fireDir;

        // Стрельба в первой фазе боя:
        if (phase >= 1)
        {
            fireDir = playerPos - firePoint1.transform.position;
            Shoot(firePoint1, fireDir);
            fireDir = playerPos - firePoint2.transform.position;
            Shoot(firePoint2, fireDir);
        }

        // Стрельба во второй фазе боя:
        if (phase >= 2)
        {
            fireDir = playerPos - firePoint3.transform.position;
            Shoot(firePoint3, fireDir);
            fireDir = playerPos - firePoint4.transform.position;
            Shoot(firePoint4, fireDir);
        }

        // Стрельба в третьей фазе боя:
        if (phase >= 3)
        {
            ShootBlock(firePoint3);
            ShootBlock(firePoint4);
        }
    }

    // Блокирующий выстрел:
    private void ShootBlock(GameObject point)
    {
        SS_Bullet bullet = Instantiate<SS_Bullet>(bulletPrefab, point.transform);
        float bulAngle = Random.Range(-fireAngle, fireAngle);
        Vector3 bulDir = Quaternion.Euler(0, bulAngle, 0) * (point.transform.position - transform.position).normalized;
        bullet.Init(this, bulDir, bEnemy);
    }

    public override void Testament()
    {
        if (explosionCount > 0)
        {
            // Создание взрыва:
            Vector3 offset = Vector3.right * Random.Range(-explosionDistance, explosionDistance);
            SpawnExplosion(transform.position + offset);
            // Выставление нового таймера в состоянии смерти:
            float newTime = Random.Range(explosionRateMin, explosionRateMax);
            deathState.RestartTimer(newTime);
            explosionCount--;
        }
        else
        {
            // Уничтожение объекта:
            base.Testament();
        }
    }

}
