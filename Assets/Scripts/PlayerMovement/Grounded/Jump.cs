using UnityEngine;

public class JumpState : GroundedState
{
    private Vector2 _moveInput;

    private Transform _checkPosition;
    private Vector2 _groundCheckSize;
    private LayerMask _groundLayer;

    public JumpState(StateMachine FSM, Rigidbody2D RB, MovementData Data, Transform transform,
        Transform groundCheck, Transform rightWallCheck, Transform leftWallCheck) :
        base(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck)
    {
        _checkPosition = GameObject.FindWithTag("checkGround").GetComponent<Transform>();
        _groundCheckSize = new Vector2(0.49f, 0.03f);
        _groundLayer = 8;
    }

    public override void Enter()
    {
        base.Enter();
        Jump();
        FSM.SetState<UpState>();
    }

    public override void Exit()
    {
        base.Exit();
    }


    private void Jump()
    {
        #region Perform Jump

        float force = Data.jumpForce;
        
        if (RB.velocity.y < 0)
        {
            force -= RB.velocity.y;
        }

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        #endregion
    }
}