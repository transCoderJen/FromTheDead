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
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        // Reduce upward velocity if space is released
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
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