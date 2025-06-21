using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float defaultGravityScale;
    private float apexTimer;

    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.coyoteTimeDuration;
        apexTimer = player.apexHangTime;
        // Store the default gravity scale and increase it for the fall state
        defaultGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0; // Increase gravity (adjust multiplier as needed)
        stateName = "Fall";
        player.ResetMaterial();
    }

    public override void Update()
    {
        base.Update();
        apexTimer -= Time.deltaTime;

        if (apexTimer < 0)
        {
            player.rb.gravityScale = defaultGravityScale * 1.5f; // Increase gravity (adjust multiplier as needed)
        }

        if (player.playerControls.Player.Jump.triggered && stateTimer > 0 && player.stateMachine.previousStateName == "PlayerWalkState" && player.jumpCount < 2)
                stateMachine.ChangeState(player.jumpState);

        if (player.IsGroundDetected())
        {
            player.fx.CreateDustParticles(DustParticleType.Landing);
            stateMachine.ChangeState(player.idleState);
        }

        if (player.jumpCount < player.jumpsAllowed && player.playerControls.Player.Jump.triggered)
            stateMachine.ChangeState(player.jumpState);
            
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocity.y);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        // Reset gravity scale to its default value
        player.rb.gravityScale = defaultGravityScale;
    }
}