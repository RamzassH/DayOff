using UnityEngine;

public class Slide : State
{
    private Vector2 _moveInput;
    public Slide(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
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
            Fsm.SetState<WallJumpState>();
        }

        if (_moveInput.x != 0)
        {
            //TODO Проверка на упор в стену + таймер
            Fsm.SetState<Grap>();
        }

        // if (!IsFalling())
        // {
        //     Fsm.SetState<IDLE>();    
        // }

    }
}
