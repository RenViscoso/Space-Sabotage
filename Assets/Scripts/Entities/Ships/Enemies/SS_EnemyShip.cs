using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_EnemyShip : SS_Ship
{
    protected SS_PlayerShip player;             // ������ �� ������� ������
    protected bool bKamikaze = false;            // ������ �� ���� ���������� ����� ������������ � �������

    public SS_PlayerShip Player
    {
        set
        {
            player = value;
        }
    }

    // ���������� ������� ������
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

    // ������������ � �������
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SS_PlayerShip target))
        {
            // ��������� ����� ������
            target.Hit(damage);

            // ��������� ����� ����, ���� ���� ������ ����������
            if (bKamikaze)
            {
                Hit(10000);
            }
        }
    }
}
