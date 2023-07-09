using Unity.VisualScripting;
using UnityEngine;

public class IDLE : GroundedState
{
    private Rigidbody2D _rb;
    private MovementData _data;

    private Transform _groundCheck;
    private Vector2 _groundCheckSize;

    #region States

    private bool IsFacingRight;

    #endregion
    
    #region Timers

    // Таймер последнего нахождения персонажа на земле(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnGroundTime { get; private set; }

    // Таймер последнего нахождения персонажа на стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallTime { get; private set; }

    // Таймер последнего нахождения персонажа на правой стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallRightTime { get; private set; }

    // Таймер последнего нахождения персонажа на левой стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallLeftTime { get; private set; }

    #endregion

    #region INPUT PARAMETERS

    private Vector2 _moveInput;

    // Таймер послденего считывания кнопки "прыжка"
    public float LastPressedJumpTime { get; private set; }

    // Таймер послденего считывания кнопки "дэша"
    public float LastPressedDashTime { get; private set; }

    #endregion

    public IDLE(StateMachine fsm, MovementData Data, Rigidbody2D RB, Transform groundCheck, Vector2 groundCheckSize) : base(fsm, RB, Data)
    {
        _groundCheck = groundCheck;
        _groundCheckSize = groundCheckSize;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Стою хуле");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Не стою, получается");
    }

    public override void Update()
    {
        #region Input

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
        {
            Fsm.SetState<RunState>();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fsm.SetState<JumpState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Fsm.SetState<DashState>();
        }
        #endregion

        
        base.Update();
        Debug.Log("Все еще стою, хуле");
    }

    public void OnJumpInput()
    {
        LastPressedJumpTime = _data.jumpInputBufferTime;
    }
    

    // public void CheckDirectionToFace(bool isMovingRight)
    // {
    //     if (isMovingRight != IsFacingRight)
    //         Turn();
    // }
    
    // private void Turn()
    // {
    //     Vector3 scale = transform.localScale;
    //     scale.x *= -1;
    //     transform.localScale = scale;
    //     IsFacingRight = !IsFacingRight;
    // }
}