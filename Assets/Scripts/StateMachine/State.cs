using Unity.VisualScripting;
using UnityEngine;

public interface State
{
    public void Enter();

    public void Exit();

    public void Update();

    public void FixedUpdate();

    public void Awake();

    public void LateUpdate();
}