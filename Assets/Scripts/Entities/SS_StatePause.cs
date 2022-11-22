using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_StatePause : SS_State
{
    private Vector3 oldVelocity;                // ����������-��������� �������� ���������
    private bool oldFireMode;                   // ����������-��������� ��������� �������� ���������

    public SS_StatePause(SS_Entity newOwner) : base(newOwner)
    {

    }

    public override void StateEnter()
    {
        // ��������� ��������-��������� ��� ����� � �����:
        oldVelocity = owner.Velocity;
        owner.Velocity = Vector3.zero;

        // ���� �������� - �������, ����� ���������� ��� ��������:
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
        // ���������� ��������:
        owner.Velocity = oldVelocity;

        // ���������� ��������� ��������, ���� �������� - �������:
        if (owner is SS_Ship)
        {
            ((SS_Ship)owner).BFireMode = oldFireMode;
        }
    }


}
