using UnityEditor;
using UnityEngine;

public class PlayerRespawnHolyState : PlayerState
{
    public PlayerRespawnHolyState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.sr.material = player.respawnHolyMat;
        player.transform.position = player.respawnPosition.position;
        player.ZeroVelocity();

        player.SlowEntityBy(1,.2f);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
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