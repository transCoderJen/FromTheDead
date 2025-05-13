using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateName = "Idle";
        if (stateMachine.previousStateName == "Dash")
            player.SetVelocity(player.facingDir * 20f, rb.linearVelocity.y);
        else
            player.ZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && !player.isBusy)
            player.stateMachine.ChangeState(player.walkState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}