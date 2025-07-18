using System.Collections;
using TMPro;
using UnityEngine;


public class Player : Entity
{
    [SerializeField] private bool isClone = false;
    public PlayerControlller playerControls;
    public SkillManager skill { get; private set; }

    [Header("Materials")]
    public Material idleMat;
    public Material attack1Mat;
    public Material attack2Mat;
    public Material attack3Mat;
    public Material respawnHolyMat;
    public Material dashMat;
    public Material runMat;
    public Material jumpMat;
    public Material healMat;
    public Material deadMat;
    public Material laserMat;


    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;

    [Header("Move Info")]
    public Transform respawnPosition;
    public float moveSpeed = 8f;
    public float jumpForce;
    public int jumpCount = 0;
    public int jumpsAllowed = 2;
    public float coyoteTimeDuration;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    public float apexHangTime;
    public float swordReturnImpact;


    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    private float defaultDashSpeed;

    public GameObject sword { get; private set; }

    #region Player State Machine
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerRespawnHolyState respawnHolyState { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerHealState healState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerFlashState flashState { get; private set; }
    public PlayerLaserState laserState { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    #endregion

    [HideInInspector] public bool nextAttackQueued = false; // Flag to store input for the next attack
    public bool isBusy { get; private set; }
    public PlayerFX fx { get; private set; }

    private PulseIntensity pulseIntensity;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControlller();

        stateMachine = new PlayerStateMachine();

        //Movement States
        idleState = new PlayerIdleState(this, stateMachine, "idle");
        walkState = new PlayerWalkState(this, stateMachine, "walk");
        fallState = new PlayerFallState(this, stateMachine, "jump");
        jumpState = new PlayerJumpState(this, stateMachine, "jump");

        //Respawn States
        respawnHolyState = new PlayerRespawnHolyState(this, stateMachine, "respawnHoly");

        //Attack States
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "counter");

        // Skill States
        dashState = new PlayerDashState(this, stateMachine, "dash");
        healState = new PlayerHealState(this, stateMachine, "heal");
        flashState = new PlayerFlashState(this, stateMachine, "flash");
        laserState = new PlayerLaserState(this, stateMachine, "laser");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        //Dead State
        deadState = new PlayerDeadState(this, stateMachine, "dead");

        pulseIntensity = GetComponent<PulseIntensity>();

        playerControls.Player.Dash.performed += ctx => Dash();
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.Instance;

        if (!isClone)
        {
            stateMachine.Initialize(respawnHolyState);
        }
        else
        {
            stateMachine.Initialize(idleState);
        }

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        fx = GetComponent<PlayerFX>();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        if (stats.currentHealth < 0)
            Die();

        stateMachine.currentState.Update();

        //Debugging
        UpdateStateText();
    }

    private void UpdateStateText()
    {
        if (debugStateText == null)
            return;

        debugStateText.text = stateMachine.currentStateName;

        debugStateText.GetComponent<Transform>().position = transform.position + new Vector3(0, 2, 0);
    }

    private void Dash()
    {
        if (IsWallDetected())
            return;


        if (SkillManager.Instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }

    public void ResetMaterial()
    {
        pulseIntensity.ResetMaterial();
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        Debug.Log("Player is dead");
        stateMachine.ChangeState(deadState);
    }

    public void GeneratePulse()
    {
        pulseIntensity.TriggerPulse();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    
    public void CatchSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
}
