using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Data")] //Создаем меню для настройки передвижения игрока
public class MovementData : ScriptableObject
{
    [Header("Gravity")] 
    [HideInInspector] public float gravityStrength; //Сила притяжения
    [HideInInspector] public float gravityScale; //Множитель гравитации

    [Space(5)]
    public float fallGravityMultiplier; //Множитель на gravityScale, когда персонаж падает
    public float maxFallSpeed; //Максимальная скорость падения игрока
    
    [Space(20)]

    [Header("Run")] 
    public float runMaxSpeed; //Максимальная скорость игрок в n-й момент времени
    public float runAcceleration; //Ускорение игрока
    public float runDecceleration; //Игрок замедляется перед тем, как остановится
    [HideInInspector] public float runAccelerationAmount; //Фактическое ускорение, которое действует на игрока
    [HideInInspector] public float runDeccelerationAmount; //Фактическое замедление игрока, перед остановкой.

    [Space(10)]
    
    [Range(0.01f, 1)] public float accelerationInAir; //Ускорение, которое действует на игрока в воздухе
    [Range(0.01f, 1)] public float deccelerationInAir;

  

    [Space(20)]
    [Header("Jump")]
    public float jumpHeight; //Стандартная высота прыжка
    public float jumpTimeToApex; //Время, которое должно пройти перед достижением максимальной высоты прыжка
    public float doubleJumpTimeToApex;
    public float jumpFallingGravityScale;
    public float jumpVelocityFallOff;
    public float jumpHorizontalSpeed;
    public float maxVelocityValueX;
    public float jumpCutBlockTimeBuffer;
    [HideInInspector] public float doubleJumpForce;
    [HideInInspector] public float jumpForce; //Сила, прикладываемая к игроку во время прыжка


    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //Сила прыжка игрока от стены

    [Space(5f)]
    [Space(20)]
    
    [Header("Slide")]
    public float slideAccel;
    public float slideSpeed;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Игрок не на земле, но в этот промежуток времени еще может прыгнуть
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; // Насколько я понял, эта нужна для: 
    /*
     * Мы нажимаем кнопку прыжка, и если мы находимся, например в воздухе, то в течении определенного времени,
     * которое мы задаем этой переменной, персонаж прыгнет автоматически, как только будет на замеле.
     * Это нужно для того, чтобы игрок не ебашил 20 раз по пробелу, дожидаясь момента, пока юнити осознает,
     * что персонаж на земле/*
     */

    [Space(20)]

    [Header("Dash")]
    public float dashSpeed;
    
    [Space(5)]
    public float dashAttackTime;
    [Space(5)]
    public float dashEndTime;
    public Vector2 dashEndSpeed;
    [Range(0f, 1f)] public float dashEndRunLerp;
    [Space(5)]
    public float dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    private void OnValidate()
    {
        //Считаем силу притяжения по формуле (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (Mathf.Pow(jumpTimeToApex,2));
        
        gravityScale = gravityStrength / Physics2D.gravity.y;
        
        runAccelerationAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelerationAmount = (50 * runDecceleration) / runMaxSpeed;
        
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
        doubleJumpForce = Mathf.Abs(gravityStrength) * doubleJumpTimeToApex; 

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}