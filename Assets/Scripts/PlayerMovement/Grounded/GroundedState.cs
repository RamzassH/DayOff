using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundedState : PlayerState
{
    protected Transform playerTransform;
    
    public GroundedState(ChController controller) :
        base(controller)
    {
        playerTransform = controller.playerBody;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        
        #region TIMERS
        
        controller.coyoteTime = controller.data.coyoteTime;
        controller.lastPressedDashTime -= Time.deltaTime;
        controller.dashRechargeTime -= Time.deltaTime;
        
        #endregion

        #region FLAGS
        
        bool isFalling = IsFalling();

        #endregion

        #region MOVE INPUT

        _moveInput = controller.playerInput.Player.Move.ReadValue<Vector2>();
        
        if(controller.playerInput.Player.Jump.ReadValue<float>() > 0.1)
        {
            OnJumpInput();
        }
        if (controller.playerInput.Player.Dash.ReadValue<float>() > 0.1
            && controller.dashRechargeTime < 0)
        {
            OnDashInput();
        }
        
        if (_moveInput.x > 0)
        {
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < 0)
        {
            playerTransform.localScale = new Vector3(-1, 1, 1);
        }

        #endregion

        #region COMBAT INPUT

        if (Input.GetAxis("Fire1") > 0f && !isFalling && 
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.LightAttack);
            return;
        }
        if (Input.GetAxis("Fire2") > 0f && !isFalling &&
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.HeavyAttack);
            return;
        }
        if (Input.GetKey(KeyCode.Q) && !isFalling &&
            (this is IDLE || this is RunState)) 
        {
            FSM.SetState<BattleIDLEState>();
            (FSM.GetCurrentState() as BattleIDLEState).SetStartAction(ComboEvents.Block);
            return;
        }

        #endregion

        #region CHANGE STATE

              
        if (IsTouchWall())
        {
            FSM.SetState<TouchWall>();
        }
        //
        if (IsFalling() && !IsDashing)
        {
            RechargeCoyoteTime();
            FSM.SetState<FallingState>();
        }

        #endregion
    }
}