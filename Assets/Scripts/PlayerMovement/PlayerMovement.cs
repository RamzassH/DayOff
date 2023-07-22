using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region STATE MACHINE

    private StateMachine _stateMachine;

    #endregion

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

    // Переменные таймеры нужны для "времени кайота"

    #region Timers

    // Таймер последнего нахождения персонажа на земле(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnGroundTime { get; private set; }

    // Таймер последнего нахождения персонажа на стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallTime { get; private set; }

    // Таймер последнего нахождения персонажа на правой стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallRightTime { get; private set; }

    // Таймер последнего нахождения персонажа на левой стене(если больше 0, значит находится на замле, иначе - нет)
    public float LastOnWallLeftTime { get; private set; }

    #endregion

    // Приватные  переменные для прыжка

    #region Jump

    // jumpCut - Прерывание прыжка, то есть персонаж прыгает не на полную амплетуду
    private bool _isJumpCut;

    // Падает ли сейчас персонаж
    private bool _isJumpFalling;

    // Кол-во зарядов прыжков персонажа(нужно для дабл джампа).
    // TODO можно сделать бонусы, которые временно добавляют кол-во впрыжков. 
    private int _jumpCharge;

    #endregion

    // Приватные переменные для прыжка от стены

    #region Wall Jump

    private float _wallJumpStartTime;

    // Мы хотим знать направление в которое мы прыгаем от стены
    private int _lastWallJumpDir;

    #endregion

    // Dash

    #region Dash

    private int _dashesLeft;

    // Перезарядка дэша
    private bool _dashRefilling;

    // Направление последнего дэша
    private Vector2 _lastDashDir;

    // Происходит ли сейчас Дэш
    private bool _isDashAttacking;

    #endregion

    #endregion

    #region INPUT PARAMETERS

    private Vector2 _moveInput;

    // Таймер послденего считывания кнопки "прыжка"
    public float LastPressedJumpTime { get; private set; }

    // Таймер послденего считывания кнопки "дэша"
    public float LastPressedDashTime { get; private set; }

    #endregion

    // Точки зачекивания на те или иные слои(дочерние объекты персонажа)

    #region CHECK PARAMETERS

    [Header("Checks")] [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    [Space(5)] [SerializeField] private Transform _frontWallCheckPoint;
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
        #region StateMachine

        // Создаем машину состояний
        _stateMachine = new StateMachine();

        // Добавляем состояние я в машину;
        //TODO Добавить состоянияни
        // Устанавливаем начальное состояние 
        _stateMachine.SetState<IDLE>();

        #endregion

        // Изменяем параметр гравитации, действующей на нашего персонажа
        SetGravityScale(Data.gravityScale);
        // По умолчанию персонаж смотрит вперед
        IsFacingRight = true;
    }

    private void Update()
    {
        _stateMachine.Update();
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
            //CheckDirectionToFace(_moveInput.x > 0);
        }

        // При нажатии на прыжок, запускаем наше "окно" прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (_moveInput.y < 0)
        {
            DoJumpCut();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) ||
            Input.GetKeyDown(KeyCode.RightShift))
        {
            OnDashInput();
        }

        #endregion

        #region COLISION CHECK

        // Если персонаж не прыгает и находится на земле, то мы ему даем время "кайота"
        if (!IsDashing && !IsJumping)
        {
            // Если персонаж на заемле, то постоянно накидываем ему время "кайота",
            // Т.е. если персонаж уходит с платформы, то может прыгнуть, пока время "кайота" не выйдет
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping)
            {
                JumpRefill(Data.jumpCharge);
                LastOnGroundTime = Data.coyoteTime;
            }

            // То же, что и с землей, только для стен
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                 || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) &&
                     !IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallRightTime = Data.coyoteTime;
            }

            //Right Wall Check
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) &&
                  !IsFacingRight)
                 || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) &&
                     IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallLeftTime = Data.coyoteTime;
            }

            // Две проверки нужны, чтобы не было необходимости проверять от какой стены мы сейчас отпрегнули
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
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

        if (LastOnGroundTime > 0)
        {
            JumpRefill(Data.jumpCharge);
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            _isJumpFalling = false;
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
                //Jump();
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

        #region SLIDE CHECKS

        if (CanSlide() &&
            ((LastOnWallLeftTime > 0 && _moveInput.x < 0) ||
             (LastOnWallRightTime > 0 && _moveInput.x > 0)))
        {
            IsSliding = true;
        }
        else
        {
            IsSliding = false;
        }

        #endregion

        #region DASH CHECKS

        if (CanDash() && LastPressedDashTime > 0)
        {
            // Можно зафризить игру на долю секунды, чтобы добпвить Epic...
            Sleep(Data.dashSleepTime);

            // Если направление Дэша не выбрано, то дэшимся в текущем направлении
            if (_moveInput != Vector2.zero)
            {
                _lastDashDir = _moveInput;
            }
            else
            {
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }


            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(StartDash), _lastDashDir);
        }

        #endregion

        #region GRAVITY

        if (!_isDashAttacking)
        {
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                // Если жмем вниз во время падения, то падаем быстрее
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMultiplier);
                // Ограничиваем максимальную скорость падения
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                RB.velocity = new Vector2(RB.velocity.x, -Mathf.Max(RB.velocity.y, Data.maxFastFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) &&
                     Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMultiplier);
            }
            else if (RB.velocity.y < 0)
            {
                SetGravityScale(Data.gravityScale * Data.fallGravityMultiplier);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                SetGravityScale(Data.gravityScale);
            }
        }
        else
        {
            // Пока дэшимся, гравитацию вырубаем
            SetGravityScale(0);
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                Run(Data.wallJumpRunLerp);
            }
            else
            {
                Run(1);
            }
        }
        else if (_isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
        }

        if (IsSliding)
        {
            Slide();
        }
    }

    #region INPUT CALLBACKS

    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void DoJumpCut()
    {
        if (CanJumpCut() || CanWallJumpCut())
        {
            _isJumpCut = true;
        }
    }

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
        yield return new WaitForSecondsRealtime(duration);
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

        _jumpCharge--;
        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        #endregion
    }

    private void JumpRefill(int charge)
    {
        _jumpCharge = charge;
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

    #region DASH METHODS

    private IEnumerator StartDash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;

            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        IsDashing = false;
    }


    private IEnumerator RefillDash()
    {
        _dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
    }

    #endregion

    #region OTHER

    private void Slide()
    {
        // float speedDif = Data.slideSpeed - RB.velocity.y;
        // float movement = speedDif * Data.slideAccel;
        //
        // movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
        //     Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        //
        // RB.AddForce(movement * Vector2.up);
    }

    #endregion

    #region CHECK METHODS

    private bool CanJump()
    {
        return _jumpCharge > 0 && (LastOnGroundTime > 0 && !IsJumping ||
                                   (IsJumping || _isJumpFalling) && LastOnWallTime <= 0);
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

    private bool CanDash()
    {
        if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // public void CheckDirectionToFace(bool isMovingRight)
    // {
    //     if (isMovingRight != IsFacingRight)
    //         Turn();
    // }

    #endregion

    #region EDITOR METHODS

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }

    #endregion
}