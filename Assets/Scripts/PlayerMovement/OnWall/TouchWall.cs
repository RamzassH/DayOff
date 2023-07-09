using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchWall : OnWall
{
    private float _timer;
    
    public TouchWall(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _timer = 1.0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _timer -= Time.deltaTime;

        if (_timer > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Fsm.SetState<WallJumpState>();
        }
        
        //TODO проверка на инпут в сторону текущей стены и переход в SLIDE
        if (_timer > 0)
        {
            Fsm.SetState<Slide>();
        }
    }
}
