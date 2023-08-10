using System;
using UnityEngine;

public class AirState : State
{
    static protected bool IsDoubleJumped;
    public AirState(ChController controller) :
        base(controller)
    {
    }

    public override void Update()
    {
        base.Update();
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        if (_moveInput.x > 0)
        {
            controller.playerBody.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < 0)
        {
            controller.playerBody.localScale = new Vector3(-1, 1, 1);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (RB.velocity.y < -Data.maxFallSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, -Data.maxFallSpeed);
        }
        if (RB.velocity.y > Data.maxVelocityValueY)
        {
            RB.velocity = new Vector2(RB.velocity.x, Data.maxVelocityValueY);
        }

        if (Math.Abs(RB.velocity.x) > Data.maxVelocityValueX)
        {
            RB.velocity = new Vector2(Data.maxVelocityValueX * Math.Sign(RB.velocity.x), RB.velocity.y);
        }
    }
}