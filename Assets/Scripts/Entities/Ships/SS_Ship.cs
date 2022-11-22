using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_Ship : SS_Entity
{
    protected float speedAcc;                   // Ускорение корабля
    protected float health;                     // Здоровье корабля
    protected float rollMax;                    // Максимальный наклон корабля
    protected float rotCoeff;                   // Коэффициент, разворачивающий наклон, если корабль повёрнут на 180 градусов
    protected bool bFireMode = false;           // Стреляет ли корабль в данный момент
    protected float fireRate;                   // Частота стрельбы корабля
    protected float fireAngle;                  // Макисмальный разброс снарядов
    protected bool bEnemy = true;               // Является ли корабль вражеским
    protected float deathTime = 0;              // Время состояния уничтожения корабля
    protected float explosionTime = 0f;

    protected SS_StateDeath deathState;         // Состояние уничтожения корабля

    [SerializeField] protected Collider coll;                       // Коллайдер
    [SerializeField] protected GameObject mesh;                     // Модель
    [SerializeField] protected AudioSource aSource;                 // Компонент AudioSource
    [SerializeField] protected DestroyEffect explosion;             // Префаб взрыва
    [SerializeField] protected SS_Bullet bulletPrefab;              // Префаб снаряда
    [SerializeField] protected AudioClip shootSound;                // Звук выстрела

    // Установка активности коллизий:
    public bool BCollision
    {
        set
        {
            coll.enabled = value;
        }
    }

    // Текущее вращение корабля:
    public Vector3 Rotation
    {
        get
        {
            return mesh.transform.localEulerAngles;
        }

        set
        {
            mesh.transform.localEulerAngles = value;
        }
    }

    public bool BFireMode
    {
        get
        {
            return bFireMode;
        }

        set
        {
            bFireMode = value;
        }
    }
    
    public float DeathTime
    {
        get
        {
            return deathTime;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public float SpeedAcc
    {
        get
        {
            return speedAcc;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        deathState = new SS_StateDeath(this);
        rotCoeff = Mathf.Cos(Mathf.Deg2Rad * rb.transform.rotation.eulerAngles.y);
    }

    protected override void Update()
    {
        base.Update();

        if (currentState != pauseState)
        {
            // Установка текущего вращения в зависимости от горизонтальной скорости:
            Vector3 startRotation = new Vector3(Rotation.x, Rotation.y, 0);
            Rotation = Vector3.Lerp(startRotation, startRotation + 
                Vector3.forward * rollMax * rotCoeff * -Mathf.Sign(Velocity.x), Mathf.Abs(Velocity.x) / speedMax);

            // Стрельба
            if (BFireMode)
            {
                Fire();
            }
        }
    }

    // Открыть огонь
    protected virtual void Fire()
    {
        aSource.PlayOneShot(shootSound, 0.08f);
    }

    // Запуск снаряда c разбросом
    protected void Shoot(GameObject point, Vector3 direction)
    {
        SS_Bullet bullet = Instantiate(bulletPrefab, point.transform.position, Quaternion.identity);
        float bulAngle = Random.Range(-fireAngle, fireAngle);
        Vector3 bulDir = Quaternion.Euler(0, bulAngle, 0) * direction.normalized;
        bullet.Init(this, bulDir, bEnemy);
    }

    // Нанесение урона
    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            StateSwitch(deathState);
        }
    }

    // Что происходит перед уничтожением объекта
    public virtual void Testament()
    {
        SpawnExplosion(transform.position);
        Die();
    }

    // Создание взрыва
    public void SpawnExplosion(Vector3 position)
    {
        Instantiate(explosion, position + Vector3.up * 10, Quaternion.identity);
    }
}
