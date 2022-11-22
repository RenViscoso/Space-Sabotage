using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SS_State
{
    protected SS_Entity owner;                    // �������� ���������

    // �����������
    public SS_State(SS_Entity newOwner)
    {
        owner = newOwner;
    }

    // ��� ����������� ��� ����� � ���������
    public abstract void StateEnter();

    // ��� ����������� ������ ����
    public abstract void StateUpdate();

    // ��� ����������� ��� ������ �� ���������
    public abstract void StateExit();
}
