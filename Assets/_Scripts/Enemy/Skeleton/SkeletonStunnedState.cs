
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;
    private GloryKillController glorykill;
    
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);
        stateTimer = enemy.stunDuration;
        enemy.SetVelocity(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
        enemy.Flip();
        stateName = "Stunned";
        glorykill = enemy.GetComponent<GloryKillController>();
        glorykill.CreateHoykey(enemy);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.CancelInvoke();
    }

    public override void Update()
    {
        base.Update();

        if (glorykill.WasHotkeyPressed())
        {
            Debug.Log("GLORY KILL ACTIVATED");
            glorykill.HotkeyPressed(false);
            PlayerManager.Instance.player.stateMachine.ChangeState(PlayerManager.Instance.player.laserState);
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
            glorykill.DestroyHotHeys();

        }
    }

}
