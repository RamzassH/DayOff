using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IDLE : GroundedState
{
    public IDLE(ChController controller) :
        base(controller)
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

    private void OnEnable()
    {
        controller.playerInput.Enable();
    }

    private void OnDisable()
    {
        controller.playerInput.Disable();
    }

    public override void Awake()
    {
        base.Awake();
    }


    public override void Update()
    {
        base.Update();

        if (_moveInput.x != 0)
        {
            FSM.SetState<RunState>();
        }

        if (controller.lastPressedJumpTime > 0)
        {
            FSM.SetState<JumpState>();
        }

        if (controller.lastPressedDashTime > 0 && 
            controller.dashRechargeTime < 0)
        {
            FSM.SetState<DashState>();
        }
        
        
    }
}