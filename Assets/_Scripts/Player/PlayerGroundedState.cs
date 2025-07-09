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
        if (UI.Instance.IsMenuOpened())
            return;
            
        base.Update();


        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.fallState);
        else
            player.jumpCount = 0;

        if (player.playerControls.Player.Jump.triggered)
        {
            stateMachine.ChangeState(player.jumpState);
            // player.fx.CreateDustParticles(DustParticleType.Jump);
        }

        if(player.playerControls.Player.Counter.triggered && HasNoSword() && SkillManager.Instance.canUseSword)
            stateMachine.ChangeState(player.aimSword);

        if (player.playerControls.Player.Counter.triggered && SkillManager.Instance.parry.CanUseSkill() && SkillManager.Instance.canUseParry)
            stateMachine.ChangeState(player.counterAttack);

        if (player.playerControls.Player.Attack.triggered)
            stateMachine.ChangeState(player.primaryAttack);

        if (player.playerControls.Player.Spell1.triggered)
            SkillManager.Instance.skill1.spellPrefab.GetComponent<ISpell>().CanUseSkill();

        if (player.playerControls.Player.Spell2.triggered)
            SkillManager.Instance.skill2.spellPrefab.GetComponent<ISpell>().CanUseSkill();

        if (Input.GetKeyDown(KeyCode.T))
        {
            SkillManager.Instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));

        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override public void Exit()
    {
        base.Exit();
    }
    
    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }

        if(!player.sword.GetComponent<SwordSkillController>().canRotate)
            player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
