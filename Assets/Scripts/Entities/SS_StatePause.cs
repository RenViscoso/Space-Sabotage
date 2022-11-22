using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_StatePause : SS_State
{
    private Vector3 oldVelocity;                // Переменная-держатель скорости владельца
    private bool oldFireMode;                   // Переменная-держатель состояния стрельбы владельца

    public SS_StatePause(SS_Entity newOwner) : base(newOwner)
    {

    }

    public override void StateEnter()
    {
        // Остановка сущности-владельца при входе в паузу:
        oldVelocity = owner.Velocity;
        owner.Velocity = Vector3.zero;

        // Если владелец - корабль, также прекращаем его стрельбу:
        if (owner is SS_Ship)
        {
            oldFireMode = ((SS_Ship)owner).BFireMode;
            ((SS_Ship)owner).BFireMode = false;
        }
    }

    public override void StateUpdate()
    {

    }

    public override void StateExit()
    {
        // Возвращаем скорость:
        owner.Velocity = oldVelocity;

        // Возвращаем состояние стрельбы, если владелец - корабль:
        if (owner is SS_Ship)
        {
            ((SS_Ship)owner).BFireMode = oldFireMode;
        }
    }


}
