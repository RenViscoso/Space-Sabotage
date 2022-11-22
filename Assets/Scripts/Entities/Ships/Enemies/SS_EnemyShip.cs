using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_EnemyShip : SS_Ship
{
    protected SS_PlayerShip player;             // Ссылка на корабль игрока
    protected bool bKamikaze = false;            // Должен ли враг взрываться после столкновения с игроком

    public SS_PlayerShip Player
    {
        set
        {
            player = value;
        }
    }

    // Нахождение позиции игрока
    public Vector3 PlayerPosition
    {
        get
        {
            if (player)
            {
                return player.transform.position;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    // Столкновение с игроком
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SS_PlayerShip target))
        {
            // Нанесение урона игроку
            target.Hit(damage);

            // Нанесение урона себе, если враг должен взорваться
            if (bKamikaze)
            {
                Hit(10000);
            }
        }
    }
}
