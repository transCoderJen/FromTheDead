using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ResetMaterial();
    }

    public override void Update()
    {
        base.Update();


        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.fallState);
        else
            player.jumpCount = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            // player.fx.CreateDustParticles(DustParticleType.Jump);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.counterAttack);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);
            
        if (Input.GetKeyDown(KeyCode.E))
            SkillManager.Instance.heal.CanUseSkill();
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
