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

    public IDLE(tmpMovement playerMovement) :
        base(playerMovement)
    {
        _groundCheck = playerMovement.groundCheck;
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
        #region Input

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (IsTouchWall(playerMovement.rightWallCheck.position, playerMovement.leftWallCheck.position, wallCheckSize, groundLayer))
        {
            FSM.SetState<TouchWall>();
        }

        if (_moveInput.x != 0)
        {
            FSM.SetState<RunState>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSM.SetState<JumpState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FSM.SetState<DashState>();
        }

        #endregion


        base.Update();
    }

    public void OnJumpInput()
    {
        LastPressedJumpTime = _data.jumpInputBufferTime;
    }
}