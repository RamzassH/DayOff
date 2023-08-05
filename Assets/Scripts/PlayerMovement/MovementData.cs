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

    [Space(5)] 
    public float fastFallGravityMultiplier; //Если игрок нажимает кнопку "вниз", падение быстрее
    public float maxFastFallSpeed; //Максимальная скорость падения при нажатой "вниз

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

    // [Space(5)]
    // public bool doConserveMomentum = true; //Если включено, то есть задержка перед остановкой

    [Space(20)] 
    public float jumpHeight; //Стандартная высота прыжка
    public float jumpTimeToApex; //Время, которое должно пройти перед достижением максимальной высоты прыжка
    public float doubleJumpTimeToApex;
    public float jumpFallingGravityScale;
    public float jumpVelocityFallOff;
    [HideInInspector] public float doubleJumpForce;
    [HideInInspector] public float jumpForce; //Сила, прикладываемая к игроку во время прыжка

    //[Header("Both Jumps")] public int jumpCharge; //Колличество возможных подряд прыжков
    
    public float jumpCutGravityMultiplier; //Множитель накладывается на игрока, если тот нажимает "вниз" во время прыжка

    // [Range(0f, 1)]
    // public float jumpHangGravityMultiplier; //Чем игрок выше во время прыжка, тем слабее сила, которая тянет его вверх
    // public float jumpHangTimeThreshold; // Чем меньше скорость игрока, тем дольше он будет "зависать" в воздухе
    //
    
    // [Space(0.5f)]
    // public float jumpHangAccelerationMult; //Ускорение на игрока во время прыжка 
    // public float jumpHangMaxSpeedMult; //Множитель максимального ускорения на игрока

    
    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //Сила прыжка игрока от стены

    [Space(5f)]
    //[Range(0f, 1f)] public float wallJumpRunLerp; //Если игрок прыгает от стены, то его скорость меньше
    //[Range(0f, 1.5f)] public float wallJumpTime; //Время на которое игрок замедляется после прыжка от стены
    
    //public bool doTurnOnWallJump; //При прижке игрок делает поворот
    
    [Space(20)]
    
    [Header("Slide")]
    public float slideAccel;
    public float slideSpeed;
    // public float slideEndSpeed;
    // public float slideInputBufferTime;
    
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
    public int dashAmount;
    public float dashSpeed;
    
    public float dashSleepTime;
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