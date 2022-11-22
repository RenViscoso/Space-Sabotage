using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_StateDeath : SS_State
{
    private float timer = 0f;

    public SS_StateDeath(SS_Ship newOwner) : base(newOwner)
    {
        
    }

    public override void StateEnter()
    {
        if (timer <= 0f)
        {
            // Останавливаем владельца, прекращаем его стрельбу и отключаем его коллизии:
            timer = ((SS_Ship)owner).DeathTime;
            owner.Velocity = Vector3.zero;
            ((SS_Ship)owner).BFireMode = false;
            ((SS_Ship)owner).BCollision = false;
        }
    }

    public override void StateUpdate()
    {
        // Уменьшаем таймер:
        timer -= Time.deltaTime;

        // Если таймер закончился - выполняем функцию уничтожения объекта:
        if (timer <= 0)
        {
            ((SS_Ship)owner).Testament();
        }
    }

    public override void StateExit()
    {

    }

    // Метод для продолжения таймера:
    public void RestartTimer(float newTime)
    {
        timer = newTime;
    }
}
