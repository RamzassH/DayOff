using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tmpMovement : MonoBehaviour
{
    public MovementData Data;
    private StateMachine Fsm;
    private Rigidbody2D RB;

    public Transform groundCheck;
    private GameObject obj;

    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Fsm = new StateMachine();
        Fsm.AddState(new IDLE(Fsm, Data, RB, groundCheck, new (0.2f, 0.1f)));
        Fsm.AddState(new RunState(Fsm, RB, Data));
        Fsm.AddState(new JumpState(Fsm, Data, RB));
        // State tmp = new DashState(Fsm, Data, RB);
        // tmp.gameObject.AddComponent<DashState>();
        obj.AddComponent<DashState>();
        Fsm.AddState(new DashState(Fsm, Data, RB));
    }

    void Start()
    {
        Fsm.SetState<IDLE>();
    }
    
    void Update()
    {
        Fsm.Update();
    }

    private void FixedUpdate()
    {
        Fsm.FixedUpdate();
        
    }
}
