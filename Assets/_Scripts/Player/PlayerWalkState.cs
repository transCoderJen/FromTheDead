using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateName = "Walk";
        player.sr.material = player.runMat;
        if (player.IsGroundDetected())
            player.fx.CreateDustParticles(DustParticleType.Running);
        AudioManager.Instance.PlaySFX("Player_Footsteps");
    }    

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.Instance.StopSFX("Player_Footsteps");
    }
}