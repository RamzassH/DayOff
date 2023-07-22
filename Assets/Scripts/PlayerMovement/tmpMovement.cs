using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class tmpMovement : MonoBehaviour
{
    public MovementData Data;
    private StateMachine FSM;
    private Rigidbody2D RB;

    public Transform groundCheck;
    public Transform rightWallCheck;
    public Transform leftWallCheck;

    public TextMeshProUGUI text;
    
    private GameObject obj;
    
    [SerializeField] private GameObject _coroutinePrefab;

    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        FSM = new StateMachine();

        #region GROUND

        FSM.AddState(new IDLE(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new RunState(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new JumpState(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new DashState(FSM, RB, Data, transform, groundCheck, rightWallCheck, leftWallCheck));
        
        #endregion
        
        #region AIR

        FSM.AddState(new UpState(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new DoubleJump(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new DoubleJumpUpState(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new FallingState(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new JumpCutState(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        
        #endregion

        #region ON WALL

        FSM.AddState(new TouchWall(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new OnWall(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new Grap(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new Slide(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        FSM.AddState(new WallJumpState(FSM, RB, Data, groundCheck, rightWallCheck, leftWallCheck));
        
        #endregion
    }

    void Start()
    {
        FSM.SetState<IDLE>();
    }
    
    void Update()
    {
        FSM.Update();
        text.text = FSM.GetCurrentState().ToString();
    }

    private void FixedUpdate()
    {
        FSM.FixedUpdate();
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.position, new Vector2(0.5f, 0.03f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(rightWallCheck.position, new Vector2(0.03f, 1f));
        Gizmos.DrawCube(leftWallCheck.position, new Vector2(0.03f, 1f));

    }


}
