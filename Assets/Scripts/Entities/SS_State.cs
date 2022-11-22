using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_State
{
    protected SS_Entity owner;                    // Владелец состояния

    // Конструктор
    public SS_State(SS_Entity newOwner)
    {
        owner = newOwner;
    }

    // Что выполняется при входе в состояние
    public abstract void StateEnter();

    // Что выполняется каждый кадр
    public abstract void StateUpdate();

    // Что выполняется при выходе из состояния
    public abstract void StateExit();
}
