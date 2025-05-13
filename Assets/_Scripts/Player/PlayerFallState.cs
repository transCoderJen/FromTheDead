using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float defaultGravityScale;

    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.coyoteTimeDuration;

        // Store the default gravity scale and increase it for the fall state
        defaultGravityScale = player.rb.gravityScale;
        player.rb.gravityScale *= 1.5f; // Increase gravity (adjust multiplier as needed)
        stateName = "Fall";
    }

    public override void Update()
    {
        base.Update();

        Debug.Log(player.stateMachine.previousStateName);

        if (Input.GetKey(KeyCode.Space) && stateTimer > 0 && player.stateMachine.previousStateName == "PlayerWalkState" && player.jumpCount < 2)
            stateMachine.ChangeState(player.jumpState);

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            player.fx.CreateDustParticles(DustParticleType.Landing);
        }

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