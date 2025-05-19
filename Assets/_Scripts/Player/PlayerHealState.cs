public class PlayerHealState : PlayerState
{
    public PlayerHealState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.stats.MakeInvincible(true);
        player.sr.material = player.healMat;
        player.ResetMaterial();
        AudioManager.Instance.PlaySFX("heal");
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.stats.MakeInvincible(false);
    }
}