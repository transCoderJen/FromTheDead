using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
        player.jumpCount++;
        stateName = "Jump";
        player.sr.material = player.jumpMat;
        player.ResetMaterial();
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (player.playerControls.Player.Attack.triggered)
            stateMachine.ChangeState(player.primaryAttack);

        // Reduce upward velocity if space is released
        if (player.playerControls.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (player.jumpCount < player.jumpsAllowed && player.playerControls.Player.Jump.triggered)
            stateMachine.ChangeState(player.jumpState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override public void Exit()
    {
        base.Exit();
        CreateTrailAfterImage();
    }
        
}