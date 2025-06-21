public class PlayerLaserState : PlayerState
{
    public PlayerLaserState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.sr.material = player.laserMat;
        player.ResetMaterial();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}