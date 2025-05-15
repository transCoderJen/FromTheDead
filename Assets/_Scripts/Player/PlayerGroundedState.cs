using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpCount = 0;
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.fallState);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            //TODO player.fx.CreateDustParticles(DustParticleType.Jump)
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
                player.fx.ScreenShake(player.fx.lightShakePower);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override public void Exit()
    {
        base.Exit();
    }
}
