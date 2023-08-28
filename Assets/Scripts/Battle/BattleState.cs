using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
    private const int MAX_ENEMY_TO_DAMAGE_COUNT = 10;
    private List<Collider2D> _damagedColliders;

    public BattleState(ChController controller) : base(controller)
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

    public override void Update()
    {
        base.Update();

        float moveInputX = Input.GetAxisRaw("Horizontal");
        bool isFalling = IsFalling();
        if (isFalling)
        {
            FSM.SetState<FallingState>();
            return;
        }

        if (moveInputX != 0 && this is BattleIDLEState)
        {
            FSM.SetState<RunState>();
            return;
        }

        if (Input.GetKey(KeyCode.Space) && this is BattleIDLEState)
        {
            FSM.SetState<JumpState>();
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) && this is BattleIDLEState)
        {
            FSM.SetState<DashState>();
            return;
        }
    }

    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[MAX_ENEMY_TO_DAMAGE_COUNT];
        _damagedColliders = new List<Collider2D>();
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.useTriggers = true;
        filter.SetLayerMask(64);
        int colliderCount = Physics2D.OverlapCollider(controller.swordCollider, filter, collidersToDamage);

        if (colliderCount != 0)
        {
            for (int i = 0; i < colliderCount; i++)
            {
                if (_damagedColliders.Contains(collidersToDamage[i]))
                {
                    break;
                }
                
                Enemy enemyToAttack = collidersToDamage[i].GetComponentInParent<Enemy>();
                Sigment sigment = collidersToDamage[i].GetComponent<SigmentInfo>().sigment;
                if (enemyToAttack is not null)
                {
                    enemyToAttack.GetDamage(30, sigment);
                }
                _damagedColliders.Add(collidersToDamage[i]);
            }
        }

        //TODO сделать возможность атаковать сразу несколько противников
    }
}