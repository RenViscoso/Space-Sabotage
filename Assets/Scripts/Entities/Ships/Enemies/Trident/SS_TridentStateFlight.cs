using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_TridentStateFlight : SS_State
{
    private float playerX;                      // Позиция игрока по оси X
    private float area;
    private float speedMax;
    private float speedAcc;
    private float dir;                          // Направление движения
    private float rammingChance;
    private Vector3 maxVelocity;                // Максимальная скорость

    public SS_TridentStateFlight(SS_Trident newOwner) : base(newOwner)
    {
        area = newOwner.TargetingArea;
        speedMax = newOwner.SpeedMax;
        speedAcc = newOwner.SpeedAcc;
        rammingChance = newOwner.RammingChance;
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        // Вычисление максимальной скорости:
        playerX = ((SS_Trident)owner).PlayerPosition.x;
        dir = Mathf.Sign(playerX - owner.transform.position.x);
        maxVelocity = Vector3.right * dir * speedMax;

        // Разгон:
        owner.Velocity = Vector3.Lerp(owner.Velocity, maxVelocity, speedAcc);

        // Проверка позиции Trident:
        float ownerX = owner.transform.position.x;

        // Если Trident нацелился на игрока - атака или таран
        if ((playerX - area) < ownerX && (playerX + area) > ownerX)
        {
            float rand = Random.Range(0f, 1f);

            if (rand > rammingChance)
            {
                owner.StateSwitch(((SS_Trident)owner).FireState);
            }
            else
            {
                owner.StateSwitch(((SS_Trident)owner).RammingState);
            }
        }
    }

    public override void StateExit()
    {
        
    }
}
