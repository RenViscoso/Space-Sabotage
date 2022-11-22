using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_Ship : SS_Entity
{
    protected float speedAcc;                   // ��������� �������
    protected float health;                     // �������� �������
    protected float rollMax;                    // ������������ ������ �������
    protected float rotCoeff;                   // �����������, ��������������� ������, ���� ������� ������� �� 180 ��������
    protected bool bFireMode = false;           // �������� �� ������� � ������ ������
    protected float fireRate;                   // ������� �������� �������
    protected float fireAngle;                  // ������������ ������� ��������
    protected bool bEnemy = true;               // �������� �� ������� ���������
    protected float deathTime = 0;              // ����� ��������� ����������� �������
    protected float explosionTime = 0f;

    protected SS_StateDeath deathState;         // ��������� ����������� �������

    [SerializeField] protected Collider coll;                       // ���������
    [SerializeField] protected GameObject mesh;                     // ������
    [SerializeField] protected AudioSource aSource;                 // ��������� AudioSource
    [SerializeField] protected DestroyEffect explosion;             // ������ ������
    [SerializeField] protected SS_Bullet bulletPrefab;              // ������ �������
    [SerializeField] protected AudioClip shootSound;                // ���� ��������

    // ��������� ���������� ��������:
    public bool BCollision
    {
        set
        {
            coll.enabled = value;
        }
    }

    // ������� �������� �������:
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
            // ��������� �������� �������� � ����������� �� �������������� ��������:
            Vector3 startRotation = new Vector3(Rotation.x, Rotation.y, 0);
            Rotation = Vector3.Lerp(startRotation, startRotation + 
                Vector3.forward * rollMax * rotCoeff * -Mathf.Sign(Velocity.x), Mathf.Abs(Velocity.x) / speedMax);

            // ��������
            if (BFireMode)
            {
                Fire();
            }
        }
    }

    // ������� �����
    protected virtual void Fire()
    {
        aSource.PlayOneShot(shootSound, 0.08f);
    }

    // ������ ������� c ���������
    protected void Shoot(GameObject point, Vector3 direction)
    {
        SS_Bullet bullet = Instantiate(bulletPrefab, point.transform.position, Quaternion.identity);
        float bulAngle = Random.Range(-fireAngle, fireAngle);
        Vector3 bulDir = Quaternion.Euler(0, bulAngle, 0) * direction.normalized;
        bullet.Init(this, bulDir, bEnemy);
    }

    // ��������� �����
    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            StateSwitch(deathState);
        }
    }

    // ��� ���������� ����� ������������ �������
    public virtual void Testament()
    {
        SpawnExplosion(transform.position);
        Die();
    }

    // �������� ������
    public void SpawnExplosion(Vector3 position)
    {
        Instantiate(explosion, position + Vector3.up * 10, Quaternion.identity);
    }
}
