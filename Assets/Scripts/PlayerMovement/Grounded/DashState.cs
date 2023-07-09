using System.Collections;
using UnityEngine;
public class DashState : GroundedState
{
    private Vector2 _moveInput;
    private Transform _checkPosition;
    private Vector2 _checkSize;
    private LayerMask _groundLayer;

    private bool _isDashing;


    public DashState(StateMachine fsm,  MovementData Data, Rigidbody2D rb) : base(fsm, rb, Data)
    {
        _checkPosition = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _checkSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
    }
    public override void Enter()
    {
        base.Enter();
        
        Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;
        
        // Проблема тут
        StartCoroutine(StartDash(direction)); 
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("не Дэшусь ебать");
    }

    public override void Update()
    {
        base.Update();
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        
        if (_moveInput.x != 0 && !_isDashing)
        {
            Fsm.SetState<RunState>();
        }
        
        if (_moveInput.x == 0 && !_isDashing)
        {
            Fsm.SetState<IDLE>();
        }
        
        if (IsFalling(_checkPosition.position, _checkSize, _groundLayer) &&
            !_isDashing)
        {
            Fsm.SetState<FallingState>();
        }
    }
    public IEnumerator StartDash(Vector2 dir)
    {
        _isDashing = true;
        
        // LastOnGroundTime = 0;
        // LastPressedDashTime = 0;

        float startTime = Time.time;
        
        RB.gravityScale = 0;
        
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;

            yield return null;
        }

        startTime = Time.time;
        
        RB.gravityScale = Data.gravityScale;
        
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        _isDashing = false;
    }
}
