using UnityEngine;

public class AirState : State
{
    static protected bool IsDoubleJumped;
    public AirState(ChController controller) :
        base(controller)
    {
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}