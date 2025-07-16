using UnityEngine;

public class SlimeAttackState : EnemyState
{
    private EnemySlime enemy;
    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetInteger("AttackSelect", Random.Range(0, 3));
    }

    public override void Update()
    {
        base.Update();
        
        enemy.ZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);


    }

    public override void Exit()
    {
        base.Exit();
    }
}
