using UnityEngine;
public class HydraDeathState : EnemyState
{
    private EnemyHydra enemy;

    public HydraDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyHydra _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.pcd.enabled = false;

        stateTimer = .1f;
        stateName = "Dead";
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 10);
    }

    public override void Exit()
    {
        base.Exit();
    }
}