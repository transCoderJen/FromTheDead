using System.Collections;
using TMPro;
using UnityEngine;


public class Player : Entity
{
    [Header("Materials")]
    public Material idleMat;
    public Material attack1Mat;
    public Material attack2Mat;
    public Material attack3Mat;
    public Material respawnHolyMat;
    public Material dashMat;
    public Material runMat;
    public Material jumpMat;


    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;

    [Header("Move Info")]
    public Transform respawnPosition;
    public float moveSpeed = 8f;
    public float jumpForce;
    public int jumpCount = 0;
    public float coyoteTimeDuration;
    private float defaultMoveSpeed;
    private float defaultJumpForce;


    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set;}
    private float defaultDashSpeed;

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
    #endregion

    [HideInInspector]public bool nextAttackQueued = false; // Flag to store input for the next attack
    public bool isBusy { get; private set; }
    public PlayerFX fx { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "idle");
        walkState = new PlayerWalkState(this, stateMachine, "walk");
        fallState = new PlayerFallState(this, stateMachine, "jump");
        jumpState = new PlayerJumpState(this, stateMachine, "jump");
        dashState = new PlayerDashState(this, stateMachine, "dash");
        
        respawnHolyState = new PlayerRespawnHolyState(this, stateMachine, "respawnHoly");

        primaryAttack= new PlayerPrimaryAttackState(this, stateMachine, "attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "counter");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(respawnHolyState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        fx = GetComponent<PlayerFX>();
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

        stateMachine.currentState.Update();
        CheckForDashInput();
        //Debugging
        UpdateStateText();
    }

    private void UpdateStateText()
    {
        debugStateText.text = stateMachine.currentStateName;

        debugStateText.GetComponent<Transform>().position = transform.position + new Vector3(0, 2, 0);
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;
        
        if (stateMachine.currentState == jumpState || stateMachine.currentState == fallState)
            return;
            
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }

   public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
