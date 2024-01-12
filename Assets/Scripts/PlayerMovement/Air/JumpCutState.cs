using Unity.VisualScripting;
using UnityEngine;

public class JumpCutState : AirState
{
    
    public JumpCutState(ChController controller) :
        base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        JumpCut();
        FSM.SetState<FallingState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void JumpCut()
    {
        RB.velocity = new Vector2(RB.velocity.x, -RB.velocity.y);
    }
}