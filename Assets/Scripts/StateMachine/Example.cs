using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPVersion
{
    public class Example : MonoBehaviour
    {
        //// Это пример того, что в себе содержит юнит на
        //// машине состояний
        //
        //public StateMachine movementSM; // Машина состояний
        //                                // Все состояния, которые может принимать юнит
        //                                // Скорее всего лучше для каждого прописать гетор.
        //public StandingState standing;
        //public DuckingState ducking;
        //public JumpingState jumping;
        //
        //private void Start()
        //{
        //    // Инициализация, только конструктор принимает в начале ссыслку
        //    // на сам юнит, а потом машина
        //    standing = new StandingState(this, movementSM);
        //    ducking = new DuckingState(this, movementSM);
        //    jumping = new JumpingState(this, movementSM);
        //
        //    // Инициализация машины
        //    movementSM = new StateMachine(standing);
        //}
        //
        //private void Update()
        //{
        //    movementSM.CurrentState.HandleInput();
        //
        //    movementSM.CurrentState.LogicUpdate();
        //}
        //
        //private void FixedUpdate()
        //{
        //    movementSM.CurrentState.PhysicsUpdate();
        //}
    }
}