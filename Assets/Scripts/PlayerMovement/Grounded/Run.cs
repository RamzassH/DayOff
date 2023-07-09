using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class RunState : GroundedState
{
    private MovementData _data;

    private Transform _checkPosition;
    private Vector2 _checkSize;
    private LayerMask _groundLayer;
    
    #region INPUT PARAMETERS

    private Vector2 _moveInput;

    #endregion

    public RunState(StateMachine fsm, Rigidbody2D rb, MovementData Data) : base(fsm, rb, Data)
    {
        _data = Data;
        _checkPosition = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _checkSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.LogError("теперь бягу хуле"); 
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Не бягу, получается");
    }

    public override void Update()
    {
        #region Input
        _moveInput.x = Input.GetAxisRaw("Horizontal");

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
        Debug.Log("Бягу");

        if (RB.velocity.x == 0 && RB.velocity.y == 0 && _moveInput.x == 0)
        {
            Fsm.SetState<IDLE>();
        }

        if (IsFalling(_checkPosition.position, _checkSize, _groundLayer))
        {
            Fsm.SetState<FallingState>();
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_moveInput.x != 0)
        {
            Run(1, _moveInput);
        }
    }
    
}
