using System;
using System.Collections;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MovementData Data;

    #region COMPONENTS

    public Rigidbody2D RB { get; private set; }

    #endregion

    #region STATE PARAMETERS

    //Флаги описывающие текущее состояние персонажа
    #region State flags

    // Смотрит ли песронаж направо
    public bool IsFacingRight { get; private set; }
    // Прыгает ли сейчас персонаж
    public bool IsJumping { get; private set; }
    // Прыгает ли сейчас персонаж от стены
    public bool IsWallJumping { get; private set; }
    // Происходит ли сейчас дэщ
    public bool IsDashing { get; private set; }
    // Происходит ли сейчай слайд
    public bool IsSliding { get; private set; }

    #endregion

    //Переменные таймеры нужны для "времени кайота"
    #region Timers

    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    #endregion
    
    // Приватные  переменные для прыжка
    #region Jump

    //jumpCut - Прерывание прыжка, то есть персонаж прыгает не на полную амплетуду
    private bool _isJumpCut;
    private bool _isJumpFalling;

    #endregion

    // Приватные переменные для прыжка от стены
    #region Wall Jump
    
    private float _wallJumpStartTime;
    // Мы хотим знать направление в которое мы прыгаем от стены
    private int _lastWallJumpDir;

    #endregion

    // Dash
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;

    #endregion

    #region INPUT PARAMETERS

    private Vector2 _moveInput;

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }

    #endregion

    // Точки зачекивания на те или иные слои(дочерние объекты персонажа)
    #region CHECK PARAMETERS

    [Header("Checks")] 
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    [Space(5)] 
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    #endregion

    //Слои и тэги
    #region LAYERS & TAGS

    [Header("Layers & Tags")] [SerializeField]
    private LayerMask _groundLayer;

    #endregion

    private void Awake()
    {
        // Получаем доступ к компоненту Rigidbody на персонаже
        RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Изменяем параметр гравитации, действующей на нашего персонажа
        SetGravityScale(Data.gravityScale);
        // По умолчанию персонаж смотрит вперед
        IsFacingRight = true;
    }

    private void Update()
    {
        // Запускаем наши таймеры
        #region TIMERS

        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        
        #endregion
        
        #region INPUT

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Если был ввод по оси x, то проверяем направление нашего персонажа
        if (_moveInput.x != 0)
        {
            CheckDirectionToFace(_moveInput.x > 0);
        }

        // При нажатии на прыжок, запускаем наше "окно" прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        #endregion

        #region COLISION CHECK

        // Если персонаж не прыгает и находится на земле, то мы ему даем время "кайота"
        if (!IsDashing && !IsJumping)
        {
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            {
                LastOnGroundTime = Data.coyoteTime;
            }
        }
        #endregion

        #region JUMP CHECKS
        
        // Мы узнаем, что наш персонаж падает после прыжка
        if (IsJumping && RB.velocity.y < 0)
        {
            // Здесь мы даем понять, что наш персонаж закончил прыжок
            IsJumping = false;
            // Убеждаемся, что персонаж не прыгает от стены
            if (!IsWallJumping)
            {
                // Даем понять, что сейчас идет стадия падения после прыжка
                _isJumpFalling = true;
            }
        }
        
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;

            if (!IsJumping)
            {
                _isJumpFalling = false;
            }
        }
        
        // Мы не прыгаем, если дэшимся
        if (!IsDashing)
        {
            // Если можем прыгнуть и окно для прыжка еще аткульно
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;

                _wallJumpStartTime = Time.time;
                // Определяем напралвение прыжка нашего персонажа
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
                
                WallJump(_lastWallJumpDir);
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        Run(0.5f);
    }

    #region INPUT CALLBACKS
    
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    // public void OnJumpUpInput()
    // {
    //     if (CanJumpCut() || CanWallJumpCut())
    //     {
    //         _isJumpCut = true;
    //     }
    // }

    public void OnDashInput()
    {
        LastPressedDashTime = Data.dashInputBufferTime;
    }

    #endregion

    #region GENERAL METHODS

    private void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    
    private void Sleep(float duration)
    {
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }
    
    #endregion

    #region RUN METHODS

    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;

        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region CALCULATE ACCELERATEION

        float accelerationRate;
        
        if (LastOnGroundTime > 0)
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f)
                ? Data.runAccelerationAmount
                : Data.runDeccelerationAmount;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f)
                ? Data.runAccelerationAmount * Data.accelerationInAir
                : Data.runDeccelerationAmount * Data.deccelerationInAir;

        #endregion

        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelerationRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        
        #region Conserve Momentum

        if (Data.doConserveMomentum &&
            Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) &&
            Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) &&
            Mathf.Abs(targetSpeed) > 0.01f &&
            LastOnGroundTime < 0)
        {
            accelerationRate = 0;
        }

        #endregion

        float speedDif = targetSpeed - RB.velocity.x;

        float movement = speedDif * accelerationRate;

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }

    #endregion

    #region JUMP METHODS

    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump

        float force = Data.jumpForce;

        if (RB.velocity.y < 0)
        {
            force -= RB.velocity.y;
        }
        
        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        #endregion
    }

    private void WallJump(int dir)
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallLeftTime = 0;
        LastOnWallRightTime = 0;

        #region Perform Wall Jump

        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
        {
            force.x -= RB.velocity.x;
        }

        if (RB.velocity.y < 0)
        {
            force.y -= RB.velocity.y;
        }
        
        RB.AddForce(force, ForceMode2D.Impulse);
        
        #endregion
    }

    #endregion

    #region CHECK METHODS

    
    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 &&
               LastOnWallTime > 0 &&
               LastOnGroundTime <= 0 &&
               (!IsWallJumping ||
                (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) ||
                (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0; 
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    #endregion
}