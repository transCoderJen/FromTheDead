using UnityEngine;

public class BeggarAttackState : EnemyState
{
    private EnemyBeggar enemy;

    public BeggarAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBeggar _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateName = "Attack";
    }

    public override void Update()
    {
        base.Update();

        if (projecticleTriggerCalled)
        {
            enemy.CreateProjectile(enemy.transform.position, enemy.transform.rotation);
            projecticleTriggerCalled = false;
        }

        if (triggerCalled) { stateMachine.ChangeState(enemy.idleState); }
    }

    public override void Exit()
    {
        base.Exit();
    }
}