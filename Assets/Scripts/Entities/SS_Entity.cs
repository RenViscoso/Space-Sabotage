using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_Entity : MonoBehaviour
{
    protected float damage;                     // Урон при столкновении
    protected float speedMax;                   // Максимальная скорость
    protected SS_State currentState;            // Текущее состояние
    protected SS_State previousState;
    protected SS_StatePause pauseState;         // Состояние паузы

    [SerializeField] protected Rigidbody rb;                        // Компонент RigidBody

    public float SpeedMax
    {
        get
        {
            return speedMax;
        }
    }

    // Текущая скорость
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

    // Создание экземпляра состояния паузы
    protected virtual void Awake()
    {
        pauseState = new SS_StatePause(this);
    }

    protected virtual void Update()
    {
        // Вызов метода StateUpdate из текущего состояния:
        currentState.StateUpdate();
    }

    // Уничтожение объекта
    public void Die()
    {
        Destroy(this.gameObject);
    }

    // Смена состояния
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
