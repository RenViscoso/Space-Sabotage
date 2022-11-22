using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_Entity : MonoBehaviour
{
    protected float damage;                     // ���� ��� ������������
    protected float speedMax;                   // ������������ ��������
    protected SS_State currentState;            // ������� ���������
    protected SS_State previousState;
    protected SS_StatePause pauseState;         // ��������� �����

    [SerializeField] protected Rigidbody rb;                        // ��������� RigidBody

    public float SpeedMax
    {
        get
        {
            return speedMax;
        }
    }

    // ������� ��������
    public Vector3 Velocity
    {
        get
        {
            return rb.velocity;
        }
        
        set
        {
            rb.velocity = value;
        }
    }

    // �������� ���������� ��������� �����
    protected virtual void Awake()
    {
        pauseState = new SS_StatePause(this);
    }

    protected virtual void Update()
    {
        // ����� ������ StateUpdate �� �������� ���������:
        currentState.StateUpdate();
    }

    // ����������� �������
    public void Die()
    {
        Destroy(this.gameObject);
    }

    // ����� ���������
    public void StateSwitch(SS_State newState)
    {
        previousState = currentState;
        if (previousState != null)
        {
            previousState.StateExit();
        }
        currentState = newState;
        currentState.StateEnter();
    }

    public void Pause()
    {
        if (currentState == pauseState)
        {
            StateSwitch(previousState);
        }
        else
        {
            StateSwitch(pauseState);
        }
    }
}
