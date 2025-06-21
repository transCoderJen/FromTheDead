public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.sr.material = player.deadMat;
        player.isDead = true;
        player.ResetMaterial();
        stateTimer = 2;
        player.ZeroVelocity();
        AudioManager.Instance.PlaySFX("Player_Dead");
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            UI_InGame.Instance.OnGameOver();
        }
    }
}