using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPVersion
{
    public class Example : MonoBehaviour
    {
        //// ��� ������ ����, ��� � ���� �������� ���� ��
        //// ������ ���������
        //
        //public StateMachine movementSM; // ������ ���������
        //                                // ��� ���������, ������� ����� ��������� ����
        //                                // ������ ����� ����� ��� ������� ��������� �����.
        //public StandingState standing;
        //public DuckingState ducking;
        //public JumpingState jumping;
        //
        //private void Start()
        //{
        //    // �������������, ������ ����������� ��������� � ������ �������
        //    // �� ��� ����, � ����� ������
        //    standing = new StandingState(this, movementSM);
        //    ducking = new DuckingState(this, movementSM);
        //    jumping = new JumpingState(this, movementSM);
        //
        //    // ������������� ������
        //    movementSM = new StateMachine(standing);
        //}
        //
        //private void Update()
        //{
        //    movementSM.CurrentState.HandleInput();
        //
        //    movementSM.CurrentState.LogicUpdate();
        //}
        //
        //private void FixedUpdate()
        //{
        //    movementSM.CurrentState.PhysicsUpdate();
        //}
    }
}