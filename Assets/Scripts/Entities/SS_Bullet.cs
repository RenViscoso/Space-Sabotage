using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Bullet : SS_Entity
{
    [SerializeField] private Material redMaterial;                  // Материал для вражеских снарядов

    private SS_Ship owner;                                       // Владелец снаряда
    private bool bEnemy;                                            // Принадлежит ли снаряд врагу
    private float deathDistance = 120f;                             // Радиус, в котором может существовать снаряд

    private SS_BulletState state;

    public float DeathDistance
    {
        get
        {
            return deathDistance;
        }
    }

    // Новые значения скорости и урона
    protected override void Awake()
    {
        base.Awake();
        speedMax = 60f;
        damage = 10f;
        state = new SS_BulletState(this);
        StateSwitch(state);
    }

    // Инициализация снаряда (владелец, векторная скорость и цвет)
    public void Init(SS_Ship newOwner, Vector3 newDir, bool bNewEnemy)
    {
        owner = newOwner;
        Velocity = SpeedMax * newDir;

        bEnemy = bNewEnemy;

        if (bEnemy)
        {
            GetComponent<Renderer>().sharedMaterial = redMaterial;
        }
    }

    // Столкновение с объектом, нанесение ему урона и самоуничтожение
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner.gameObject)
        {
            // Если снаряд вражеский, он реагирует только на игрока
            if (bEnemy && other.gameObject.GetComponent<SS_PlayerShip>() || !bEnemy)
            {
                other.gameObject.GetComponent<SS_Ship>().Hit(damage);
                Die();
            }
        }
    }
}
