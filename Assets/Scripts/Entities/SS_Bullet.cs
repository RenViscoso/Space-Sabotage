using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Bullet : SS_Entity
{
    [SerializeField] private Material redMaterial;                  // �������� ��� ��������� ��������

    private SS_Ship owner;                                       // �������� �������
    private bool bEnemy;                                            // ����������� �� ������ �����
    private float deathDistance = 120f;                             // ������, � ������� ����� ������������ ������

    private SS_BulletState state;

    public float DeathDistance
    {
        get
        {
            return deathDistance;
        }
    }

    // ����� �������� �������� � �����
    protected override void Awake()
    {
        base.Awake();
        speedMax = 60f;
        damage = 10f;
        state = new SS_BulletState(this);
        StateSwitch(state);
    }

    // ������������� ������� (��������, ��������� �������� � ����)
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

    // ������������ � ��������, ��������� ��� ����� � ���������������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner.gameObject)
        {
            // ���� ������ ���������, �� ��������� ������ �� ������
            if (bEnemy && other.gameObject.GetComponent<SS_PlayerShip>() || !bEnemy)
            {
                other.gameObject.GetComponent<SS_Ship>().Hit(damage);
                Die();
            }
        }
    }
}
