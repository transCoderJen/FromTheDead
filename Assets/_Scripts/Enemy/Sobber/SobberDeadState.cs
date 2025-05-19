using UnityEngine;

public class SobberDeadState : EnemyState
{
    private EnemySobber enemy;

    public SobberDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySobber _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .1f;
        stateName = "Dead";
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 10);
            rb.gravityScale = 5;
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}