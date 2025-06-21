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
        AudioManager.Instance.StopBossMusic();
        

        if (Random.Range(0, 2) == 0)
        {
            player.anim.SetBool("respawnHoly", false);
            player.anim.SetBool("respawnHell", true);
            player.sr.material = player.deadMat;

        }
        else
        {

            player.sr.material = player.respawnHolyMat;
        }


        player.transform.position = player.respawnPosition.position;
        player.ZeroVelocity();

        player.SlowEntityBy(1, .2f);
        
        player.ResetMaterial();
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
        player.anim.SetBool("respawnHoly", false);
        player.anim.SetBool("respawnHell", false);
        base.Exit();
    }
}