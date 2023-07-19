using UnityEngine;

public class Slide : State
{
    private Vector2 _moveInput;

    public Slide(StateMachine FSM, Rigidbody2D RB, MovementData Data,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<WallJumpState>();
        }

        if (_moveInput.x != 0)
        {
            //TODO Проверка на упор в стену + таймер
            FSM.SetState<Grap>();
        }

        // if (!IsFalling())
        // {
        //     Fsm.SetState<IDLE>();    
        // }
    }
}